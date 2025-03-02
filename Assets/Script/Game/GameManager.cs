using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;

[System.Serializable]
public class PlayerControllerUI
{
    [SerializeField]
    public Text selectButtonText = null;

    [SerializeField]
    public Canvas buttonVisibleCanvas = null;

    [SerializeField]
    public Canvas buttonVisibleCanvasController = null;

}

public class GameManager : MonoBehaviour
{
    public enum MainStep
    {
        TurnStart,
        P_UseItem,
        P_PutStone,
        P_PlayMagic,
        OP_PlayMagic,
        P_SetTrap,
        TurnEnd
    }

    public enum PlayMagicStep
    {
        StartStep,
        U_SelectCard,
        U_OpenCard,
        OU_CardCheck,
        OU_OpenCard,
        U_TrapCheck,
        U_OpenTrap,
        EndStep
    }

    [SerializeField]
    Text message = null;

    [SerializeField]
    PlayerControllerUI verticalPlayerUIs = null, landscapePlayerUIs = null;

    public PlayerControllerUI verticalPlayerControllerUIs { get { return verticalPlayerUIs; } }
    public PlayerControllerUI landscapePlayerControllerUIs { get { return landscapePlayerUIs; } }

    [SerializeField, ReadOnly]
    List<Player> players = new List<Player>();

    [SerializeField]
    Player playerPrefab = null;

    [SerializeField]
    StoneBoardManager stoneBoard = null;

    [SerializeField]
    Camera cameraObject = null;

    [SerializeField, ReadOnly]
    int nowPlayerCount = 0;

    [SerializeField, ReadOnly]
    ScriptManager scriptManager = new ScriptManager();

    [SerializeField, ReadOnly]
    List<CardScript> stack = new List<CardScript>();


    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectStone = null;

    public StoneBoardManager stoneBoardObj { get { return stoneBoard; } }

    [SerializeField, ReadOnly]
    Manager manager = Manager.ins;

    bool initFlg { get; set; } = false;

    MainStep mainStep = MainStep.TurnStart;
    PlayMagicStep playMagicStep = PlayMagicStep.StartStep;

    public void SelectStonePos(int _x,int _y)
    {
        scriptManager.SelectTargetPos(_x + (_y * stoneBoardObj.HOLYZONTAL_SIZE));
    }

    public void SetMessate(string _message)
    {
        if (message == null) return;
        message.text = _message;
    }

    public Player GetPlayer(int _num)
    {
        if (_num < 0) return null;
        if (players.Count >= _num) return null;
        return players[_num];
    }

    // Start is called before the first frame update
    void Start()
    {
        if (stoneBoard != null) stoneBoard.Init();

        CreatePlayer();

        for (int i = 1; i < Manager.MAX_GMAE_PLAYER && i < manager.cpuFlgs.Length; i++)
        {
            var cpuFlg = manager.cpuFlgs[i - 1];
            CreateCPUPlayer(cpuFlg);
            CreateNetworkPlayer(cpuFlg);
        }

    }

    // Update is called once per frame
    void Update()
    {

        StartDice();

        if (!initFlg) return;

        RunTurnStep();

        if (!scriptManager.isRunScript) return;

        var controller = players[nowPlayerCount].GetComponent<ControllerBase>();

        scriptManager.RunScript(controller, this);
    }


    void RunTurnStep()
    {
        if (scriptManager.isRunScript) return;

        TurnStart();

        P_UseItem();

        TurnEnd();

    }

    void StartDice()
    {
        if (initFlg) return;

        selectStone = scriptManager.CreateScript(new ScriptData(
            new ScriptParts[] { new ScriptParts((int)ScriptManager.ScriptType.SelectStoneBoard, "--min 1 --max 3") },
            ScriptManager.ActionType.Entry));

        scriptManager.SetRunScript(selectStone);

        initFlg = true;
    }

    void TurnStart()
    {
        if (mainStep != MainStep.TurnStart) return;

        mainStep = MainStep.P_UseItem;
    }

    void P_UseItem()
    {
        if (mainStep != MainStep.P_UseItem) return;


        mainStep = MainStep.P_UseItem;
    }

    void TurnEnd()
    {
        if (mainStep != MainStep.TurnEnd) return;

        mainStep = MainStep.TurnStart;

        nowPlayerCount++;
        nowPlayerCount %= players.Count;
    }

    IEnumerator GetBookData(int _bookId,
        System.Action<BookAPIBase.GetBookDatasResponse> _success,
        System.Action _failed = null)
    {
        if (_success == null) yield break;

        var res = BookAPIBase.ins.GetBookData(_bookId);

        yield return res;

        if (res.Current.statusCode != 200)
        {
            if (_failed != null) _failed();
            yield break;
        }
        _success(res.Current);

    }

    IEnumerator GetCardsFromBookData(int _bookId,
        System.Action<CardAPIBase.GetCardsFromBookResponse> _success,
        System.Action _failed = null)
    {
        if (_success == null) yield break;

        var res = CardAPIBase.ins.GetCardsFromBook(_bookId);
        yield return res;


        if (res.Current.statusCode != 200)
        {
            if (_failed != null) _failed();
            yield break;
        }

        _success(res.Current);

    }

    void CreatePlayer()
    {
        var playerCom = CreatePlayerComponent();

        playerCom.SetLocalPlayerController();
        StartCoroutine(GetBookData(manager.useBookNo, (res)=>{}));
        StartCoroutine(GetCardsFromBookData(manager.useBookNo, (res) =>
        {
            var cards = new List<CardData>();

            foreach (var card in res.data)
            {
                cards.Add(CardData.CreateCardDataFromDTO(card));
            }

            playerCom.Init(cards.ToArray(), true);
        }));

        cameraObject.transform.SetParent(playerCom.transform);
    }

    void CreateCPUPlayer(Manager.MemberType _type)
    {
        if (_type != Manager.MemberType.CPU) return;

        var playerCom = CreatePlayerComponent();

        playerCom.SetCPUController();

        CreateOtherPlayerBase(playerCom);
    }

    void CreateNetworkPlayer(Manager.MemberType _type)
    {
        if (_type != Manager.MemberType.NetWorkPlayer) return;

        var playerCom = CreatePlayerComponent();

        playerCom.SetNetController();

        CreateOtherPlayerBase(playerCom);
    }

    void CreateOtherPlayerBase(Player _player)
    {
        //StartCoroutine(GetBookData(manager.useBookNo, (res)=>{}));
        StartCoroutine(GetCardsFromBookData(1, (res) =>
        {
            var cards = new List<CardData>();

            foreach (var card in res.data)
            {
                cards.Add(CardData.CreateCardDataFromDTO(card));
            }

            _player.Init(cards.ToArray());
        }));
    }

    Player CreatePlayerComponent()
    {
        var player = Instantiate(playerPrefab.gameObject);
        var playerCom = player.GetComponent<Player>();
        playerCom.SetGameManager(this);

        players.Add(playerCom);
        return playerCom;
    }

}

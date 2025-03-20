using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum MainStep
    {
        StartTurn,
        UseItem,
        PutStone,
        PlayMagic,
        SetTrap,
        EndTurn
    }

    public enum PlayMagicStep
    {
        StartStep,
        SelectCard,
        OpenCard,
        EndStep
    }

    //Initialize//

    [SerializeField]
    int RANDOM_COUNT = 100;

    [SerializeField]
    GameObject initRandomPutStone = null;

    //Player ŠÖŒW//
    [SerializeField,ReadOnly]
    Text vMessage = null, lMessage = null;

    [SerializeField, ReadOnly]
    List<Player> players = new List<Player>();

    [SerializeField]
    Player playerPrefab = null;

    [SerializeField]
    Camera cameraObject = null;

    [SerializeField, ReadOnly]
    int nowPlayerCount = 0;

    [SerializeField, ReadOnly]
    int otherPlayerCount = 0;

    //ScriptŠÖŒW//

    [SerializeField, ReadOnly]
    ScriptManager scriptManager = new ScriptManager();

    [SerializeField, ReadOnly]
    List<CardScript> stack = new List<CardScript>();

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectItem = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectStone = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectMagic = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectSetTrap = null;

    [SerializeField]
    StoneBoardManager stoneBoard = null;

    public StoneBoardManager stoneBoardObj { get { return stoneBoard; } }


    [SerializeField, ReadOnly]
    Manager manager = Manager.ins;

    bool initFlg { get; set; } = false;

    MainStep mainStep = MainStep.StartTurn;
    PlayMagicStep playMagicStep = PlayMagicStep.EndStep;

    public void SelectStonePos(int _x,int _y)
    {
        scriptManager.SelectTargetPos(_x, _y, this);
    }

    public void SetTextObject(Text _vText,Text _lText)
    {
        vMessage = _vText;
        lMessage = _lText;
    }

    public void SetMessate(string _message)
    {
        if (vMessage == null) return;
        if (lMessage == null) return;
        vMessage.text = _message;
        lMessage.text = _message;
    }

    public Player GetPlayer(int _num)
    {
        if (_num < 0) return null;
        if (players.Count >= _num) return null;
        return players[_num];
    }

    public Player GetNowPlayer()
    {
        return players[nowPlayerCount];
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

        TurnEnd();
        PlayMagic();
        PutStone();
        UseItem();
        TurnStart();

    }

    void StartDice()
    {
        if (initFlg) return;

        selectStone = scriptManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectStoneBoard, "--min 1 --max 3 --is-put"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveStone, "--put"),},
            ScriptManager.ActionType.Entry));

        selectMagic = scriptManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--min 1 --max 3 --is-put"),
               new ScriptParts((int)ScriptManager.ScriptType.Stack, "--put"),},
            ScriptManager.ActionType.Entry));

        InitRandomPutStone();

        initFlg = true;
    }

    void TurnStart()
    {
        if (mainStep != MainStep.StartTurn) return;

        mainStep = MainStep.UseItem;
    }

    void UseItem()
    {
        if (mainStep != MainStep.UseItem) return;


        mainStep = MainStep.PutStone;
    }

    void PutStone()
    {
        if (mainStep != MainStep.PutStone) return;

        scriptManager.SetRunScript(selectStone);

        mainStep = MainStep.PlayMagic;
    }

    void PlayMagic()
    {
        if (mainStep != MainStep.PlayMagic) return;

        StartStep();


        mainStep = MainStep.EndTurn;
    }

    void TurnEnd()
    {
        if (mainStep != MainStep.EndTurn) return;

        mainStep = MainStep.StartTurn;

        nowPlayerCount++;
        nowPlayerCount %= players.Count;
    }

    void StartStep()
    {
        if (playMagicStep != PlayMagicStep.StartStep) return;

        mainStep = MainStep.PlayMagic;

        scriptManager.SetRunScript(selectMagic);

        otherPlayerCount++;

        if (otherPlayerCount <= players.Count) return;

        otherPlayerCount = 0;
    }

    void InitRandomPutStone()
    {
        if (initRandomPutStone == null) return;
        if (manager.randomPutStone <= 0) return;

        int fieldSize = (stoneBoard.VERTICAL_SIZE -1) * (stoneBoard.HOLYZONTAL_SIZE - 1);
        Vector2Int[] positions = new Vector2Int[fieldSize];
        int[] numList = new int[fieldSize];

        int tmpLoopCount = 0;

        for (tmpLoopCount = 0; tmpLoopCount < fieldSize; tmpLoopCount++)
        {
            positions[tmpLoopCount] = new Vector2Int(tmpLoopCount % (stoneBoard.HOLYZONTAL_SIZE - 1), tmpLoopCount / (stoneBoard.HOLYZONTAL_SIZE - 1));
            numList[tmpLoopCount] = tmpLoopCount;
        }

        int changeNum = 0;
        int baseNum = 0;

        for (tmpLoopCount = 0; tmpLoopCount < RANDOM_COUNT; tmpLoopCount++)
        {
            for(baseNum = 0; baseNum<numList.Length; baseNum++)
            {
                changeNum = Random.Range(0, fieldSize);

                if (baseNum == changeNum) continue;
                numList[baseNum] += numList[changeNum];
                numList[changeNum] = numList[baseNum] - numList[changeNum];
                numList[baseNum] = numList[baseNum] - numList[changeNum];
            }

        }
        Vector2Int pos = Vector2Int.zero;
        for (tmpLoopCount = 0; tmpLoopCount < manager.randomPutStone; tmpLoopCount++)
        {
            pos = positions[numList[tmpLoopCount]];
            stoneBoard.PutStone(pos.x, pos.y, initRandomPutStone);
        }

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

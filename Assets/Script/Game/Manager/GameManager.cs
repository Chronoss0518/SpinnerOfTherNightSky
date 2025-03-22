using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Initialize//
    [SerializeField]
    GameObject initRandomPutStone = null;

    //Player ŠÖŒW//
    [SerializeField,ReadOnly]
    Text vMessage = null, lMessage = null;

    [SerializeField, ReadOnly]
    List<Player> players = new List<Player>();

    public int PlayersCount { get { return players.Count; } }

    [SerializeField]
    Player playerPrefab = null;

    [SerializeField]
    Camera cameraObject = null;

    [SerializeField]
    public int nowPlayerCount { get; private set; } = 0;

    //ScriptŠÖŒW//

    [SerializeField, ReadOnly]
    ScriptManager scriptManager = new ScriptManager();

    [SerializeField, ReadOnly]
    TurnManager turnManager = new TurnManager();

    [SerializeField, ReadOnly]
    List<CardScript> stack = new List<CardScript>();

    public int StackCount { get { return stack.Count; } }

    [SerializeField]
    StoneBoardManager stoneBoard = null;

    public StoneBoardManager stoneBoardObj { get { return stoneBoard; } }


    [SerializeField, ReadOnly]
    Manager manager = Manager.ins;

    bool initFlg { get; set; } = false;

    public void SelectStonePos(int _x,int _y)
    {
        scriptManager.SelectTargetPos(_x, _y, this);
    }

    public void SelectCard(Player _player, CardScript _card,ScriptManager.ZoneType _type)
    {
        
    }

    public void StartSelectCard(ScriptManager.SelectCardAction _action)
    {
        for(int i = 0;i<players.Count;i++)
        {
            players[i].SelectTargetStart(_action, players[nowPlayerCount]);
        }
    }

    public void EndSelectCardTest()
    {
        for (int i = 0; i<players.Count; i++)
        {
            players[i].SelectTargetEnd();
        }
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

    public void AddNowPlayerCount()
    {
        nowPlayerCount++;
        nowPlayerCount %= players.Count;
    }

    public void SetNowPlayerCount(int _count)
    {
        if (_count <= 0) return;
        nowPlayerCount = _count;
        nowPlayerCount %= players.Count;
    }

    public ScriptManager.ScriptActionData CreateScript(ScriptData _data)
    {
        return scriptManager.CreateScript(_data);
    }

    public void RegistScript(ScriptManager.ScriptActionData _script)
    {
        scriptManager.SetRunScript(_script);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (stoneBoard != null) stoneBoard.Init();


        for (int i = 0; i < Manager.MAX_GMAE_PLAYER && i < manager.memberFlgs.Length; i++)
        {
            var memberFlgs = manager.memberFlgs[i];
            CreateLocalPlayer(memberFlgs);
            CreateCPUPlayer(memberFlgs);
            CreateNetworkPlayer(memberFlgs);
        }

    }

    // Update is called once per frame
    void Update()
    {
        StartDice();

        if (!initFlg) return;

        if (!scriptManager.isRunScript)
        {
            turnManager.Update();
            return;
        }

        var controller = players[nowPlayerCount].GetComponent<ControllerBase>();

        scriptManager.RunScript(controller, this);
    }


    void StartDice()
    {
        if (initFlg) return;

        turnManager.Init(this);

        stoneBoard.PutRandomStone(manager.randomPutStone, initRandomPutStone);

        initFlg = true;
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

    void CreateLocalPlayer(Manager.MemberType _type)
    {
        if (_type != Manager.MemberType.LocalPlayer) return;

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

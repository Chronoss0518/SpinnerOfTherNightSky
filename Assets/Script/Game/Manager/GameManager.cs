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

    public int playersCount { get { return players.Count; } }

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

    bool runStackFlg = false;

    public int stackCount { get { return stack.Count; } }

    [SerializeField]
    StoneBoardManager stoneBoard = null;

    public StoneBoardManager stoneBoardObj { get { return stoneBoard; } }


    [SerializeField, ReadOnly]
    Manager manager = Manager.ins;

    bool initFlg { get; set; } = false;

    [SerializeField]
    GameObject cardPrefabBase = null;
    
    public GameObject cardPrefab { get { return cardPrefabBase; } }
    

    public void AddStackCard(CardScript _card)
    {
        stack.Add(_card);
    }

    public void SelectStonePos(int _x,int _y)
    {
        scriptManager.SelectTargetPos(_x, _y, this);
    }

    public void SelectCard(CardScript _card)
    {
        scriptManager.SelectCard(_card, this);
    }

    public bool SelectItemZonePos(int _pos)
    {
        return scriptManager.SelectTargetItemZonePos(_pos, this);
    }

    public void StartSelectCard(ScriptManager.SelectCardArgument _action)
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

    public void SelectItemZone(int _num)
    {
        scriptManager.SelectTargetItemZonePos(_num, this);
    }

    public void StartSelectItemZone(ScriptManager.SelectItemZoneArgument _action)
    {
        for (int i = 0; i<players.Count; i++)
        {
            if (_action.selectTarget == 1 && i != nowPlayerCount) continue;
            if (_action.selectTarget == 2 && i == nowPlayerCount) continue;

            players[i].SelectTargetItemZoneStart();
        }
    }

    public void EndSelectItemZone(ScriptManager.SelectItemZoneArgument _action)
    {
        for (int i = 0; i<players.Count; i++)
        {
            players[i].SelectTargetItemZoneEnd();
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

    public int GetPlayerNum(Player _player)
    {
        if (_player == null) return -1;
        for(int i = 0;i<players.Count;i++)
        {
            if (!players[i].gameObject.Equals(_player.gameObject)) continue;
            return i;
        }
        return -1;
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

    public ScriptManager.ScriptArgumentData CreateScript(ScriptData _data)
    {
        return scriptManager.CreateScript(_data);
    }

    public void RegistScript(ScriptManager.ScriptArgumentData _script)
    {
        scriptManager.SetRunScript(_script);
    }

    public void RunStackScriptStart()
    {
        if (stack.Count <= 0) return;
        runStackFlg = true;
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

    bool IsInitializPlayers()
    {
        foreach (var player in players)
        {
            if (player.initFlg) continue;
            return false;
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {

        StartDice();

        if (!initFlg) return;

        MainUpdate();

        StackUpdate();

        var controller = players[nowPlayerCount].GetComponent<ControllerBase>();

        scriptManager.RunScript(controller, this);

    }

    void MainUpdate()
    {
        if (scriptManager.isRunScript) return;
        if (runStackFlg) return;

        turnManager.Update();
    }

    void StackUpdate()
    {
        if (scriptManager.isRunScript) return;
        if (!runStackFlg) return;

        var stackScript = stack[stack.Count - 1];

        stack.RemoveAt(stack.Count - 1);

        scriptManager.CreateScript(stackScript.script[0], true);

        if (stack.Count > 0) return;
        runStackFlg = false;
    }

    void StartDice()
    {
        if (initFlg) return;

        if (!IsInitializPlayers()) return;

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

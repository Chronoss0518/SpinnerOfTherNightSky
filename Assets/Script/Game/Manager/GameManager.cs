using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using ChUnity.Input;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public enum PlayerPosition
    {
        Front,
        Right,
        Back,
        Left,
    }

    [SerializeField, ReadOnly]
    PointerManager pointerManager = PointerManager.instance;

    //Initialize//
    [SerializeField]
    GameObject initRandomPutStone = null;

    //Player �֌W//
    [SerializeField, ReadOnly]
    Text vMessage = null, lMessage = null;

    [SerializeField, ReadOnly]
    List<Player> players = new List<Player>();

    public int playersCount { get { return players.Count; } }

    [SerializeField]
    Player playerPrefab = null;

    [SerializeField, ReadOnly]
    Player localPlayer = null;

    [SerializeField]
    Camera cameraObject = null;

    public int nowPlayerNo { get; private set; } = 0;

    public int useScriptPlayerNo { get; private set; } = 0;

    [SerializeField, ReadOnly]
    int viewUseScriptPlayerNo = 0;

    //Script�֌W//

    [SerializeField, ReadOnly]
    ScriptManager scriptManager = new ScriptManager();

    public bool isRunScript { get { return scriptManager.isRunScript; } }

    [SerializeField, ReadOnly]
    TurnManager turnManager = new TurnManager();

    public TurnManager.MainStep turnStep { get { return turnManager.nowMainStep; } }

    [SerializeField, ReadOnly]
    PanelPosBase.PanelPosManager panelPosManager = new PanelPosBase.PanelPosManager();

    [SerializeField,ReadOnly]
    StackManager stackManager = new StackManager();

    public int stackCount { get { return stackManager.stackCount; } }

    [SerializeField]
    StoneBoardManager stoneBoard = null;

    public StoneBoardManager stoneBoardObj { get { return stoneBoard; } }


    [SerializeField, ReadOnly]
    FindStarFromMagicManager findStarFromMagicManager = FindStarFromMagicManager.ins;


    [SerializeField, ReadOnly]
    Manager manager = Manager.ins;

    bool initFlg { get; set; } = false;

    [SerializeField]
    GameObject cardPrefabBase = null;

    public GameObject cardPrefab { get { return cardPrefabBase; } }

    [SerializeField]
    StarPosSheet starPosSheetPrefab = null;

    public void AddStackCard(StackManager.StackObject _card)
    {
        stackManager.AddStackCard(_card);
    }

    public void SelectStonePos(int _x,int _y)
    {
        scriptManager.SelectTargetPos(_x, _y, this);
    }

    public void SelectCard(CardScript _card)
    {
        scriptManager.SelectCard(_card, this);
    }

    public bool SelectItemZonePos(ItemZoneObject _pos)
    {
        return scriptManager.SelectTargetItemZonePos(_pos, this);
    }

    public void StartSelectCard(ScriptManager.SelectCardArgument _action)
    {
        for(int i = 0;i<players.Count;i++)
        {
            players[i].SelectTargetStart(_action, players[nowPlayerNo]);
        }
    }

    public void EndSelectCardTest()
    {
        for (int i = 0; i<players.Count; i++)
        {
            players[i].SelectTargetEnd();
        }
    }

    public void SelectItemZone(ItemZoneObject _pos)
    {
        scriptManager.SelectTargetItemZonePos(_pos, this);
    }

    public void StartSelectItemZone(ScriptManager.SelectItemZoneArgument _action)
    {
        for (int i = 0; i<players.Count; i++)
        {
            if (_action.selectTarget == 1 && i != nowPlayerNo) continue;
            if (_action.selectTarget == 2 && i == nowPlayerNo) continue;

            players[i].SelectTargetItemZoneStart();
        }
    }

    public void EndSelectItemZone()
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

    public Player GetNowPlayer()
    {
        return players[nowPlayerNo];
    }

    public void AddNowPlayerNo()
    {
        nowPlayerNo++;
        nowPlayerNo %= players.Count;
    }

    public void SetUseScriptPlayerNo(int _count = -1)
    {
        if (_count <= 0)
            _count = nowPlayerNo;

        useScriptPlayerNo = _count;
        useScriptPlayerNo %= players.Count;
    }

    public ScriptManager.ScriptArgumentData CreateScript(ScriptData _data,bool _registFlg = false, int _useScriptPlayerNo = -1)
    {
        if (_registFlg)
            SetUseScriptPlayerNo(_useScriptPlayerNo);
        return scriptManager.CreateScript(_data, _registFlg);
    }


    public ScriptManager.ScriptArgumentData CreateScript(ScriptManager.ScriptArgument[] _args, bool _registFlg = false, int _useScriptPlayerNo = -1)
    {
        if (_registFlg)
            SetUseScriptPlayerNo(_useScriptPlayerNo);
        return scriptManager.CreateScript(_args, _registFlg);
    }

    public void RegistScript(ScriptManager.ScriptArgumentData _script,int _useScriptPlayerNo = -1)
    {
        SetUseScriptPlayerNo(_useScriptPlayerNo);
        scriptManager.SetRunScript(_script);
    }

    public void RunStackScriptStart()
    {
        stackManager.RunStart();
    }

    public StarPosSheet CreateStarPanelSheet(MagicCardScript _script)
    {
        if (_script == null) return null;
        if (starPosSheetPrefab == null) return null;

        var obj = Instantiate(starPosSheetPrefab.gameObject);

        var script = obj.GetComponent<StarPosSheet>();
        panelPosManager.CreatePanel(script);

        script.SetStoneBoard(stoneBoard);
        script.SetUseCamera(cameraObject);

        foreach(var pos in _script.starPosList)
        {
            script.SetStarPosFlg(pos.x, pos.y, true);
        }

        obj.transform.SetParent(localPlayer.transform);
        obj.transform.localRotation = Quaternion.identity;

        return script;
    }

    public void OpenCardDescription(CardScript _script)
    {
        if (_script == null) return;
        if (_script.baseData == null) return;

        Debug.Log("Open Card Description");
    }

    // Start is called before the first frame update
    void Start()
    {
        stackManager.Init(this);
        findStarFromMagicManager.Init(this);
        InitStoneBoadAndStarPos();

        for (int i = 0; i < Manager.MAX_GMAE_PLAYER && i < manager.memberFlgs.Length; i++)
        {
            var memberFlgs = manager.memberFlgs[i];
            CreateLocalPlayer(memberFlgs,i);
            CreateCPUPlayer(memberFlgs, i);
            CreateNetworkPlayer(memberFlgs, i);
        }
        turnManager.Init(this);

    }

    void InitStoneBoadAndStarPos()
    {
        if (stoneBoard == null) return;
        panelPosManager.CreatePanel(stoneBoard);
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

        pointerManager.Update();

        StartDice();

        if (!initFlg) return;

        MainUpdate();

        stackManager.Update();


        viewUseScriptPlayerNo = useScriptPlayerNo;

        var controller = players[useScriptPlayerNo].GetComponent<ControllerBase>();

        scriptManager.RunScript(controller, this);

        stackManager.RunEnd();
    }


    void MainUpdate()
    {
        if (scriptManager.isRunScript) return;
        if (stackManager.runStackFlg) return;

        turnManager.Update();
    }

    void StartDice()
    {
        if (initFlg) return;

        if (!IsInitializPlayers()) return;

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

    void CreateLocalPlayer(Manager.MemberType _type, int _no)
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
                var cardData = CardData.CreateCardDataFromDTO(card);
                cards.Add(cardData);
                findStarFromMagicManager.AddMagicCard(cardData);
            }

            playerCom.Init(cards.ToArray(), true);
            localPlayer = playerCom;
        }));

        cameraObject.gameObject.SetActive(false);
    }

    void CreateCPUPlayer(Manager.MemberType _type,int _no)
    {
        if (_type != Manager.MemberType.CPU) return;

        var playerCom = CreatePlayerComponent();

        playerCom.SetCPUController();

        CreateOtherPlayerBase(playerCom, _no);
    }

    void CreateNetworkPlayer(Manager.MemberType _type, int _no)
    {
        if (_type != Manager.MemberType.NetWorkPlayer) return;

        var playerCom = CreatePlayerComponent();

        playerCom.SetNetController();

        CreateOtherPlayerBase(playerCom, _no);
    }

    void CreateOtherPlayerBase(Player _player, int _no)
    {
        //StartCoroutine(GetBookData(manager.useBookNo, (res)=>{}));
        StartCoroutine(GetCardsFromBookData(1, (res) =>
        {
            var cards = new List<CardData>();

            foreach (var card in res.data)
            {
                var cardData = CardData.CreateCardDataFromDTO(card);
                cards.Add(cardData);
                findStarFromMagicManager.AddMagicCard(cardData);
            }

            _player.Init(cards.ToArray());
        }));
    }

    Player CreatePlayerComponent()
    {
        var player = Instantiate(playerPrefab.gameObject);
        var playerCom = player.GetComponent<Player>();
        playerCom.SetGameManager(this);

        playerCom.SetPlayerNo(players.Count);
        playerCom.SetPlayerPosition((PlayerPosition)players.Count);
        players.Add(playerCom);
        return playerCom;
    }

}

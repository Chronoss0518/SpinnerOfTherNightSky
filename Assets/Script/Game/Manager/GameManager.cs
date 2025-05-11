using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using ChUnity.Input;


public class GameManager : MonoBehaviour
{
    [SerializeField,ReadOnly]
    PointerManager pointerManager = PointerManager.instance;

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

    public int nowPlayerNo { get; private set; } = 0;
    
    public int useScriptPlayerNo { get; private set; } = 0;

    //ScriptŠÖŒW//

    [SerializeField, ReadOnly]
    ScriptManager scriptManager = new ScriptManager();

    [SerializeField, ReadOnly]
    TurnManager turnManager = new TurnManager();

    [SerializeField, ReadOnly]
    PanelPosBase.PanelPosManager panelPosManager = new PanelPosBase.PanelPosManager();

    public class StackObject
    {
        public StackObject(Player _player, CardScript _card)
        {
            player = _player;
            card = _card;
        }

        public Player player = null;
        public CardScript card = null;
    }


    [SerializeField, ReadOnly]
    List<StackObject> stack = new List<StackObject>();

    [SerializeField, ReadOnly]
    StackObject playCardScript = null;


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

    [SerializeField]
    StarPosSheet starPosSheetPrefab = null;

    public void AddStackCard(StackObject _card)
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

    public void SetUseScriptPlayerNo(int _count)
    {
        if (_count <= 0)
            _count = nowPlayerNo;

        useScriptPlayerNo = _count;
        useScriptPlayerNo %= players.Count;
    }

    public ScriptManager.ScriptArgumentData CreateScript(ScriptData _data)
    {
        return scriptManager.CreateScript(_data);
    }

    public void RegistScript(ScriptManager.ScriptArgumentData _script,int _useScriptPlayerNo = -1)
    {
        SetUseScriptPlayerNo(_useScriptPlayerNo);
        scriptManager.SetRunScript(_script);
    }

    public void RunStackScriptStart()
    {
        if (stack.Count <= 0) return;
        runStackFlg = true;
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
        InitStoneBoadAndStarPos();

        for (int i = 0; i < Manager.MAX_GMAE_PLAYER && i < manager.memberFlgs.Length; i++)
        {
            var memberFlgs = manager.memberFlgs[i];
            CreateLocalPlayer(memberFlgs);
            CreateCPUPlayer(memberFlgs);
            CreateNetworkPlayer(memberFlgs);
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

        StartDice();

        if (!initFlg) return;

        MainUpdate();

        StackUpdate();

        var controller = players[useScriptPlayerNo].GetComponent<ControllerBase>();

        scriptManager.RunScript(controller, this);
        
        StackCardScriptEnd();
    }

    private void FixedUpdate()
    {
        pointerManager.Update();
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

        playCardScript = stack[stack.Count - 1];

        stack.RemoveAt(stack.Count - 1);

        scriptManager.CreateScript(playCardScript.card.script[0], true);

        useScriptPlayerNo = playCardScript.player.playerNo;

        if (stack.Count > 0) return;
        runStackFlg = false;
    }

    void StartDice()
    {
        if (initFlg) return;

        if (!IsInitializPlayers()) return;


        stoneBoard.PutRandomStone(manager.randomPutStone, initRandomPutStone);

        for(int i = 0;i<players.Count;i++)
        {
            players[i].transform.localRotation = Quaternion.Euler(0.0f, 90 * i, 0.0f);
        }

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

            playerCom.Init(cards.ToArray(),playersCount - 1, true);
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

            _player.Init(cards.ToArray(), playersCount - 1);
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

    void StackCardScriptEnd()
    {
        if (scriptManager.isRunScript) return;
        if (playCardScript == null) return;

        MagicActionEnd();
        ItemActionEnd();

        playCardScript = null;
    }

    void MagicActionEnd()
    {
        if (playCardScript.card.type != CardData.CardType.Magic) return;

        var magic = playCardScript.card.GetComponent<MagicCardScript>();

        bool removeStoneFailedFlg = magic.removeStoneFailedFlg;

        var cardData = playCardScript.card.baseData;
        var zone = playCardScript.card.zone;

        zone.RemoveCard(cardData);

        if(!removeStoneFailedFlg)
            playCardScript.player.magicZone.PutCard(cardData);
        else
            cardData.havePlayer.trashZone.PutCard(cardData);
    }

    void ItemActionEnd()
    {
        if (playCardScript.card.type != CardData.CardType.Item) return;

        var cardData = playCardScript.card.baseData;
        var zone = playCardScript.card.zone;

        var player = cardData.havePlayer;

        zone.RemoveCard(cardData);

        player.trashZone.PutCard(cardData);
    }
}

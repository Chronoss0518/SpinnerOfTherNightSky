using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

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

    [SerializeField, ReadOnly]
    List<Player> players = new List<Player>();

    [SerializeField, ReadOnly]
    int nowPlayerCount = 0;

    [SerializeField]
    BoardStoneManager stoneBoard = null;

    [SerializeField]
    Player playerPrefab = null;

    [SerializeField, ReadOnly]
    List<CardScript> stack = new List<CardScript>();

    public BoardStoneManager stoneBoardObj { get { return stoneBoard; } }

    [SerializeField, ReadOnly]
    Manager manager = Manager.ins;

    bool initFlg { get; set; } = false;

    MainStep mainStep = MainStep.TurnStart;
    PlayMagicStep playMagicStep = PlayMagicStep.StartStep;

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

        for (int i = 1; i < Manager.MAX_GMAE_PLAYER; i++)
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

        TurnStart();

        P_UseItem();


        TurnEnd();
    }


    void StartDice()
    {
        if (initFlg) return;
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
        var player = Instantiate(playerPrefab.gameObject);
        var playerCom = player.GetComponent<Player>();

        players.Add(playerCom);

        playerCom.SetPlayerController();
        //StartCoroutine(GetBookData(manager.useBookNo, (res)=>{}));
        StartCoroutine(GetCardsFromBookData(manager.useBookNo, (res) =>
        {
            var cards = new List<CardData>();

            foreach (var card in res.data)
            {
                cards.Add(CardData.CreateCardDataFromDTO(card));
            }

            playerCom.Init(cards.ToArray(), true);
        }));
    }

    void CreateCPUPlayer(Manager.MemberType _type)
    {
        if (_type != Manager.MemberType.CPU) return;
        var player = Instantiate(playerPrefab.gameObject);
        var playerCom = player.GetComponent<Player>();

        playerCom.SetCPUController();
        players.Add(playerCom);

        CreateOtherPlayerBase(playerCom);
    }

    void CreateNetworkPlayer(Manager.MemberType _type)
    {
        if (_type != Manager.MemberType.NetWorkPlayer) return;

        var player = Instantiate(playerPrefab.gameObject);
        var playerCom = player.GetComponent<Player>();

        playerCom.SetNetController();
        players.Add(playerCom);

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

}

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

    [SerializeField,ReadOnly]
    List<Player> players = new List<Player>();

    [SerializeField, ReadOnly]
    int nowPlayerCount = 0;

    [SerializeField]
    BoardStoneManager stoneBoard = null;

    [SerializeField]
    Player playerPrefab = null;

    [SerializeField, ReadOnly]
    List<CardScript>stack = new List<CardScript>();

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
        for(int i = 0;i < (int)(manager.memberNum); i++)
        {
            var player = Instantiate(playerPrefab.gameObject);
            var playerCom = player.GetComponent<Player>();

            InitPlayerScript(playerCom,i);

            players.Add(playerCom);
        }

        StartCoroutine(WebRequest());
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

    void InitPlayerScript(Player _player,int _no)
    {
        _player.SetGameManager(this);
        
        if (_no <= 0)
        {
            _player.SetPlayerController();

        } else {

            if (manager.cpuFlgs[_no - 1])
                _player.SetCPUController();
            else
                _player.SetNetController();

        }
    }

    IEnumerator WebRequest()
    {
        for(int i = 0;i<players.Count;i++)
        {
            int bookId = manager.useBookNo;
            
            if (i > 0)
                bookId = 1;

            //yield return GetBookData(manager.useBookNo, (_res) => { });

            yield return GetCardsFromBookData(manager.useBookNo, (_res) =>
            {
                var res = _res;
                var resultList = new List<CardData>();

                for (int i = 0; i < res.data.Length; i++)
                {
                    resultList.Add(CardData.CreateCardDataFromDTO(res.data[i]));
                    Debug.Log("SetManager Init Book Pos:[" + res.data[i].init_book_pos.ToString() + "]");
                }

                players[i].Init(resultList.ToArray());
            },
            () =>
            {
                Debug.Log("Is Failed");
            });


        }

    }

    IEnumerator GetBookData(int _bookId,
        System.Action<BookAPIBase.GetBookDatasResponse> _success,
        System.Action _failed = null)
    {
        if (_success == null) yield break;
        
        yield return BookAPIBase.ins.GetBookData(_bookId, (_res) =>
        {
            if (_res.statusCode != 200)
            {
                if (_failed != null) _failed();
                return;
            }

            _success(_res);
        });


    }

    IEnumerator GetCardsFromBookData(int _bookId,
        System.Action<CardAPIBase.GetCardsFromBookResponse> _success,
        System.Action _failed = null)
    {
        if (_success == null) yield break;

        yield return CardAPIBase.ins.GetCardsFromBook(_bookId, (_res) =>
        {
            if (_res.statusCode != 200)
            {
                if (_failed != null) _failed();
                return;
            }

            _success(_res);
        });



    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    List<Player> players = new List<Player>();

    [SerializeField]
    BoardStoneManager stoneBoard;

    [SerializeField]
    Player playerPrefab = null;

    public BoardStoneManager stoneBoardObj { get { return stoneBoard; } }


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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

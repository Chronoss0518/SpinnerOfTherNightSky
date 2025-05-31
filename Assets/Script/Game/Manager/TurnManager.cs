using UnityEngine;
using Unity.Collections;

[System.Serializable]
public class TurnManager
{
    abstract public class TurnClass
    {
        public TurnClass(TurnManager _manager){ manager = _manager; }

        virtual public void Init() {}

        abstract public void Update();

        abstract public void Next();

        public void SetMainStep(MainStep _turn)
        {
            manager.mainStep = _turn;
        }

        protected void ChangeTurn()
        {
            manager.changeTurn = true;
        }

        protected GameManager gameManager { get { return manager.gameManager; } }

        protected MainStep mainStep { get { return manager.mainStep; } }

        private TurnManager manager = null;
    }

    public enum MainStep
    {
        StartTurn,
        UseItem,
        PutStone,
        PlayMagic,
        SetTrap,
        EndTurn
    }

    GameManager gameManager = null;

    [SerializeField,ReadOnly]
    private bool changeTurn = false;

    TurnClass[] turnClass = null;

    TurnClass runTurn = null;

    [SerializeField,ReadOnly]
    private MainStep mainStep = MainStep.StartTurn;

    public MainStep nowMainStep { get { return mainStep; } }

    public void Init(GameManager _gm)
    {
        gameManager = _gm;
        mainStep = MainStep.StartTurn;

        turnClass =  new TurnClass[] { 
            new StartTurn(this),
            new UseItem(this),
            new PutStone(this),
            new PlayMagic(this),
            new SetTrap(this),
            new EndTurn(this)
        };

        runTurn = turnClass[(int)mainStep];
    }

    public void Update()
    {
        if(changeTurn)
        {
            runTurn.Next();
            runTurn = null;
            runTurn = turnClass[(int)mainStep];
            runTurn.Init();
            changeTurn = false;
            return;
        }

        runTurn.Update();
    }


}

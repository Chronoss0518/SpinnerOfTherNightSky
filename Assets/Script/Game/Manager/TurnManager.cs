using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;



[System.Serializable]
public class TurnManager
{
    abstract public class TurnClass
    {

        public TurnClass(TurnManager _manager)
        {
            manager = _manager;
        }
        virtual public void Init() { }

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
        PlayMagicInit,
        PlayMagic,
        SetTrap,
        EndTurn
    }

    public enum PlayMagicStep
    {
        StartStep,
        SelectCard,
        UseCard,
        EndStep
    }

    GameManager gameManager = null;

    private bool changeTurn = false;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectItem = null;

    [SerializeField,ReadOnly]
    ScriptManager.ScriptActionData selectTrap = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectStone = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectMagic = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectCard = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectSetItem = null;

    public MainStep mainStep { get; private set; } = MainStep.StartTurn;
    public PlayMagicStep playMagicStep { get; private set; } = PlayMagicStep.EndStep;

    [SerializeField, ReadOnly]
    int tmpNowPlayerCount = 0;

    [SerializeField, ReadOnly]
    int playCardUserCount = 0;

    [SerializeField, ReadOnly]
    int otherPlayerCount = 0;

    [SerializeField, ReadOnly]
    int testPlayMagicCount = 0;

    [SerializeField, ReadOnly]
    int testPlayCardCount = 0;

    public void Init(GameManager _gm)
    {
        gameManager = _gm;
        mainStep = MainStep.StartTurn;
        playMagicStep = PlayMagicStep.EndStep;

        selectItem = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--min 0 --max 1 --player-type 0 --zone-type-book --card-type 2 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveCard, "--open-item-zone"),
                new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),},
            ScriptManager.ActionType.Entry));

        selectTrap = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--min 0 --max 1 --player-type 0 --zone-type-item --card-type 4 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveCard, "--open-item-zone"),
                new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),},
            ScriptManager.ActionType.Entry));

        selectStone = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectStoneBoard, "--min 1 --max 3 --is-put"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveStone, "--put"),},
            ScriptManager.ActionType.Entry));

        selectMagic = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--player-type 1 --min 0 --max 1 --zone-type-book --card-type 1 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),},
            ScriptManager.ActionType.Entry));

        selectCard = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--player-type 1 --min 0 --max 1 --zone-type-book --zone-type-item --card-type 5 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveCard, "--open-item-zone"),
                new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),},
            ScriptManager.ActionType.Entry));

        selectSetItem = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--player-type 1 --min 0 --max 1 --zone-type-book --card-type 6 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),},
            ScriptManager.ActionType.Entry));
    }

    public void Update()
    {
        TurnEnd();
        SetTrap();
        PlayMagic();
        PlayMagicInit();
        PutStone();
        UseItem();
        TurnStart();
    }

    void TurnStart()
    {
        if (mainStep != MainStep.StartTurn) return;

        mainStep = MainStep.UseItem;
    }

    void UseItem()
    {
        if (mainStep != MainStep.UseItem) return;

        gameManager.RegistScript(selectItem);

        mainStep = MainStep.PutStone;
    }

    void PutStone()
    {
        if (mainStep != MainStep.PutStone) return;

        gameManager.RegistScript(selectStone);

        mainStep = MainStep.PlayMagicInit;
    }

    void PlayMagicInit()
    {
        if (mainStep != MainStep.PlayMagicInit) return;

        tmpNowPlayerCount = gameManager.nowPlayerCount;
        playMagicStep = PlayMagicStep.StartStep;
        mainStep = MainStep.PlayMagic;
    }

    void PlayMagic()
    {
        if (mainStep != MainStep.PlayMagic) return;

        EndStep();
        UseCard();
        SelectCard();
        StartStep();

    }

    void SetTrap()
    {
        if (mainStep != MainStep.SetTrap) return;

        gameManager.RegistScript(selectSetItem);


        mainStep = MainStep.EndTurn;
    }

    void TurnEnd()
    {
        if (mainStep != MainStep.EndTurn) return;

        mainStep = MainStep.StartTurn;
        gameManager.AddNowPlayerCount();
    }

    void StartStep()
    {
        if (playMagicStep != PlayMagicStep.StartStep) return;

        playMagicStep = PlayMagicStep.SelectCard;
    }

    void SelectCard()
    {
        if (playMagicStep != PlayMagicStep.SelectCard) return;

        playMagicStep = PlayMagicStep.UseCard;
        gameManager.RegistScript(selectMagic);

    }

    void UseCard()
    {
        if (playMagicStep != PlayMagicStep.UseCard) return;

        playMagicStep = PlayMagicStep.EndStep;

        if (gameManager.StackCount <= 0) return;

        playCardUserCount = tmpNowPlayerCount + otherPlayerCount;
        playCardUserCount %= gameManager.PlayersCount;

        playMagicStep = PlayMagicStep.StartStep;
        gameManager.RegistScript(selectCard);

        otherPlayerCount++;
        gameManager.SetNowPlayerCount(playCardUserCount + otherPlayerCount);

    }

    void EndStep()
    {
        if (playMagicStep != PlayMagicStep.EndStep) return;

        testPlayCardCount++;
        if (testPlayCardCount < gameManager.PlayersCount) return;
        testPlayMagicCount++;
        testPlayCardCount = 0;

        gameManager.SetNowPlayerCount(tmpNowPlayerCount + testPlayMagicCount);

        if (testPlayMagicCount < gameManager.PlayersCount) return;

        testPlayMagicCount = 0;
        gameManager.SetNowPlayerCount(tmpNowPlayerCount);
        mainStep = MainStep.SetTrap;
    }
}

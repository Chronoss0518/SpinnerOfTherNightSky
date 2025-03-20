using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;



[System.Serializable]
public class TurnManager
{
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

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectItem = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectStone = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectMagic = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectCard = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectSetTrap = null;

    MainStep mainStep = MainStep.StartTurn;
    PlayMagicStep playMagicStep = PlayMagicStep.EndStep;

    bool useMagicFlg = false;

    [SerializeField, ReadOnly]
    int otherPlayerCount = 0;

    public void Init(GameManager _gm)
    {
        gameManager = _gm;
        mainStep = MainStep.StartTurn;
        playMagicStep = PlayMagicStep.EndStep;

        selectStone = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectStoneBoard, "--min 1 --max 3 --is-put"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveStone, "--put"),},
            ScriptManager.ActionType.Entry));

        selectMagic = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--min 1 --max 3 --is-put"),
               new ScriptParts((int)ScriptManager.ScriptType.Stack, "--put"),},
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



        

        mainStep = MainStep.EndTurn;
    }

    void TurnEnd()
    {
        if (mainStep != MainStep.EndTurn) return;

        mainStep = MainStep.StartTurn;
        gameManager.AddPlayerCount();
    }

    void StartStep()
    {
        if (playMagicStep != PlayMagicStep.StartStep) return;

        playMagicStep = PlayMagicStep.SelectCard;
    }

    void SelectCard()
    {
        if (playMagicStep != PlayMagicStep.StartStep) return;

        playMagicStep = PlayMagicStep.UseCard;
        gameManager.RegistScript(selectMagic);
    }

    void UseCard()
    {
        if (playMagicStep != PlayMagicStep.StartStep) return;

        playMagicStep = PlayMagicStep.UseCard;
        gameManager.RegistScript(selectMagic);
    }

    void EndStep()
    {
        if (playMagicStep != PlayMagicStep.EndStep) return;

        mainStep = MainStep.SetTrap;
    }


}

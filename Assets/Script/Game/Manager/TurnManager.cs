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
        PlayMagic,
        SetTrap,
        EndTurn
    }

    public enum PlayMagicStep
    {
        StartStep,
        SelectCard,
        OpenCard,
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
    ScriptManager.ScriptActionData selectSetTrap = null;

    MainStep mainStep = MainStep.StartTurn;
    PlayMagicStep playMagicStep = PlayMagicStep.EndStep;

    public void Init(GameManager _gm)
    {
        gameManager = _gm;

        selectStone = gameManager.scriptMgr.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectStoneBoard, "--min 1 --max 3 --is-put"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveStone, "--put"),},
            ScriptManager.ActionType.Entry));

        selectMagic = gameManager.scriptMgr.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--min 1 --max 3 --is-put"),
               new ScriptParts((int)ScriptManager.ScriptType.Stack, "--put"),},
            ScriptManager.ActionType.Entry));

    }


    public void Update()
    {
    }

    void UpdateMainTurn()
    {
        TurnEnd();
        PlayMagic();
        PutStone();
        UseItem();
        TurnStart();
    }

    void UpdateMagicStep()
    {
        TurnEnd();
        PlayMagic();
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

        gameManager.scriptMgr.SetRunScript(selectStone);

        mainStep = MainStep.PlayMagic;
    }

    void PlayMagic()
    {
        if (mainStep != MainStep.PlayMagic) return;

        StartStep();


        mainStep = MainStep.EndTurn;
    }

    void TurnEnd()
    {
        if (mainStep != MainStep.EndTurn) return;

        mainStep = MainStep.StartTurn;

        nowPlayerCount++;
        nowPlayerCount %= players.Count;
    }

    void StartStep()
    {
        if (playMagicStep != PlayMagicStep.StartStep) return;

        mainStep = MainStep.PlayMagic;

        gameManager.scriptMgr.SetRunScript(selectMagic);

        otherPlayerCount++;

        if (otherPlayerCount <= players.Count) return;

        otherPlayerCount = 0;
    }
}

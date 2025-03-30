using UnityEngine;
using Unity.Collections;

[System.Serializable]
public class UseItem : TurnManager.TurnClass
{
    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectItem = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptActionData selectTrap = null;

    int beforeStackCount = 0;

    int tmpNowPlayerCount = 0;

    int scriptUsePlayerCount = 0;

    int passPlayerCount = 0;

    public UseItem(TurnManager _manager) : base(_manager)
    {
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
    }

    override public void Init() 
    {
        beforeStackCount = 0;
        tmpNowPlayerCount = gameManager.nowPlayerCount;
        gameManager.RegistScript(selectItem);
    }

    public override void Update()
    {

        int tmpCount = gameManager.stackCount;

        if (beforeStackCount <= 0 && beforeStackCount >= tmpCount)
        {
            ChangeTurn();
            return;
        }

        if(gameManager.playersCount >= passPlayerCount)
        {
            ChangeTurn();
            return;
        }

        if(beforeStackCount < tmpCount)
        {
            scriptUsePlayerCount = gameManager.nowPlayerCount + passPlayerCount;
            passPlayerCount = 0;
            gameManager.SetNowPlayerCount(scriptUsePlayerCount + 1);
            gameManager.RegistScript(selectTrap);
            return;
        }

        passPlayerCount++;

        gameManager.SetNowPlayerCount(scriptUsePlayerCount + passPlayerCount);

        gameManager.RegistScript(selectItem);

    }

    public override void Next()
    {
        gameManager.SetNowPlayerCount(tmpNowPlayerCount);
        SetMainStep(TurnManager.MainStep.UseItem);
    }
}

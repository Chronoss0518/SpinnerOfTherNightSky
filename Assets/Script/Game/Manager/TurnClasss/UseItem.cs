using UnityEngine;
using Unity.Collections;

public class UseItem : TurnManager.TurnClass
{
    [SerializeField, ReadOnly]
    ScriptManager.ScriptArgumentData selectItem = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptArgumentData selectTrap = null;

    int beforeStackCount = 0;

    int tmpNowPlayerCount = 0;

    int scriptUsePlayerCount = 0;

    int passPlayerCount = 0;

    bool reRunFlg = false;

    public UseItem(TurnManager _manager) : base(_manager)
    {
        selectItem = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--min 0 --max 1 --player-type 0 --zone-type-book --zone-type-item --card-type 2 --normal-playing"),
                new ScriptParts((int)ScriptManager.ScriptType.SelectItemZone, "--target-player"),
                new ScriptParts((int)ScriptManager.ScriptType.MoveCard, "--open-item-zone"),
                new ScriptParts((int)ScriptManager.ScriptType.OpenItemZoneCard,""),
                new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),}));

        selectTrap = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--min 0 --max 1 --player-type 0 --zone-type-item --card-type 4 --normal-playing"),
                new ScriptParts((int)ScriptManager.ScriptType.OpenItemZoneCard, ""),
                new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),}));
    }

    override public void Init() 
    {
        scriptUsePlayerCount = 0;
        beforeStackCount = 0;
        passPlayerCount = 0;
        tmpNowPlayerCount = gameManager.nowPlayerCount;
        gameManager.RegistScript(selectItem);
    }

    public override void Update()
    {
        if (reRunFlg)
        {
            reRunFlg = false;
            gameManager.SetNowPlayerCount(tmpNowPlayerCount);
            Init();
            return;
        }


        int tmpCount = gameManager.stackCount;

        if (beforeStackCount <= 0 && beforeStackCount >= tmpCount)
        {
            ChangeTurn();
            return;
        }

        if(gameManager.playersCount >= passPlayerCount)
        {
            gameManager.RunStackScriptStart();
            reRunFlg = true;
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
        SetMainStep(TurnManager.MainStep.PutStone);
    }
}

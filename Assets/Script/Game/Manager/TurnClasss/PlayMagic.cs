using UnityEngine;
using Unity.Collections;

public class PlayMagic : TurnManager.TurnClass
{
    public PlayMagic(TurnManager _manager) : base(_manager)
    {
        selectMagic = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--player-type 0 --min 0 --max 1 --zone-type-book --card-type 1 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),}));

        selectCard = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--player-type 0 --min 0 --max 1 --zone-type-book --zone-type-item --card-type 5 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveCard, "--open-item-zone"),
                new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),}));
    }

    [SerializeField, ReadOnly]
    ScriptManager.ScriptArgumentData selectMagic = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptArgumentData selectCard = null;

    int beforeStackCount = 0;

    int tmpNowPlayerCount = 0;

    int scriptUsePlayerCount = 0;

    int passPlayerCount = 0;

    public override void Init()
    {
        base.Init();
        beforeStackCount = 0;
        tmpNowPlayerCount = gameManager.nowPlayerCount;
        gameManager.RegistScript(selectMagic);
    }

    public override void Update()
    {
        int tmpCount = gameManager.stackCount;

        if (gameManager.playersCount >= passPlayerCount)
        {
            gameManager.RunStackScriptStart();
            ChangeTurn();
            return;
        }

        if (beforeStackCount < tmpCount)
        {
            scriptUsePlayerCount = gameManager.nowPlayerCount + passPlayerCount;
            passPlayerCount = 1;
            gameManager.SetNowPlayerCount(scriptUsePlayerCount + 1);
            gameManager.RegistScript(selectCard);
            return;
        }

        passPlayerCount++;

        gameManager.SetNowPlayerCount(scriptUsePlayerCount + passPlayerCount);

        gameManager.RegistScript(selectCard);

    }

    public override void Next()
    {
        gameManager.SetNowPlayerCount(tmpNowPlayerCount);
        SetMainStep(TurnManager.MainStep.SetTrap);
    }
}

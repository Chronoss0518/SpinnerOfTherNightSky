using UnityEngine;
using Unity.Collections;

public class PlayMagic : TurnManager.TurnClass
{
    public PlayMagic(TurnManager _manager) : base(_manager)
    {
        selectMagic = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--player-type 0 --min 0 --max 1 --zone-type-book --card-type 1 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.Stack, $"{StackFunction.NORMAL_PLAY_MAGIC_ARG}"),}));

        selectCard = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--player-type 0 --min 0 --max 1 --zone-type-book --zone-type-item --card-type 5 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.OpenItemZoneCard, ""),
                new ScriptParts((int)ScriptManager.ScriptType.Stack, $"{StackFunction.NORMAL_PLAY_MAGIC_ARG}"),}));
    }

    [SerializeField, ReadOnly]
    ScriptManager.ScriptArgumentData selectMagic = null;

    [SerializeField, ReadOnly]
    ScriptManager.ScriptArgumentData selectCard = null;

    int beforeStackCount = 0;

    int scriptUsePlayerCount = 0;

    int passPlayerCount = 0;

    int nowMagicPlayerCount = 0;

    public override void Init()
    {
        beforeStackCount = 0;
        nowMagicPlayerCount = 0;
        passPlayerCount = 0;

        gameManager.RegistScript(selectMagic, gameManager.nowPlayerNo);
    }

    public override void Update()
    {
        if(gameManager.playersCount <= nowMagicPlayerCount)
        {
            ChangeTurn();
            return;
        }

        int tmpCount = gameManager.stackCount;

        if (beforeStackCount <= 0 &&
            beforeStackCount >= tmpCount)
        {
            nowMagicPlayerCount++;
            passPlayerCount = 0;
            gameManager.RegistScript(selectMagic,gameManager.nowPlayerNo + nowMagicPlayerCount);
            return;
        }

        if (gameManager.playersCount <= passPlayerCount + 1)
        {
            gameManager.RunStackScriptStart();
            return;
        }

        if (beforeStackCount < tmpCount)
        {
            scriptUsePlayerCount = gameManager.nowPlayerNo + passPlayerCount + nowMagicPlayerCount;
            beforeStackCount = tmpCount;
            passPlayerCount = 0;
            gameManager.RegistScript(selectCard, scriptUsePlayerCount + 1);
            return;
        }

        passPlayerCount++;

        gameManager.RegistScript(selectCard, scriptUsePlayerCount + passPlayerCount);


    }

    public override void Next()
    {
        SetMainStep(TurnManager.MainStep.SetTrap);
    }
}

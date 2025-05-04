using UnityEngine;
using Unity.Collections;

[System.Serializable]
public class SetTrap : TurnManager.TurnClass
{
    public SetTrap(TurnManager _manager) : base(_manager)
    {
        selectSetItem = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectCard, "--player-type 1 --min 0 --max 1 --zone-type-book --card-type 6 --normal-playing"),
               new ScriptParts((int)ScriptManager.ScriptType.Stack, ""),}));
    }

    [SerializeField, ReadOnly]
    ScriptManager.ScriptArgumentData selectSetItem = null;

    int beforeSetCount = 0;

    public override void Init()
    {
        var player = gameManager.GetNowPlayer();
        beforeSetCount = player.itemZone.nowPutCount;
    }

    public override void Update()
    {

        var player = gameManager.GetNowPlayer();
        int tmp = player.itemZone.nowPutCount;

        if (tmp >= ItemZoneManager.PUT_ITEM_COUNT)
        {
            ChangeTurn();
            return;
        }

        if (beforeSetCount == tmp)
        {
            ChangeTurn();
            return;
        }

        gameManager.RegistScript(selectSetItem);

    }

    public override void Next()
    {
        SetMainStep(TurnManager.MainStep.EndTurn);
    }
}

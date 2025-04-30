public class OpenItemZoneCardFunction : ScriptManager.ScriptFunctionBase
{
    public OpenItemZoneCardFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.MoveCardArgument();

        res.type = ScriptManager.ScriptType.OpenItemZoneCard;

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        ItemZonePosTest();
        TargetCardTest(_controller);

        AddUseScriptCount();
        return true;
    }

    void TargetCardTest(ControllerBase _controller)
    {
        var player = _controller.GetComponent<Player>();
        if (player == null) return;
        if (targetCards.Count > player.itemZone.nowCanPutCount) return;

        foreach(var card in targetCards)
        {
            if (card.zone.zoneType != ScriptManager.ZoneType.ItemZone) continue;
            var itemZone = (ItemZoneManager)card.zone;
            if (itemZone == null) continue;
            itemZone.OpenCard(card.baseData);
        }

        return;
    }


    void ItemZonePosTest()
    {
        if (targetItemZonePos == null) return;
        if (targetItemZonePos.IsOpen()) return;

        targetItemZonePos.SetOpenFlg(true);

    }

}
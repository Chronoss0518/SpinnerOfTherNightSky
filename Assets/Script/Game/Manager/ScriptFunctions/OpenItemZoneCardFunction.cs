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
        var targetPos = GetItemZonePos();

        if (targetPos == null) return true;
        if (targetPos.IsOpen()) return true;

        targetPos.SetOpenFlg(true);

        return true;
    }

}
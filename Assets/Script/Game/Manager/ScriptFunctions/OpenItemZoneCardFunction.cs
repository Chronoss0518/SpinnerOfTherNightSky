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

        if (targetItemZonePos == null) return true;
        if (targetItemZonePos.IsOpen()) return true;

        targetItemZonePos.SetOpenFlg(true);

        return true;
    }

}
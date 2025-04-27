public class StayFunction : ScriptManager.ScriptFunctionBase
{
    public StayFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.BlockStoneArgument();

        res.type = ScriptManager.ScriptType.Stay;

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        AddUseScriptCount();

        return true;
    }

}

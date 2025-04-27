public class BlockCardFunction : ScriptManager.ScriptFunctionBase
{
    public BlockCardFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.BlockStoneArgument();

        res.type = ScriptManager.ScriptType.BlockStone;

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        AddUseScriptCount();

        return true;
    }

}

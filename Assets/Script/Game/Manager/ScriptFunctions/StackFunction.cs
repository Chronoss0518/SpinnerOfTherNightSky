public class StackFunction : ScriptManager.ScriptFunctionBase
{
    public const string NORMAL_PLAY_MAGIC_ARG = "--normal-play-magic";

    public StackFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.StackArgument();

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == NORMAL_PLAY_MAGIC_ARG)
                res.normalPlayMagicFlg = true;
        }

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {

        if (_script.type != ScriptManager.ScriptType.Stack) return false;

        if (targetCards.Count <= 0 || targetCards.Count >= 2)
        {
            AddUseScriptCount();
            return true;
        }

        var arg = (ScriptManager.StackArgument)_script;

        var obj = new StackManager.StackObject(_controller.GetComponent<Player>(), targetCards[0], arg.normalPlayMagicFlg);

        Stack(obj, _gameManager);

        AddUseScriptCount();
        return true;
    }

}
public class PlayMagicInitializeFunction : ScriptManager.ScriptFunctionBase
{
    public PlayMagicInitializeFunction(ScriptManager _manager) : base(_manager)
    {
        controller.Init(_manager);
    }

    public void Init()
    {
        if (isInit) return;
        controller.ClearTarget();

        isInit = true;
    }

    public override void Release()
    {
        isInit = false;
    }


    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.PlayMagicInitializeArgument();

        res.type = ScriptManager.ScriptType.PlayMagicInitialize;

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        Init();
        SetSelectStoneBoardFunctionController(controller);
        return controller.SelectAction(_controller, _gameManager, _script);
    }

    bool isInit = false;
    RemoveStoneFromMagicFunctionController controller = new RemoveStoneFromMagicFunctionController();

}
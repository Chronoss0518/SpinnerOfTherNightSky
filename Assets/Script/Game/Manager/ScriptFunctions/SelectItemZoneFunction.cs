public class SelectItemZoneFunction : ScriptManager.ScriptFunctionBase
{
    public SelectItemZoneFunction(ScriptManager _manager) : base(_manager)
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
        var res = new ScriptManager.SelectItemZoneArgument();

        res.type = ScriptManager.ScriptType.SelectItemZone;

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--target-player")
                res.selectTarget = 1;

            if (args[i] == "--target-other")
                res.selectTarget = 2;
        }

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        Init();
        SetSelectItemZoneFunctionController(controller);
        return controller.SelectAction(_controller, _gameManager, _script);
    }

    bool isInit = false;
    SelectItemZoneFunctionController controller = new SelectItemZoneFunctionController();
}

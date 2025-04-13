using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStoneBoardFunction : ScriptManager.ScriptFunctionBase
{
    public SelectStoneBoardFunction(ScriptManager _manager) : base(_manager)
    {
        controller.Init(_manager);
        SetSelectStoneBoardFunctionController(controller);
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
        var res = new ScriptManager.SelectStoneBoardArgument();

        res.type = ScriptManager.ScriptType.SelectStoneBoard;

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--max" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.maxCount))
                    i += 1;

            if (args[i] == "--min" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.minCount))
                    i += 1;

            if (args[i] == "--is-put")
                res.isPutPos = true;

            if (args[i] == "--is-remove")
                res.isPutPos = false;
        }

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        Init();

        return controller.SelectAction(_controller, _gameManager, _script);
    }

    bool isInit = false;
    SelectStoneBoardFunctionController controller = new SelectStoneBoardFunctionController();

}
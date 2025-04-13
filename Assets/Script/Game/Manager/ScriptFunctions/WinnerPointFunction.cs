using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerPointFunction : ScriptManager.ScriptFunctionBase
{
    public WinnerPointFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.WinnerPointArgument();

        res.type = ScriptManager.ScriptType.WinnerPoint;

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--up")
                res.downFlg = false;

            if (args[i] == "--downFlg")
                res.downFlg = true;

            if (args[i] == "--point" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.point))
                    i += 1;
        }

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        AddUseScriptCount();

        return true;
    }

}

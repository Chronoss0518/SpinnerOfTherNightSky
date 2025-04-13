using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStoneFunction : ScriptManager.ScriptFunctionBase
{
    public MoveStoneFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.MoveStoneArgument();

        res.type = ScriptManager.ScriptType.MoveStone;

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--remove")
                res.removeFlg = true;

            if (args[i] == "--put")
                res.removeFlg = false;
        }

        return res;

    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        var targetStonePos = GetTargetStonePos();

        if (targetStonePos.Count <= 0) return true;

        var act = (ScriptManager.MoveStoneArgument)_script;

        foreach (var pos in targetStonePos)
        {
            var sec = pos.Value;

            sec.UnSelectStonePos();
            if (!act.removeFlg) sec.PutStone(_gameManager.GetNowPlayer().stoneModel);
            else sec.RemovePutStone();
        }

        AddUseScriptCount();

        return true;
    }

}

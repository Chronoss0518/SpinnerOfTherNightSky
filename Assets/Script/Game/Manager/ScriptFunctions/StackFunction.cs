using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackFunction : ScriptManager.ScriptFunctionBase
{
    public StackFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.ScriptArgument();

        res.type = ScriptManager.ScriptType.Stack;

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        var targetCards = GetTargetCard();

        if (targetCards.Count <= 0 || targetCards.Count >= 2)
        {
            AddUseScriptCount();
            return true;
        }

        var card = targetCards[0];

        Stack(card, _gameManager);

        AddUseScriptCount();
        return true;
    }

}
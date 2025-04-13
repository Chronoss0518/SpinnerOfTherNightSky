using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCardFunction : ScriptManager.ScriptFunctionBase
{
    public MoveCardFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.MoveCardArgument();

        res.type = ScriptManager.ScriptType.MoveCard;

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--book")
                res.moveZone = ScriptManager.ZoneType.Book;

            if (args[i] == "--magic-zone")
                res.moveZone = ScriptManager.ZoneType.MagicZone;

            if (args[i] == "--open-item-zone")
            {
                res.openFlg = true;
                res.moveZone = ScriptManager.ZoneType.ItemZone;
            }

            if (args[i] == "--close-item-zone")
            {
                res.openFlg = false;
                res.moveZone = ScriptManager.ZoneType.ItemZone;
            }

            if (args[i] == "--trash-zone")
                res.moveZone = ScriptManager.ZoneType.TrashZone;
        }

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        var targetCards = GetTargetCard();

        if (targetCards.Count <= 0)
        {
            AddUseScriptCount();
            return true;
        }

        var act = (ScriptManager.MoveCardArgument)_script;

        for (int i = 0; i< targetCards.Count; i++)
        {
            if (act.moveZone == ScriptManager.ZoneType.MagicZone && targetCards[i].type == CardData.CardType.Item) continue;
            if (act.moveZone == ScriptManager.ZoneType.ItemZone && targetCards[i].type == CardData.CardType.Magic) continue;


        }

        AddUseScriptCount();

        return true;
    }

}
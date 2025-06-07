using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using static ScriptManager;

public class SelectItemZoneFunctionController : SelectItemZoneControllerBase
{

    public ItemZoneObject GetTargetPos() { return targetItemZonePos; }

    public override void ClearTarget()
    {
        targetItemZonePos = null;
    }

    public override bool SelectPos(ItemZoneObject _pos, GameManager _manager, ScriptArgument _runArgument)
    {
        var runArgument = (SelectItemZoneArgument)_runArgument;
        if (_runArgument == null) return false;
        if (_pos == null) return false;

        if(targetItemZonePos != null && targetItemZonePos != _pos)
        {
            manager.SetError(ErrorType.IsNotTargetZone);
            return false;
        }

        targetItemZonePos = targetItemZonePos == _pos ? null : _pos;
        return true;
    }


    public override bool SelectAction(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.SelectItemZone) return false;

        if (targetCards.Count <= 0)
        {
            manager.AddUseScriptCount();
            return true;
        }

        bool actionFlg = false;

        foreach(var card in targetCards)
        {
            if (card.zone.zoneType == ZoneType.ItemZone) continue;
            actionFlg = true;
            break;
        }

        if(!actionFlg)
        {
            manager.AddUseScriptCount();
            return true;
        }

        var act = (SelectItemZoneArgument)_script;

        _controller.ActionStart();
        StartManager(_gameManager,act);


        string message = "";

        if (manager.GetErrorType() == ErrorType.None)
        {
            message = $"カードを配置するItemZoneを指定してください。\n{(targetItemZonePos != null ? "選択済み" : "未選択")}";
        }
        else
        {
            if (manager.GetErrorType() == ErrorType.IsNotTargetZone) message = $"選択した場所は既にカードが置かれています。";
            if (manager.GetErrorType() == ErrorType.IsRangeMinOverCount) message = $"配置先のItemZoneが選択されていません。";

            manager.DownErrorMessageDrawCount();
        }

        _gameManager.SetMessate(message);

        if (!_controller.isAction) return true;
        if (targetItemZonePos == null)
        {
            manager.SetError(ErrorType.IsRangeMinOverCount);
            _controller.DownActionFlg();
            return true;
        }

        _controller.ActionEnd();
        manager.AddUseScriptCount();
        _gameManager.EndSelectItemZone();
        startFlg = false;
        return true;
    }

    private void StartManager(GameManager _manager, SelectItemZoneArgument _arg)
    {
        if (startFlg) return;

        _manager.StartSelectItemZone(_arg);

        startFlg = true;
    }

    bool startFlg = false;

}

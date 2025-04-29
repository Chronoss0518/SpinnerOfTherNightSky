using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using static ScriptManager;

[System.Serializable]
public class SelectItemZoneFunctionController : SelectScriptControllerBase
{

    [SerializeField, ReadOnly]
    ScriptManager manager = null;

    public ItemZoneObject targetPos = null;

    public ItemZoneObject GetTargetPos() { return targetPos; }

    public override void ClearTarget()
    {
        targetPos = null;
    }

    public void Init(ScriptManager _manager)
    {
        manager = _manager;
    }

    public bool SelectPos(ItemZoneObject _pos, GameManager _manager, SelectItemZoneArgument _runArgument)
    {
        if (_runArgument == null) return false;
        if (_pos == null) return false;
        Debug.Log($"Select Item Zone Pos{_pos}");
        if(targetPos != null && targetPos != _pos)
        {
            manager.SetError(ErrorType.IsNotTargetZone);
            return false;
        }

        targetPos = targetPos == _pos ? null : _pos;
        return true;
    }


    public override bool SelectAction(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.SelectItemZone) return false;
        var act = (SelectItemZoneArgument)_script;

        _controller.ActionStart();
        StartManager(_gameManager,act);


        string message = "";

        if (manager.GetErrorType() == ErrorType.None)
        {
            message = $"カードを配置するItemZoneを指定してください。\n{(targetPos != null ? "選択済み" : "未選択")}";
        }
        else
        {
            if (manager.GetErrorType() == ErrorType.IsNotTargetZone) message = $"選択した場所は既にカードが置かれています。";
            if (manager.GetErrorType() == ErrorType.IsRangeMinOverCount) message = $"配置先のItemZoneが選択されていません。";

            manager.DownErrorMessageDrawCount();
        }

        _gameManager.SetMessate(message);

        if (!_controller.isAction) return true;
        if (targetPos == null)
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

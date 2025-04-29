using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using static ScriptManager;

public class SelectItemZoneFunctionController : SelectScriptControllerBase
{

    [SerializeField, ReadOnly]
    ScriptManager manager = null;

    public int TargetPos { get; private set; } = -1;


    public override void ClearTarget()
    {
        TargetPos = -1;
    }

    public void Init(ScriptManager _manager)
    {
        manager = _manager;
    }

    public bool SelectPos(int _pos, GameManager _manager, SelectItemZoneArgument _runArgument)
    {
        if (_runArgument == null) return false;
        if (_pos < 0) return false;
        if (_pos >= ItemZoneManager.PUT_ITEM_COUNT) return false;
        if(TargetPos != -1 && TargetPos != _pos)
        {
            manager.SetError(ErrorType.IsNotTargetZone);
            return false;
        }

        TargetPos = TargetPos == _pos ? -1 : _pos;
        return true;
    }


    public override bool SelectAction(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.SelectItemZone) return false;
        StartManager(_gameManager);

        var act = (SelectItemZoneArgument)_script;

        string message = "";

        if (manager.GetErrorType() == ErrorType.None)
        {
            message = $"カードを配置するItemZoneを指定してください。\n{(TargetPos >= 0 ? "選択済み" : "未選択")}";
        }
        else
        {
            if (manager.GetErrorType() == ErrorType.IsNotTargetZone) message = $"選択した場所は既にカードが置かれています。";
            if (manager.GetErrorType() == ErrorType.IsRangeMinOverCount) message = $"配置先のItemZoneが選択されていません。";

            manager.DownErrorMessageDrawCount();
        }

        _gameManager.SetMessate(message);

        if (!_controller.isAction) return true;
        if (TargetPos < 0)
        {
            manager.SetError(ErrorType.IsRangeMinOverCount);
            _controller.DownActionFlg();
            return true;
        }

        _controller.ActionEnd();
        manager.AddUseScriptCount();
        _gameManager.EndSelectCardTest();
        startFlg = false;
        return true;
    }

    private void StartManager(GameManager _manager)
    {
        if (startFlg) return;

        startFlg = true;
    }

    bool startFlg = false;

}

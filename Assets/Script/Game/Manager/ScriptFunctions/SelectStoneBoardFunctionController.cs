using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static ScriptManager;

public class SelectStoneBoardFunctionController : SelectScriptControllerBase
{
    public Dictionary<int, StonePosScript> GetTargetStonePos()
    {
        return targetStonePos;
    }

    public override void ClearTarget()
    {
        targetStonePos.Clear();
    }

    public void SelectTargetPos(int _x, int _y, GameManager _manager,SelectStoneBoardArgument _runArgument)
    {
        if (_runArgument == null) return;
        if (_manager.stoneBoardObj.IsPutStone(_x, _y) == _runArgument.isPutPos)
        {
            manager.SetError(_runArgument.isPutPos ?
                ErrorType.IsPutStonePosSelect :
                ErrorType.IsRemoveStonePosSelect);

            return;
        }

        int pos = _manager.stoneBoardObj.CreatePosKey(_x, _y);

        if (targetStonePos.ContainsKey(pos))
        {
            targetStonePos[pos].UnSelectStonePos();
            targetStonePos.Remove(pos);
            return;
        }

        if (_runArgument.maxCount <= targetStonePos.Count)
        {
            manager.SetError(ErrorType.IsRangeMaxOverCount);
            return;
        }

        var script = _manager.stoneBoardObj.GetStonePosScript(_x, _y);
        targetStonePos.Add(pos, script);
        _manager.stoneBoardObj.SelectStonePos(_x, _y);

    }


    public override bool SelectAction(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.SelectStoneBoard) return false;

        _controller.ActionStart();

        var act = (SelectStoneBoardArgument)_script;

        string message = "";

        if (manager.GetErrorType() == ErrorType.None)
        {
            message = !act.isPutPos ? "�΂���菜���ꏊ��" : "�΂�u���ꏊ��";

            string tmp = act.minCount > targetStonePos.Count ?
                $"�c��:{act.minCount - targetStonePos.Count}" :
                "�I���ς�";

            message = act.minCount != act.maxCount ?
                $"{message}{act.minCount}����{act.maxCount}�I�����Ă��������B\n{tmp}" :
                 $"{message}{act.minCount}�I�����Ă��������B\n{tmp}";

        }
        else
        {

            manager.DownErrorMessageDrawCount();
            if (manager.GetErrorType() == ErrorType.IsRangeMaxOverCount) message = "����ȏ�I���ł��܂���";
            if (manager.GetErrorType() == ErrorType.IsRangeMinOverCount) message = $"{act.minCount}�ȏ�I�����Ă�������";
            if (manager.GetErrorType() == ErrorType.IsPutStonePosSelect) message = $"���ɐ΂��u����Ă��܂�";
            if (manager.GetErrorType() == ErrorType.IsRemoveStonePosSelect) message = $"���̏ꏊ�ɂ͐΂�����܂���";
        }

        _gameManager.SetMessate(message);

        if (!_controller.isAction) return true;
        if (act.minCount > targetStonePos.Count)
        {
            manager.SetError(ErrorType.IsRangeMinOverCount);
            _controller.DownActionFlg();
            return true;
        }

        _controller.ActionEnd();
        manager.AddUseScriptCount();

        return true;
    }

}

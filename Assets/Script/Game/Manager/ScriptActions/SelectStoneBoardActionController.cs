using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static ScriptManager;

public class SelectStoneBoardActionController : SelectScriptActionBase
{
    [SerializeField,ReadOnly]
    ScriptManager manager = null;

    [SerializeField, ReadOnly]
    Dictionary<int, StonePosScript> targetStonePos = new Dictionary<int, StonePosScript>();

    public Dictionary<int, StonePosScript> GetTargetStonePos()
    {
        return targetStonePos;
    }

    public override void ClearTarget()
    {
        targetStonePos.Clear();
    }

    public void Init(ScriptManager _manager)
    {
        manager = _manager;
    }


    public void SelectTargetPos(int _x, int _y, GameManager _manager,SelectStoneBoardAction _runAction)
    {
        if (_runAction == null) return;
        if (_manager.stoneBoardObj.IsPutStone(_x, _y) == _runAction.isPutPos)
        {
            manager.SetError(_runAction.isPutPos ?
                ErrorType.IsPutStonePosSelect :
                ErrorType.IsRemoveStonePosSelect);

            return;
        }

        int pos = _x + (_y * _manager.stoneBoardObj.HOLYZONTAL_SIZE);

        if (targetStonePos.ContainsKey(pos))
        {
            targetStonePos[pos].UnSelectStonePos();
            targetStonePos.Remove(pos);
            return;
        }

        if (_runAction.maxCount <= targetStonePos.Count)
        {
            manager.SetError(ErrorType.IsRangeMaxOverCount);
            return;
        }

        var script = _manager.stoneBoardObj.GetStonePosScript(_x, _y);
        targetStonePos.Add(pos, script);
        _manager.stoneBoardObj.SelectStonePos(_x, _y);

    }


    public override bool SelectAction(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.SelectStoneBoard) return false;

        _controller.ActionStart();

        var act = (SelectStoneBoardAction)_script;

        string message = "";

        if (manager.GetErrorType() == ErrorType.None)
        {
            string tmp = act.minCount > targetStonePos.Count ?
                $"�c��:{act.minCount - targetStonePos.Count}" :
                "�I���ς�";

            message = act.minCount != act.maxCount ?
                $"�΂�u���ꏊ��{act.minCount}����{act.maxCount}�I�����Ă��������B\n{tmp}" :
                 $"�΂�u���ꏊ��{act.minCount}�I�����Ă��������B\n{tmp}";

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

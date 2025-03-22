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
                $"残り:{act.minCount - targetStonePos.Count}" :
                "選択済み";

            message = act.minCount != act.maxCount ?
                $"石を置く場所を{act.minCount}から{act.maxCount}選択してください。\n{tmp}" :
                 $"石を置く場所を{act.minCount}選択してください。\n{tmp}";

        }
        else
        {

            manager.DownErrorMessageDrawCount();
            if (manager.GetErrorType() == ErrorType.IsRangeMaxOverCount) message = "これ以上選択できません";
            if (manager.GetErrorType() == ErrorType.IsRangeMinOverCount) message = $"{act.minCount}以上選択してください";
            if (manager.GetErrorType() == ErrorType.IsPutStonePosSelect) message = $"既に石が置かれています";
            if (manager.GetErrorType() == ErrorType.IsRemoveStonePosSelect) message = $"その場所には石がありません";
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

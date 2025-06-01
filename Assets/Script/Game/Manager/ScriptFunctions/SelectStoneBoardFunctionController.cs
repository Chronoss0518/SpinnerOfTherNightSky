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
            message = !act.isPutPos ? "石を取り除く場所を" : "石を置く場所を";

            string tmp = act.minCount > targetStonePos.Count ?
                $"残り:{act.minCount - targetStonePos.Count}" :
                "選択済み";

            message = act.minCount != act.maxCount ?
                $"{message}{act.minCount}から{act.maxCount}選択してください。\n{tmp}" :
                 $"{message}{act.minCount}選択してください。\n{tmp}";

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

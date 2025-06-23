using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using static ScriptManager;

public class RemoveStoneFromMagicFunctionController : SelectStoneBoardControllerBase
{
    public Dictionary<int, StonePosScript> GetTargetStonePos()
    {
        return targetStonePos;
    }

    public override void ClearTarget()
    {
        targetStonePos.Clear();
        if (stoneBoard != null) stoneBoard.AllSelectDisable();
        stoneBoard = null;
        targetPositions.Clear();
    }

    public void Init(GameManager _manager, PlayMagicInitializeArgument _runArgument)
    {
        if (targetPositions.Count > 0) return;
        if (_runArgument == null) return;
        if (_manager == null) return;
        if (_runArgument.playMagicCard == null) return;
        if (_runArgument.playMagicCard.cardType != (int)CardData.CardType.Magic) return;

        var useScriptPlayer = _manager.GetPlayer(_manager.useScriptPlayerNo);

        stoneBoard = _manager.stoneBoardObj;

        //for (int v = findData.minPos.y; v < stoneBoard.PANEL_COUNT_Y - findData.maxPos.y; v++)
        for (int v = 0; v < stoneBoard.PANEL_COUNT_Y; v++)
        {
            //for (int h = findData.minPos.x; h < stoneBoard.PANEL_COUNT_X - findData.maxPos.x; h++)
            for (int h = 0; h < stoneBoard.PANEL_COUNT_X; h++)
            {
                var list = findStarFromMagicManager.FindStarPos(h, v, _runArgument.playMagicCard, useScriptPlayer);
                if (list.Count <= 0) continue;
                targetPosCount = list.Count;

                for (int count = 0; count < list.Count; count++)
                {
                    stoneBoard.SelectEnable(list[count].x, list[count].y);
                }

                targetPositions.Add(list.ToArray());
            }
        }

        if (removeStone != null) return;

        removeStone = _manager.CreateScript(new ScriptData(
            new ScriptParts[] {
               new ScriptParts((int)ScriptManager.ScriptType.MoveStone, "--remove"),}));

    }

    public override void SelectTargetPos(int _x, int _y, GameManager _manager, ScriptArgument _runArgument)
    {
        var runArgument = (PlayMagicInitializeArgument)_runArgument;
        if (runArgument == null) return;

        if (!_manager.stoneBoardObj.IsPutStone(_x, _y))
        {
            manager.SetError(ErrorType.IsRemoveStonePosSelect);
            return;
        }

        int pos = _manager.stoneBoardObj.CreatePosKey(_x, _y);

        if (targetStonePos.ContainsKey(pos))
        {
            targetStonePos[pos].UnSelectStonePos();
            targetStonePos.Remove(pos);
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

        var act = (PlayMagicInitializeArgument)_script;

        Init(_gameManager, act);
        
        SelectActionNormal(_controller, _gameManager);

        SelectActionException(_controller, _gameManager);

        if (!_controller.isAction) return true;

        if(targetPosCount > 0)
        {
            if (targetPosCount != targetStonePos.Count && targetStonePos.Count <= 0)
            {
                manager.SetError(ErrorType.IsRangeMinOverCount);
                _controller.DownActionFlg();
                return true;
            }
        }

        _gameManager.RegistScript(removeStone,_gameManager.useScriptPlayerNo);

        _controller.ActionEnd();
        manager.AddUseScriptCount();

        return true;
    }


    private void SelectActionNormal(ControllerBase _controller, GameManager _gameManager)
    {
        if (targetPosCount <= 0) return;

        string message = "";

        if (manager.GetErrorType() == ErrorType.None)
        {
            message = "�΂���菜���ꏊ��";

            string tmp = targetPosCount > targetStonePos.Count ?
                $"�c��:{targetPosCount - targetStonePos.Count}" :
                "�I���ς�";

            message = $"{message}{targetPosCount}�I�����Ă��������B\n�g�p���悤�Ƃ��Ă���p���g�p���Ȃ��ꍇ�͐΂�I�������ɂ��̂܂܌���{�^���������Ă��������B\n{tmp}";

        }
        else
        {
            manager.DownErrorMessageDrawCount();
            if (manager.GetErrorType() == ErrorType.IsRangeMaxOverCount ||
                manager.GetErrorType() == ErrorType.IsRangeMinOverCount) message = $"�J�[�h�Ɏw�肳��Ă���΂̔z�u�ɉ����Đ΂�I�����Ă�������";
            if (manager.GetErrorType() == ErrorType.IsRemoveStonePosSelect) message = $"���̏ꏊ�ɂ͐΂�����܂���";
        }

        _gameManager.SetMessate(message);

    }

    private void SelectActionException(ControllerBase _controller, GameManager _gameManager)
    {
        if (targetPosCount > 0) return;

        _gameManager.SetMessate("�΂���菜�����Ƃ��ł��Ȃ��Ȃ�܂���");
    }

    private void UpdateSelectEnableStonePos(Vector2Int[] _list,GameManager _manager)
    {
        for (int count = 0; count < _list.Length; count++)
        {
            stoneBoard.SelectEnable(_list[count].x, _list[count].y);
        }
    }

    FindStarFromMagicManager findStarFromMagicManager = FindStarFromMagicManager.ins;
    int targetPosCount = 0;
    ScriptArgumentData removeStone = null;
    StoneBoardManager stoneBoard = null;

    List<Vector2Int[]> targetPositions = new List<Vector2Int[]>();

}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class ScriptManager
{
    public ScriptManager()
    {
        selectStoneBoardActionController.Init(this);
        selectCardActionController.Init(this);
    }

    abstract public class SelectScriptActionBase
    {
        abstract public bool SelectAction(ControllerBase _controller, GameManager _gameManager, ScriptAction _script);

        abstract public void ClearTarget();
    }


    delegate ScriptAction CreateMethod(ScriptParts _parts);

    public enum ErrorType : int
    {
        IsPutStonePosSelect,
        IsRemoveStonePosSelect,
        IsRangeMaxOverCount,
        IsRangeMinOverCount,
        IsNotTargetZone,
        IsNotRemoveStones,
        IsNotPutItemZone,
        IsNotParticialName,
        IsNotParticialDescription,
        None,
    }

    public enum ScriptType : int
    {
        BlockStone,//石を取り除かせない・カードを無効にする//
        BlockCard,//石を取り除かせない・カードを無効にする//
        SelectStoneBoard,//盤面の場所を選択する//
        SelectCard,//魔導書からカードを選択する//
        MoveStone,//石の置く・石を取り除く//
        MoveCard,//カードを移動させる//
        Stack,//カードをStackする→カードの発動準備//
        WinnerPoint,//ゲームに勝利するためのポイントを増減させる//
    }

    public enum ActionType : int
    {
        Entry,
        Stay,
        Exit,
    }

    [Flags]
    public enum ZoneType : int
    {
        Book = 1,
        MagicZone = 2,
        ItemZone = 4,
        TrashZone = 8,
    }

    [Flags]
    public enum SelectCardType :int
    {
        Magic = 1,
        Item = 2,
        Trap = 4
    }

    public class ScriptAction
    {
        public ScriptType type;
    }

    public class BlockStoneAction : ScriptAction
    {
        public BlockStoneAction()
        {
            type = ScriptType.BlockStone;
        }
    }

    public class BlockCardAction : ScriptAction
    {
        public BlockCardAction()
        {
            type = ScriptType.BlockCard;
        }
    }

    public class SelectStoneBoardAction : ScriptAction
    {
        public SelectStoneBoardAction()
        {
            type = ScriptType.SelectStoneBoard;
        }

        public int minCount = 0;
        public int maxCount = 0;
        public bool isPutPos = true;
    }

    public class SelectCardAction : ScriptAction
    {
        public SelectCardAction()
        {
            type = ScriptType.SelectCard;
        }

        public int selectMinCount = 0;
        public int selectMaxCount = 1;
        //0で自身//
        //1で自身以外//
        public int playerType = -1;
        public ZoneType zoneType = 0;
        public SelectCardType cardType = 0;
        public List<int> magicAttributeMonth = new List<int>();
        public bool normalPlaying = true;
        public List<string> particialName = new List<string>();
        public List<string> particialDescription = new List<string>();

        public void AddAttributeType(int _type){

            if (_type == (int)MagicCardData.CardAttribute.Spring){
                magicAttributeMonth.Add(2);
                magicAttributeMonth.Add(3);
                magicAttributeMonth.Add(4);
            }

            if (_type == (int)MagicCardData.CardAttribute.Summer){
                magicAttributeMonth.Add(5);
                magicAttributeMonth.Add(6);
                magicAttributeMonth.Add(7);
            }

            if (_type == (int)MagicCardData.CardAttribute.Autumn){
                magicAttributeMonth.Add(8);
                magicAttributeMonth.Add(9);
                magicAttributeMonth.Add(10);
            }

            if (_type == (int)MagicCardData.CardAttribute.Winter){
                magicAttributeMonth.Add(11);
                magicAttributeMonth.Add(0);
                magicAttributeMonth.Add(1);
            }
        }
    }

    public class MoveStoneAction : ScriptAction
    {
        public MoveStoneAction()
        {
            type = ScriptType.MoveStone;
        }

        public bool removeFlg = false;
    }

    public class MoveCardAction : ScriptAction
    {
        public MoveCardAction()
        {
            type = ScriptType.MoveCard;
        }

        public bool openFlg = false;
        public ZoneType moveZone = 0;
    }

    public class WinnerPointAction : ScriptAction
    {
        public WinnerPointAction()
        {
            type = ScriptType.WinnerPoint;
        }

        public bool downFlg = false;
        public int point = 0;
    }

    public class ScriptActionData
    {
        public ActionType type = ActionType.Entry;

        public List<ScriptAction> actions = new List<ScriptAction>();
    }

    ScriptActionData runScript = null;
    public bool isRunScript { get { return runScript != null; } }

    public ScriptType RunScriptType { get { return runScript.actions[useScriptCount].type; } }


    List<ScriptActionData> stayScript = new List<ScriptActionData>();

    [SerializeField, ReadOnly]
    List<Player> targetPlayer = new List<Player>();

    public int GetTargetPlayerCount { get { return targetPlayer.Count; } }

    SelectStoneBoardActionController selectStoneBoardActionController = new SelectStoneBoardActionController();

    SelectCardActionController selectCardActionController = new SelectCardActionController();


    [SerializeField,ReadOnly]
    int useScriptCount = 0;

    public void AddUseScriptCount() { useScriptCount++; }

    [SerializeField]
    int errorMessageDrawMaxCount = 0;

    [SerializeField, ReadOnly]
    int errorMessageDrawCount = -1;

    public int GetErrorMessageDrawCount() { return errorMessageDrawCount; }

    public void DownErrorMessageDrawCount()
    {
        if (errorMessageDrawCount < 0)
        {
            errorType = ErrorType.None;
            return;
        }

        errorMessageDrawCount--;
    }

    [SerializeField, ReadOnly]
    ErrorType errorType = ErrorType.None;

    public ErrorType GetErrorType() { return errorType; }

    public void SetError(ErrorType _type)
    {
        if (_type == ErrorType.None) return;
        errorType = _type;
        errorMessageDrawCount = errorMessageDrawMaxCount;
    }

    [SerializeField]
    GameObject selectStonePrefab = null;

    public void SelectTargetPos(int _x,int _y,GameManager _manager)
    {
        if (runScript == null) return;
        if (runScript.actions[useScriptCount].type != ScriptType.SelectStoneBoard) return;

        var action = (SelectStoneBoardAction)runScript.actions[useScriptCount];

        selectStoneBoardActionController.SelectTargetPos(_x, _y, _manager, action);
    }

    public ScriptActionData CreateScript(ScriptData _script, bool _regist = false)
    {

        var res = new ScriptActionData();
        if (_script.parts == null) return res;
        if (_script.parts.Length <= 0) return res;

        res.type = _script.type;

        for(int i = 0;i<_script.parts.Length;i++)
        {
            var scr = _script.parts[i];

            if (CreateScriptAction(res, CreateBlockStoneAction, scr)) continue;
            if (CreateScriptAction(res, CreateBlockCardAction, scr)) continue;
            if (CreateScriptAction(res, CreateSelectStoneBoardAction, scr)) continue;
            if (CreateScriptAction(res, CreateSelectCardAction, scr)) continue;
            if (CreateScriptAction(res, CreateMoveStoneAction, scr)) continue;
            if (CreateScriptAction(res, CreateMoveCardAction, scr)) continue;
            if (CreateScriptAction(res, CreateStackAction, scr)) continue;
            if (CreateScriptAction(res, CreateWinnerPointAction, scr)) continue;
        }

        if (res.type == ActionType.Stay)
        {
            stayScript.Add(res);
            return null;
        }

        if (_regist)
            runScript = res;

        return res;
    }

    public void SetRunScript(ScriptActionData _data)
    {
        if (_data == null) return;
        runScript = _data;
    }

    public void IsStayScriptTest()
    {

    }

    public void RunScript(ControllerBase _controller, GameManager _gameManager)
    {
        if (runScript == null) return;
        if (runScript.actions.Count <= useScriptCount)
        {
            runScript = null;
            useScriptCount = 0;
            targetPlayer.Clear();
            selectCardActionController.ClearTarget();
            selectStoneBoardActionController.ClearTarget();
            return;
        }

        if (BlockStone(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (BlockCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (SelectStoneBoard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (SelectCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (MoveStone(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (MoveCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (Stack(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (WinnerPoint(_controller, _gameManager, runScript.actions[useScriptCount])) return;

    }

    bool BlockStone(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.BlockStone) return false;

        return true;
    }


    bool BlockCard(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.BlockCard) return false;

        return true;
    }


    bool SelectStoneBoard(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        return selectStoneBoardActionController.SelectAction(_controller, _gameManager, _script);
    }


    bool SelectCard(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.SelectCard) return false;

        if (!_controller.isAction) return true;

        useScriptCount++;

        return true;
    }

    bool MoveStone(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.MoveStone) return false;

        var act = (MoveStoneAction)_script;

        foreach (var pos in selectStoneBoardActionController.GetTargetStonePos())
        {
            var sec = pos.Value;

            sec.UnSelectStonePos();
            if (!act.removeFlg) sec.PutStone(_gameManager.GetNowPlayer().stoneModel);
            else sec.RemovePutStone();
        }

        return true;
    }

    bool MoveCard(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.MoveCard) return false;

        return true;
    }

    bool Stack(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.Stack) return false;

        return true;
    }

    bool WinnerPoint(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.WinnerPoint) return false;

        return true;
    }


    bool CreateScriptAction(ScriptActionData _data, CreateMethod _method,ScriptParts _parts)
    {
        if (_method == null) return false;
        if (_data == null) return false;

        var act = _method(_parts);

        if (act == null) return false;

        _data.actions.Add(act);

        return true;
    }

    ScriptAction CreateBlockStoneAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.BlockStone) return null;

        var res = new BlockStoneAction();

        res.type = ScriptType.BlockStone;

        return res;
    }

    ScriptAction CreateBlockCardAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.BlockCard) return null;

        var res = new BlockCardAction();

        res.type = ScriptType.BlockCard;

        return res;
    }

    ScriptAction CreateSelectStoneBoardAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.SelectStoneBoard) return null;

        var res = new SelectStoneBoardAction();

        res.type = ScriptType.SelectStoneBoard;

        var args = GenerateArgment(_script.argments);

        for(int i = 0; i < args.Count;i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--max" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.maxCount))
                    i += 1;

            if (args[i] == "--min" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.minCount))
                    i += 1;

            if (args[i] == "--is-put")
                res.isPutPos = true;

            if (args[i] == "--is-remove")
                res.isPutPos = false;
        }

        return res;

    }

    ScriptAction CreateSelectCardAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.SelectCard) return null;

        var res = new SelectCardAction();

        res.type = ScriptType.SelectCard;

        var args = GenerateArgment(_script.argments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--max" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.selectMaxCount))
                    i += 1;

            if (args[i] == "--min" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.selectMinCount))
                    i += 1;

            if (args[i] == "--player-type" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.playerType))
                    i += 1;


            if (args[i] == "--zone-type-book" && args.Count > i + 1)
                res.zoneType |= ZoneType.Book;

            if (args[i] == "--zone-type-magic-zone" && args.Count > i + 1)
                res.zoneType |= ZoneType.MagicZone;

            if (args[i] == "--zone-type-item-zone" && args.Count > i + 1)
                res.zoneType |= ZoneType.ItemZone;

            if (args[i] == "--zone-type-trash-zone" && args.Count > i + 1)
                res.zoneType |= ZoneType.TrashZone;


            if (args[i] == "--card-type" && args.Count > i + 1)
            {
                var type = 0;
                if (int.TryParse(args[i + 1], out type))
                {
                    res.cardType = (SelectCardType)(type);
                    i += 1;
                }
            }

            if (args[i] == "--magic-attribute-month" && args.Count > i + 1)
            {
                var no = 0;
                if (int.TryParse(args[i + 1], out no))
                {
                    res.magicAttributeMonth.Add(no);
                    i += 1;
                }
            }

            if (args[i] == "--magic-attribute-type" && args.Count > i + 1)
            {
                var type = 0;
                if (int.TryParse(args[i + 1], out type))
                {
                    res.AddAttributeType(type);
                    i += 1;
                }
            }

            if (args[i] == "--normal-playing")
            {
                res.normalPlaying = true;
            }

            if (args[i] == "--particial-name" && args.Count > i + 1)
            {
                res.particialName.Add(args[i + 1]);
                i += 1;
            }

            if (args[i] == "--particial-description" && args.Count > i + 1)
            {
                res.particialDescription.Add(args[i + 1]);
                i += 1;
            }

        }

        return res;

    }

    ScriptAction CreateMoveStoneAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.MoveStone) return null;

        var res = new MoveStoneAction();

        res.type = ScriptType.MoveStone;

        var args = GenerateArgment(_script.argments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--remove")
                res.removeFlg = true;

            if (args[i] == "--put")
                res.removeFlg = false;
        }

        return res;

    }

    ScriptAction CreateMoveCardAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.MoveCard) return null;

        var res = new MoveCardAction();

        res.type = ScriptType.MoveCard;

        var args = GenerateArgment(_script.argments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--book")
                res.moveZone = ZoneType.Book;

            if (args[i] == "--magic-zone")
                res.moveZone = ZoneType.MagicZone;

            if (args[i] == "--item-zone")
                res.moveZone = ZoneType.ItemZone;

            if (args[i] == "--open-item-zone")
            {
                res.openFlg = true;
                res.moveZone = ZoneType.ItemZone;
            }

            if (args[i] == "--close-item-zone")
            {
                res.openFlg = false;
                res.moveZone = ZoneType.ItemZone;
            }

            if (args[i] == "--trash-zone")
                res.moveZone = ZoneType.TrashZone;
        }

        return res;
    }

    ScriptAction CreateStackAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.WinnerPoint) return null;

        var res = new ScriptAction();

        res.type = ScriptType.Stack;

        return res;
    }

    ScriptAction CreateWinnerPointAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.WinnerPoint) return null;

        var res = new WinnerPointAction();

        res.type = ScriptType.WinnerPoint;

        var args = GenerateArgment(_script.argments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--up")
                res.downFlg = false;

            if (args[i] == "--downFlg")
                res.downFlg = true;

            if (args[i] == "--point" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.point))
                    i += 1;
        }

        return res;
    }

    List<string> GenerateArgment(string _args)
    {
        var args = _args.Split(' ');

        if (args.Length <= 0) return null;

        var res = new List<string>();

        bool strFlg = false;
        string tmp = "";

        foreach(var arg in args)
        {
            if(arg.IndexOf("\"") >= 0)
                strFlg = true;

            if (strFlg)
                tmp += (tmp.Length > 0 ? " " : "") + arg;

            if (arg.IndexOf("\"", arg.Length - 1) >= 0)
                strFlg = false;

            if (strFlg) continue;

            if (tmp.Length <= 0)
            {
                res.Add(arg);

                continue;
            }

            tmp.Replace("\"","");

            res.Add(tmp);

        }


        return res;
    }

}
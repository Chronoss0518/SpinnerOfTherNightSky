using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class ScriptManager
{
    delegate ScriptAction CreateMethod(ScriptParts _parts);

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
        Magic,
        Item,
        Trap
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
    }

    public class SelectCardAction : ScriptAction
    {
        public SelectCardAction()
        {
            type = ScriptType.SelectCard;
        }

        public int selectMinCount = 0;
        public int selectMaxCount = 1;
        //1で自身//
        //2で自身以外//
        public int playerType = -1;
        public ZoneType zoneType = 0;
        public SelectCardType cardType = 0;
        public List<int> magicAttributeMonth = new List<int>();
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

    public bool isRunScript { get { return runScript != null; } }

    ScriptActionData runScript = null;
    List<ScriptActionData> stayScript = new List<ScriptActionData>();

    [SerializeField, ReadOnly]
    List<Player> targetPlayer = new List<Player>();

    [SerializeField, ReadOnly]
    List<CardScript> targetCard = new List<CardScript>();

    [SerializeField,ReadOnly]
    List<int> targetStonePos = new List<int>();

    int useScriptCount = 0;


    [SerializeField]
    int errorMessageDrawMaxCount = 0;

    [SerializeField, ReadOnly]
    int errorMessageDrawCount = -1;


    public void SelectTargetPos(int _pos)
    {
        Debug.Log($"targetStonePos Count [{targetStonePos.Count}]");

        if (runScript == null) return;
        if (runScript.actions[useScriptCount].type != ScriptType.SelectStoneBoard) return;


        for (int i = 0;i < targetStonePos.Count;i++)
        {
            if (_pos == targetStonePos[i])
            {
                targetStonePos.RemoveAt(i);
                return;
            }
        }


        var script = (SelectStoneBoardAction)runScript.actions[useScriptCount];

        if (script.maxCount <= targetStonePos.Count)
        {
            errorMessageDrawCount = errorMessageDrawMaxCount;
        }

        targetStonePos.Add(_pos);
    }


    public ScriptActionData CreateScript(ScriptData _script, bool _regist = false)
    {
        var res = new ScriptActionData();
        res.type = _script.type;

        foreach(var scr in _script.parts)
        {
            if (CreateScriptAction(res, CreateBlockStoneAction, _script.parts[useScriptCount])) continue;
            if (CreateScriptAction(res, CreateBlockCardAction, _script.parts[useScriptCount])) continue;
            if (CreateScriptAction(res, CreateSelectStoneBoardAction, _script.parts[useScriptCount])) continue;
            if (CreateScriptAction(res, CreateSelectCardAction, _script.parts[useScriptCount])) continue;
            if (CreateScriptAction(res, CreateMoveStoneAction, _script.parts[useScriptCount])) continue;
            if (CreateScriptAction(res, CreateMoveCardAction, _script.parts[useScriptCount])) continue;
            if (CreateScriptAction(res, CreateWinnerPointAction, _script.parts[useScriptCount])) continue;
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

        if (BlockStone(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (BlockCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (SelectStoneBoard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (SelectCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (MoveStone(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (MoveCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (Stack(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (WinnerPoint(_controller, _gameManager, runScript.actions[useScriptCount])) return;

        if (runScript.actions.Count > useScriptCount) return;
        runScript = null;
        useScriptCount = 0;
        targetPlayer.Clear();
        targetCard.Clear();
        targetStonePos.Clear();
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
        if (_script.type != ScriptType.SelectStoneBoard) return false;

        var script = (SelectStoneBoardAction)_script;

        string message = "";

        if (errorMessageDrawCount < 0)
        {
            string tmp = script.minCount > targetStonePos.Count ?
                $"残り:{script.minCount - targetStonePos.Count}" :
                "選択済み";

            message = script.minCount != script.maxCount ?
                $"石を置く場所を{script.minCount}から{script.maxCount}選択してください。\n{tmp}" :
                 $"石を置く場所を{script.minCount}選択してください。\n{tmp}";

        }
        else{

            errorMessageDrawCount--;
            message = "これ以上選択できません";
        }

            _gameManager.SetMessate(message);

        if (!_controller.isAction) return true;

        useScriptCount++;

        return true;
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

        return new BlockStoneAction();
    }

    ScriptAction CreateBlockCardAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.BlockCard) return null;

        return new BlockCardAction();
    }

    ScriptAction CreateSelectStoneBoardAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.SelectStoneBoard) return null;

        var res = new SelectStoneBoardAction();

        var args = GenerateArgment(_script.argments);

        for(int i = 0; i < args.Count;i++)
        {
            if (args[i] == "--max" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.maxCount))
                    i += 1;

            if (args[i] == "--min" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.minCount))
                    i += 1;
        }

        return res;

    }

    ScriptAction CreateSelectCardAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.SelectCard) return null;

        var res = new SelectCardAction();

        var args = GenerateArgment(_script.argments);

        for (int i = 0; i < args.Count; i++)
        {
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
        if (_script.type != ScriptType.SelectStoneBoard) return null;

        var res = new MoveStoneAction();

        var args = GenerateArgment(_script.argments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i] == "--remove")
                res.removeFlg = true;

            if (args[i] == "--put")
                res.removeFlg = false;
        }

        return res;

    }

    ScriptAction CreateMoveCardAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.SelectStoneBoard) return null;

        var res = new MoveCardAction();

        var args = GenerateArgment(_script.argments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i] == "--book")
                res.moveZone = ZoneType.Book;

            if (args[i] == "--magic-zone")
                res.moveZone = ZoneType.MagicZone;

            if (args[i] == "--item-zone")
                res.moveZone = ZoneType.ItemZone;

            if (args[i] == "--trash-zone")
                res.moveZone = ZoneType.TrashZone;
        }

        return res;
    }

    ScriptAction CreateWinnerPointAction(ScriptParts _script)
    {
        if (_script.type != ScriptType.SelectStoneBoard) return null;

        var res = new WinnerPointAction();

        var args = GenerateArgment(_script.argments);

        for (int i = 0; i < args.Count; i++)
        {
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
                tmp += arg + (tmp.Length > 0 ? " " : "");

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
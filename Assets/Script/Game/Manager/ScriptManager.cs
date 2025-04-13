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
       functions[(int)ScriptType.BlockStone] = new BlockStoneFunction(this);
       functions[(int)ScriptType.BlockCard] = new BlockCardFunction(this);
       functions[(int)ScriptType.SelectStoneBoard] = new SelectStoneBoardFunction(this);
       functions[(int)ScriptType.SelectCard] = new SelectCardFunction(this);
       functions[(int)ScriptType.MoveStone] = new MoveStoneFunction(this);
       functions[(int)ScriptType.MoveCard] = new MoveCardFunction(this);
       functions[(int)ScriptType.Stack] = new StackFunction(this);
       functions[(int)ScriptType.Stay] = new StayFunction(this);
       functions[(int)ScriptType.WinnerPoint] = new WinnerPointFunction(this);
       functions[(int)ScriptType.Skip] = new BlockStoneFunction(this);
    }


    delegate ScriptArgument CreateMethod(ScriptParts _parts);

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
        Stay,//永続効果の登録
        WinnerPoint,//ゲームに勝利するためのポイントを増減させる//
        Skip,//Stackカードのスキップを行う
    }


    ScriptFunctionBase[] functions = new ScriptFunctionBase[10];

    public enum ArgumentType : int
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

    public class ScriptArgument
    {
        public ScriptType type;
    }

    public abstract class ScriptFunctionBase
    {

        public ScriptFunctionBase(ScriptManager _manager)
        {
            if (_manager == null) return;
            mgr = _manager;
        }

        public virtual void Release() { }

        public abstract ScriptArgument GenerateArgument(ScriptParts _script);

        public abstract bool Run(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script);


        protected void SetSelectStoneBoardFunctionController(SelectStoneBoardFunctionController _controller)
        {
            if (_controller == null) return;
            mgr.selectStoneBoardFunctionController = _controller;
        }

        protected void SetSelectCardFunctionController(SelectCardFunctionController _controller)
        {
            if (_controller == null) return;
            mgr.selectCardFunctionController = _controller;
        }

        protected Dictionary<int, StonePosScript> GetTargetStonePos()
        {
            return mgr.selectStoneBoardFunctionController.GetTargetStonePos();
        }

        protected List<CardScript> GetTargetCard()
        {
            return mgr.selectCardFunctionController.GetTargetCard();
        }

        protected List<string> GenerateArgument(string _args)
        {
            return mgr.GenerateArgument(_args);
        }

        protected void AddUseScriptCount()
        {
            mgr.AddUseScriptCount();
        }

        protected ScriptManager manager { get { return mgr; } }

        ScriptManager mgr = null;
    }
    
    abstract public class SelectScriptControllerBase
    {
        abstract public bool SelectAction(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script);

        abstract public void ClearTarget();
    }

    public class BlockStoneArgument : ScriptArgument
    {
        public BlockStoneArgument()
        {
            type = ScriptType.BlockStone;
        }
    }

    public class BlockCardArgument : ScriptArgument
    {
        public BlockCardArgument()
        {
            type = ScriptType.BlockCard;
        }
    }

    public class SelectStoneBoardArgument : ScriptArgument
    {
        public SelectStoneBoardArgument()
        {
            type = ScriptType.SelectStoneBoard;
        }

        public int minCount = 0;
        public int maxCount = 0;
        public bool isPutPos = true;
    }

    public class SelectCardArgument : ScriptArgument
    {
        public SelectCardArgument()
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

    public class MoveStoneArgument : ScriptArgument
    {
        public MoveStoneArgument()
        {
            type = ScriptType.MoveStone;
        }

        public bool removeFlg = false;
    }

    public class MoveCardArgument : ScriptArgument
    {
        public MoveCardArgument()
        {
            type = ScriptType.MoveCard;
        }

        public bool openFlg = false;
        public ZoneType moveZone = 0;
    }

    public class WinnerPointArgument : ScriptArgument
    {
        public WinnerPointArgument()
        {
            type = ScriptType.WinnerPoint;
        }

        public bool downFlg = false;
        public int point = 0;
    }

    public class ScriptArgumentData
    {
        public ArgumentType type = ArgumentType.Entry;
        public List<ScriptArgument> actions = new List<ScriptArgument>();
    }

    ScriptArgumentData runScript = null;
    public bool isRunScript { get { return runScript != null; } }

    public ScriptType RunScriptType { get { return runScript.actions[useScriptCount].type; } }


    List<ScriptArgumentData> stayScript = new List<ScriptArgumentData>();

    SelectStoneBoardFunctionController selectStoneBoardFunctionController = null;

    SelectCardFunctionController selectCardFunctionController = null;

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

    public void SelectTargetPos(int _x,int _y,GameManager _manager)
    {
        if (runScript == null) return;
        if (runScript.actions[useScriptCount].type != ScriptType.SelectStoneBoard) return;

        var action = (SelectStoneBoardArgument)runScript.actions[useScriptCount];

        selectStoneBoardFunctionController.SelectTargetPos(_x, _y, _manager, action);
    }

    public void SelectCard(CardScript _script, GameManager _manager)
    {
        if (_script == null) return;
        if (runScript == null) return;
        if (runScript.actions[useScriptCount].type != ScriptType.SelectCard) return;

        var action = (SelectCardArgument)runScript.actions[useScriptCount];

        selectCardFunctionController.SelectCard(_script, _manager, action);
    }

    public ScriptArgumentData CreateScript(ScriptData _script, bool _regist = false)
    {

        var res = new ScriptArgumentData();
        if (_script.parts == null) return res;
        if (_script.parts.Length <= 0) return res;

        res.type = _script.type;

        for(int i = 0;i<_script.parts.Length;i++)
        {
            var scr = _script.parts[i];

            int scriptType = (int)scr.type;

            var arg = functions[scriptType].GenerateArgument(_script.parts[i]);

            if (arg == null) continue;

            res.actions.Add(arg);

            /*
            if (CreateScriptArgument(res, CreateBlockStoneArgument, scr)) continue;
            if (CreateScriptArgument(res, CreateBlockCardArgument, scr)) continue;
            if (CreateScriptArgument(res, CreateSelectStoneBoardArgument, scr)) continue;
            if (CreateScriptArgument(res, CreateSelectCardArgument, scr)) continue;
            if (CreateScriptArgument(res, CreateMoveStoneArgument, scr)) continue;
            if (CreateScriptArgument(res, CreateMoveCardArgument, scr)) continue;
            if (CreateScriptArgument(res, CreateStackArgument, scr)) continue;
            if (CreateScriptArgument(res, CreateWinnerPointArgument, scr)) continue;
            */
        }

        if (res.type == ArgumentType.Stay)
        {
            stayScript.Add(res);
            return null;
        }

        if (_regist)
            runScript = res;

        return res;
    }

    public void SetRunScript(ScriptArgumentData _data)
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

        var argument = runScript.actions[useScriptCount];
        int scriptType = (int)argument.type;


        functions[scriptType].Run(_controller,_gameManager, argument);
        /*
        if (BlockStone(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (BlockCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (SelectStoneBoard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (SelectCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (MoveStone(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (MoveCard(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (Stack(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        if (WinnerPoint(_controller, _gameManager, runScript.actions[useScriptCount])) return;
        */


        if (runScript.actions.Count > useScriptCount) return;

        useScriptCount = 0;
        runScript = null;
        foreach (var func in functions)
        {
            func.Release();
        }



    }

    bool BlockStone(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.BlockStone) return false;

        AddUseScriptCount();

        return true;
    }


    bool BlockCard(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.BlockCard) return false;

        AddUseScriptCount();

        return true;
    }


    bool SelectStoneBoard(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        return selectStoneBoardFunctionController.SelectAction(_controller, _gameManager, _script);
    }


    bool SelectCard(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        return selectCardFunctionController.SelectAction(_controller, _gameManager, _script);
    }

    bool MoveStone(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.MoveStone) return false;

        var targetStonePos = selectStoneBoardFunctionController.GetTargetStonePos();

        if (targetStonePos.Count <= 0) return true;

        var act = (MoveStoneArgument)_script;

        foreach (var pos in targetStonePos)
        {
            var sec = pos.Value;

            sec.UnSelectStonePos();
            if (!act.removeFlg) sec.PutStone(_gameManager.GetNowPlayer().stoneModel);
            else sec.RemovePutStone();
        }

        AddUseScriptCount();

        return true;
    }

    bool MoveCard(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.MoveCard) return false;

        var targetCards = selectCardFunctionController.GetTargetCard();

        if (targetCards.Count <= 0)
        {
            AddUseScriptCount();
            return true;
        }

        var act = (MoveCardArgument)_script;

        for(int i = 0;i< targetCards.Count;i++)
        {
            if (act.moveZone == ZoneType.MagicZone && targetCards[i].type == CardData.CardType.Item) continue;
            if (act.moveZone == ZoneType.ItemZone && targetCards[i].type == CardData.CardType.Magic) continue;

            
        }

        AddUseScriptCount();

        return true;
    }

    bool Stack(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.Stack) return false;

        var targetCards = selectCardFunctionController.GetTargetCard();

        if (targetCards.Count <= 0 || targetCards.Count >= 2)
        {
            AddUseScriptCount();
            return true;
        }

        var card = targetCards[0];

        return true;
    }

    bool WinnerPoint(ControllerBase _controller, GameManager _gameManager, ScriptArgument _script)
    {
        if (_script.type != ScriptType.WinnerPoint) return false;

        AddUseScriptCount();

        return true;
    }


    bool CreateScriptArgument(ScriptArgumentData _data, CreateMethod _method,ScriptParts _parts)
    {
        if (_method == null) return false;
        if (_data == null) return false;

        var act = _method(_parts);

        if (act == null) return false;

        _data.actions.Add(act);

        return true;
    }

    ScriptArgument CreateBlockStoneArgument(ScriptParts _script)
    {
        if (_script.type != ScriptType.BlockStone) return null;

        var res = new BlockStoneArgument();

        res.type = ScriptType.BlockStone;

        return res;
    }

    ScriptArgument CreateBlockCardArgument(ScriptParts _script)
    {
        if (_script.type != ScriptType.BlockCard) return null;

        var res = new BlockCardArgument();

        res.type = ScriptType.BlockCard;

        return res;
    }

    ScriptArgument CreateSelectStoneBoardArgument(ScriptParts _script)
    {
        if (_script.type != ScriptType.SelectStoneBoard) return null;

        var res = new SelectStoneBoardArgument();

        res.type = ScriptType.SelectStoneBoard;

        var args = GenerateArgument(_script.arguments);

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

    ScriptArgument CreateSelectCardArgument(ScriptParts _script)
    {
        if (_script.type != ScriptType.SelectCard) return null;

        var res = new SelectCardArgument();

        res.type = ScriptType.SelectCard;

        var args = GenerateArgument(_script.arguments);

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


            if (args[i] == "--zone-type-book")
                res.zoneType |= ZoneType.Book;

            if (args[i] == "--zone-type-magic-zone")
                res.zoneType |= ZoneType.MagicZone;

            if (args[i] == "--zone-type-item-zone")
                res.zoneType |= ZoneType.ItemZone;

            if (args[i] == "--zone-type-trash-zone")
                res.zoneType |= ZoneType.TrashZone;

            if (args[i] == "--card-type-magic")
                res.cardType |= SelectCardType.Magic;

            if (args[i] == "--card-type-item")
                res.cardType |= SelectCardType.Item;

            if (args[i] == "--card-type-trap")
                res.cardType |= SelectCardType.Trap;


            if (args[i] == "--card-type" && args.Count > i + 1)
            {
                var type = 0;
                if (int.TryParse(args[i + 1], out type))
                {
                    res.cardType |= (SelectCardType)type;
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

            if (args[i] == "--abnormal-playing")
            {
                res.normalPlaying = false;
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

    ScriptArgument CreateMoveStoneArgument(ScriptParts _script)
    {
        if (_script.type != ScriptType.MoveStone) return null;

        var res = new MoveStoneArgument();

        res.type = ScriptType.MoveStone;

        var args = GenerateArgument(_script.arguments);

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

    ScriptArgument CreateMoveCardArgument(ScriptParts _script)
    {
        if (_script.type != ScriptType.MoveCard) return null;

        var res = new MoveCardArgument();

        res.type = ScriptType.MoveCard;

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--book")
                res.moveZone = ZoneType.Book;

            if (args[i] == "--magic-zone")
                res.moveZone = ZoneType.MagicZone;

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

    ScriptArgument CreateStackArgument(ScriptParts _script)
    {
        if (_script.type != ScriptType.WinnerPoint) return null;

        var res = new ScriptArgument();

        res.type = ScriptType.Stack;

        return res;
    }

    ScriptArgument CreateWinnerPointArgument(ScriptParts _script)
    {
        if (_script.type != ScriptType.WinnerPoint) return null;

        var res = new WinnerPointArgument();

        res.type = ScriptType.WinnerPoint;

        var args = GenerateArgument(_script.arguments);

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

    List<string> GenerateArgument(string _args)
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
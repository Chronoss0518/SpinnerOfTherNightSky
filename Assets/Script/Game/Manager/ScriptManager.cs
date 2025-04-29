using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Accessibility;

[System.Serializable]
public class ScriptManager
{
    public ScriptManager()
    {
       functions[(int)ScriptType.BlockStone] = new BlockStoneFunction(this);
       functions[(int)ScriptType.BlockCard] = new BlockCardFunction(this);
       functions[(int)ScriptType.SelectStoneBoard] = new SelectStoneBoardFunction(this);
       functions[(int)ScriptType.SelectCard] = new SelectCardFunction(this);
       functions[(int)ScriptType.SelectItemZone] = new SelectItemZoneFunction(this);
       functions[(int)ScriptType.MoveStone] = new MoveStoneFunction(this);
       functions[(int)ScriptType.MoveCard] = new MoveCardFunction(this);
       functions[(int)ScriptType.OpenItemZoneCard] = new MoveCardFunction(this);
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
        SelectItemZone,//魔導書からカードを選択する//
        MoveStone,//石の置く・石を取り除く//
        MoveCard,//カードを移動させる//
        OpenItemZoneCard,//カードを移動させる//
        Stack,//カードをStackする→カードの発動準備//
        Stay,//永続効果の登録
        WinnerPoint,//ゲームに勝利するためのポイントを増減させる//
        Skip,//Stackカードのスキップを行う
        None,
    }


    ScriptFunctionBase[] functions = new ScriptFunctionBase[(int)ScriptType.None];

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

    [System.Serializable]
    public class ScriptArgument
    {
        public ScriptType type;
    }

    [System.Serializable]
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

        protected void SetSelectItemZoneFunctionController(SelectItemZoneFunctionController _controller)
        {
            if (_controller == null) return;
            mgr.selectItemZoneFunctionController = _controller;
        }

        protected Dictionary<int, StonePosScript> GetTargetStonePos()
        {
            return mgr.selectStoneBoardFunctionController.GetTargetStonePos();
        }

        protected List<CardScript> GetTargetCard()
        {
            return mgr.selectCardFunctionController.GetTargetCard();
        }

        protected ItemZoneObject GetItemZonePos()
        {
            return mgr.selectItemZoneFunctionController.GetTargetPos();
        }

        protected List<string> GenerateArgument(string _args)
        {
            return mgr.GenerateArgument(_args);
        }

        protected void AddUseScriptCount()
        {
            mgr.AddUseScriptCount();
        }

        protected void Stack(CardScript _script,GameManager _manager)
        {
            mgr.Stack(_script, _manager);


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

    public class SelectItemZoneArgument : ScriptArgument
    {
        public SelectItemZoneArgument()
        {
            type = ScriptType.SelectItemZone;
        }

        //1:自身,2:自身以外;
        public int selectTarget = 0;
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
        public bool targetUsePlayer = true;
        public ZoneType moveZone = 0;
    }

    public class OpenItemZoneCardArgument : ScriptArgument
    {
        public OpenItemZoneCardArgument()
        {
            type = ScriptType.MoveCard;
        }
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

    [SerializeField,ReadOnly]
    SelectStoneBoardFunctionController selectStoneBoardFunctionController = null;

    public SelectStoneBoardFunctionController selectStoneBoardController { get { return selectStoneBoardFunctionController; } }

    [SerializeField, ReadOnly]
    SelectCardFunctionController selectCardFunctionController = null;

    public SelectCardFunctionController selectCardController { get { return selectCardFunctionController; } }

    [SerializeField, ReadOnly]
    SelectItemZoneFunctionController selectItemZoneFunctionController = null;

    public SelectItemZoneFunctionController selectItemZoneController { get { return selectItemZoneFunctionController; } }

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

    public void SelectTargetPos(int _x, int _y, GameManager _manager)
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

    public bool SelectTargetItemZonePos(ItemZoneObject _pos, GameManager _manager)
    {
        if (runScript == null) return false;
        if (runScript.actions[useScriptCount].type != ScriptType.SelectItemZone) return false;

        var action = (SelectItemZoneArgument)runScript.actions[useScriptCount];

        return selectItemZoneFunctionController.SelectPos(_pos, _manager, action);
    }

    void Stack(CardScript _script, GameManager _manager)
    {
        _manager.AddStackCard(_script);
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

        if (runScript.actions.Count > useScriptCount) return;

        useScriptCount = 0;
        runScript = null;
        foreach (var func in functions)
        {
            func.Release();
        }
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptManager
{
    public enum ScriptType : int
    {
        PutStone,//石を置く//
        RemoveStone,//石を取り除く//
        SelectPlayer,//Playerを選択する//
        SelectCardBook,//魔導書からカードを選択する//
        SelectCardItem,//選択したPlayerのItemZoneのカードを選択する//
        SelectCardTrash,//選択したPlayerのTrashZoneのカードを選択する//
        SelectCardMagic,//選択したPlayerのMagicZoneのカードを選択する//
        MoveSetItem,//ItemZoneへカードを伏せて出す//
        MoveOpenItem,//ItemZoneへカードを公開して出す//
        MoveMagic,//MagicZoneへカードを出す//
        MoveBook,//魔導書へカードを戻す//
        MoveTrash,//TrashZoneへカードを出す//
        Stack,//カードをStackする→カードの発動準備//
        UpWinnerPoint,//ゲームに勝利するためのポイントが上がる
    }

    public enum ActionType : int
    {
        Entry,
        Stay,
        Exit,
    }


    public bool isRunScript { get; private set; } = false;

    GameManager gameManager { get; set; } = null;

    public void SetGameManager(GameManager _gameManager) {  gameManager = _gameManager; }

    Player targetPlayer { get; set; } = null;

    CardScript targetCard { get; set; } = null;

    int useScriptCount = 0;

    Dictionary<ScriptType,CardScript> stayActionCardScripts = new Dictionary<ScriptType, CardScript>();

#if UNITY_EDITOR

    [SerializeField]
    Player testTargetPlayer = null;

    [SerializeField]
    CardScript testTargetCard = null;

    void Update()
    {
        testTargetPlayer = targetPlayer;
        testTargetCard = targetCard;
    }

#endif

    public void IsStayScriptTest()
    {

    }


    public void RunScript(Player _player, ScriptData _script)
    {
        PutStone(_player, _script.parts[useScriptCount]);
        RemoveStone(_player, _script.parts[useScriptCount]);




    }


    void PutStone(Player _player, ScriptParts _script)
    {
        if (_script.type != ScriptType.PutStone) return;

        var args = GenerateArgment(_script.argments);

        SetArgments(args,new Dictionary<string, Action<string>>(
            new List<KeyValuePair<string, Action<string>>>{
                new KeyValuePair<string, Action<string>>("-c",(string _arg)=>{
                    
                }),
            })
        );

    }

    void RemoveStone(Player _player, ScriptParts _script)
    {
        if (_script.type != ScriptType.RemoveStone) return;

        var args = GenerateArgment(_script.argments);

    }


    void SelectCardFromMyBook(Player _player, ScriptParts _script)
    {

    }

    void SelectCardFromMyMagic(Player _player, ScriptParts _script)
    {

    }

    void SelectCardFromMyTrash(Player _player, ScriptParts _script)
    {

    }

    void SelectCardFromMyItem(Player _player, ScriptParts _script)
    {

    }

    void SelectCardFromOtherMagic(Player _player, ScriptParts _script)
    {

    }

    void SelectCardFromOtherTrash(Player _player, ScriptParts    _script)
    {

    }

    void SelectCardFromOtherItem(Player _player, ScriptParts _script)
    {

    }

    void SelectStone(Player _player, ScriptParts _script)
    {

    }


    List<string> GenerateArgment(string _args)
    {
        var args = _args.Split(" ");

        if (args.Length <= 0) return null;

        var res = new List<string>();

        bool strFlg = false;
        string tmp = "";

        foreach(var arg in args)
        {
            if(arg.IndexOf("\"") <= 0)
                strFlg = true;

            if (strFlg)
                tmp += arg + (tmp.Length > 0 ? " " : "");

            if (arg.IndexOf("\"") >= arg.Length - 1)
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


    void SetArgments(List<string> _args,Dictionary<string,Action<string>> _act)
    {
        for(int i = 0;i < _args.Count;i++)
        {
            if (!_act.ContainsKey(_args[i])) continue;

            _act[_args[i]](i >= _args.Count - 1 ? "" : _args[++i]);
        }
    }

}
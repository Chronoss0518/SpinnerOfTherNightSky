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
        SelectCardItemZone,//選択したPlayerのItemZoneの場所を選択する//
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


    public void RunScript(Player _player, ScriptParts _script)
    {
        PutStone(_player, _script);
        RemoveStone(_player, _script);
    }


    void PutStone(Player _player, ScriptParts _script)
    {
        if (_script.type != ScriptType.PutStone) return;


    }

    void RemoveStone(Player _player, ScriptParts _script)
    {
        if (_script.type != ScriptType.RemoveStone) return;


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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptManager
{
    [System.Flags]
    public enum ScriptType : int
    {
        PutStone = 1,//石を置く//
        RemoveStone = 2,//石を取り除く//
        SelectPlayer = 4,//Playerを選択する//
        SelectCardBook = 8,//魔導書からカードを選択する//
        SelectCardItem = 16,//選択したPlayerのItemZoneのカードを選択する//
        SelectCardItemZone = 32,//選択したPlayerのItemZoneの場所を選択する//
        SelectCardTrash = 64,//選択したPlayerのTrashZoneのカードを選択する//
        SelectCardMagic = 128,//選択したPlayerのMagicZoneのカードを選択する//
        MoveSetItem = 256,//ItemZoneへカードを伏せて出す//
        MoveOpenItem = 512,//ItemZoneへカードを公開して出す//
        MoveMagic = 1024,//MagicZoneへカードを出す//
        MoveBook = 2048,//魔導書へカードを戻す//
        MoveTrash = 4096,//TrashZoneへカードを出す//
        Stack = 8192,//カードをStackする→カードの発動準備//
    }

    [System.Serializable]
    public class ScriptData
    {
        public ScriptType type = ScriptType.PutStone;
        public byte argment = 0;
    }

    [System.Serializable]
    public class ScriptList
    {
        public List<ScriptData>scriptList = new List<ScriptData>();
    }

    public bool isRunScript { get; private set; } = false;

    GameManager gameManager { get; set; } = null;

    public void SetGameManager(GameManager _gameManager) {  gameManager = _gameManager; }

    Player targetPlayer { get; set; } = null;

    CardScript targetCard { get; set; } = null;


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

    public void RunScript(Player _player, ScriptList _scriptList)
    {

    }

    public void RunScript(Player _player, ScriptData _script)
    {
        PutStone(_player, _script);
        RemoveStone(_player, _script);
    }


    void PutStone(Player _player, ScriptData _script)
    {
        if (_script.type != ScriptType.PutStone) return;


    }

    void RemoveStone(Player _player, ScriptData _script)
    {
        if (_script.type != ScriptType.RemoveStone) return;


    }


    void SelectCardFromMyBook(Player _player, ScriptData _script)
    {

    }

    void SelectCardFromMyMagic(Player _player, ScriptData _script)
    {

    }

    void SelectCardFromMyTrash(Player _player, ScriptData _script)
    {

    }

    void SelectCardFromMyItem(Player _player, ScriptData _script)
    {

    }

    void SelectCardFromOtherMagic(Player _player, ScriptData _script)
    {

    }

    void SelectCardFromOtherTrash(Player _player, ScriptData _script)
    {

    }

    void SelectCardFromOtherItem(Player _player, ScriptData _script)
    {

    }

    void SelectStone(Player _player, ScriptData _script)
    {

    }
}
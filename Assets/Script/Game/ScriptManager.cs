using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptManager
{
    [System.Flags]
    public enum ScriptType : int
    {
        PutStone = 1,//�΂�u��//
        RemoveStone = 2,//�΂���菜��//
        SelectPlayer = 4,//Player��I������//
        SelectCardBook = 8,//����������J�[�h��I������//
        SelectCardItem = 16,//�I������Player��ItemZone�̃J�[�h��I������//
        SelectCardItemZone = 32,//�I������Player��ItemZone�̏ꏊ��I������//
        SelectCardTrash = 64,//�I������Player��TrashZone�̃J�[�h��I������//
        SelectCardMagic = 128,//�I������Player��MagicZone�̃J�[�h��I������//
        MoveSetItem = 256,//ItemZone�փJ�[�h�𕚂��ďo��//
        MoveOpenItem = 512,//ItemZone�փJ�[�h�����J���ďo��//
        MoveMagic = 1024,//MagicZone�փJ�[�h���o��//
        MoveBook = 2048,//�������փJ�[�h��߂�//
        MoveTrash = 4096,//TrashZone�փJ�[�h���o��//
        Stack = 8192,//�J�[�h��Stack���遨�J�[�h�̔�������//
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
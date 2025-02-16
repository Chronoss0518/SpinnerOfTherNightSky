using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptManager
{
    public enum ScriptType : int
    {
        PutStone,//�΂�u��//
        RemoveStone,//�΂���菜��//
        SelectPlayer,//Player��I������//
        SelectCardBook,//����������J�[�h��I������//
        SelectCardItem,//�I������Player��ItemZone�̃J�[�h��I������//
        SelectCardItemZone,//�I������Player��ItemZone�̏ꏊ��I������//
        SelectCardTrash,//�I������Player��TrashZone�̃J�[�h��I������//
        SelectCardMagic,//�I������Player��MagicZone�̃J�[�h��I������//
        MoveSetItem,//ItemZone�փJ�[�h�𕚂��ďo��//
        MoveOpenItem,//ItemZone�փJ�[�h�����J���ďo��//
        MoveMagic,//MagicZone�փJ�[�h���o��//
        MoveBook,//�������փJ�[�h��߂�//
        MoveTrash,//TrashZone�փJ�[�h���o��//
        Stack,//�J�[�h��Stack���遨�J�[�h�̔�������//
        UpWinnerPoint,//�Q�[���ɏ������邽�߂̃|�C���g���オ��
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
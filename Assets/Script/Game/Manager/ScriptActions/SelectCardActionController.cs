using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using static ScriptManager;

public class SelectCardActionController : SelectScriptActionBase
{
    public const string BOOK_ZONE = "������";
    public const string ITEM_ZONE = "�g���b�v�]�[��";
    public const string MAGIC_ZONE = "�}�W�b�N�]�[��";
    public const string TRASH_ZONE = "�g���b�V���]�[��";

    public const string YOUR_PLAYER = "���g";
    public const string OTHER_PLAYER = "�����p�t";
    public const string ALL_PLAYER = "�S���p�t";

    [SerializeField, ReadOnly]
    ScriptManager manager = null;

    [SerializeField, ReadOnly]
    List<CardScript> targetCard = new List<CardScript>();

    public int TargetCardCount { get { return targetCard.Count; } }

    [SerializeField, ReadOnly]
    List<CardScript> targetCards = new List<CardScript>();


    [SerializeField, ReadOnly]
    List<string> targetZoneList = new List<string>();

    [SerializeField, ReadOnly]
    string targetPlayer = "";

    [SerializeField,ReadOnly]
    string targetZoneName = "";


    public List<CardScript> GetTargetCard()
    {
        return targetCards;
    }

    public override void ClearTarget()
    {
        targetCards.Clear();
    }

    public void Init(ScriptManager _manager)
    {
        manager = _manager;
    }

    public void SelectCard(CardScript _card, GameManager _manager, SelectCardAction _runAction)
    {
        if (_runAction == null) return;
        if (_card == null) return;
        if(!_card.isSelectTarget)
        {
            manager.SetError(ErrorType.IsNotTargetZone);
            return;
        }

        for(int i = 0;i < targetCards.Count;i++)
        {
            if (!_card.Equals(targetCards[i])) continue;
            targetCards.RemoveAt(i);
            _card.SetSelectFlg(false);
            return;
        }


        if (_runAction.selectMaxCount <= targetCards.Count)
        {
            manager.SetError(ErrorType.IsRangeMaxOverCount);
            return;
        }


        targetCards.Add(_card);
        _card.SetSelectFlg(true);
    }


    public override bool SelectAction(ControllerBase _controller, GameManager _gameManager, ScriptAction _script)
    {
        if (_script.type != ScriptType.SelectCard) return false;


        var act = (SelectCardAction)_script;

        if (targetZoneList.Count <= 0)
        {
            _controller.ActionStart();
            _gameManager.StartSelectCard(act);

            if ((act.zoneType | ZoneType.Book) > 0) targetZoneList.Add(BOOK_ZONE);
            if ((act.zoneType | ZoneType.MagicZone) > 0) targetZoneList.Add(MAGIC_ZONE);
            if ((act.zoneType | ZoneType.ItemZone) > 0) targetZoneList.Add(ITEM_ZONE);
            if ((act.zoneType | ZoneType.TrashZone) > 0) targetZoneList.Add(TRASH_ZONE);

            targetPlayer = act.playerType == 0 ? YOUR_PLAYER + "��" :
                act.playerType == 1 ? OTHER_PLAYER + "��" :
                ALL_PLAYER + "����";

            targetZoneName = targetZoneList[0];
            for (int i = 1;i< targetZoneList.Count;i++)
            {
                targetZoneName += "��" + targetZoneList[i];
            }

            targetZoneName = targetPlayer + targetZoneName;
        }

        string message = "";

        if (manager.GetErrorType() == ErrorType.None)
        {
            string tmp = act.selectMinCount > targetCards.Count ?
                $"�c��:{act.selectMinCount - targetCards.Count}" :
                "�I���ς�";

            message = act.selectMinCount != act.selectMaxCount ?
                $"{targetZoneName}�̃J�[�h��{act.selectMinCount}����{act.selectMaxCount}�I�����Ă��������B\n{tmp}" :
                 $"{targetZoneName}�̃J�[�h��{act.selectMinCount}�I�����Ă��������B\n{tmp}";
        }
        else
        {

            manager.DownErrorMessageDrawCount();
            if (manager.GetErrorType() == ErrorType.IsRangeMaxOverCount) message = "����ȏ�I���ł��܂���";
            if (manager.GetErrorType() == ErrorType.IsRangeMinOverCount) message = $"{act.selectMinCount}�ȏ�I�����Ă�������";
            if (manager.GetErrorType() == ErrorType.IsNotTargetZone) message = $"�I�������ꏊ�̃J�[�h��I�Ԃ��Ƃ͂ł��܂���";
            if (manager.GetErrorType() == ErrorType.IsNotRemoveStones) message = $"���̖��@���g���ɂ͎�菜���΂�����܂���";
            if (manager.GetErrorType() == ErrorType.IsNotPutItemZone) message = $"�����u�����߂̃X�y�[�X������܂���";
        }

        _gameManager.SetMessate(message);

        if (!_controller.isAction) return true;
        if (act.selectMinCount > targetCards.Count)
        {
            manager.SetError(ErrorType.IsRangeMinOverCount);
            _controller.DownActionFlg();
            return true;
        }

        _controller.ActionEnd();
        manager.AddUseScriptCount();
        _gameManager.EndSelectCardTest();

        targetZoneName = "";
        targetZoneList.Clear();
        targetPlayer = "";

        return true;
    }

}

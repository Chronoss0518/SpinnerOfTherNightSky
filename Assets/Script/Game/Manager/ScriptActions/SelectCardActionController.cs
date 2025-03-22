using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using static ScriptManager;

public class SelectCardActionController : SelectScriptActionBase
{
    public const string BOOK_ZONE = "魔導書";
    public const string ITEM_ZONE = "トラップゾーン";
    public const string MAGIC_ZONE = "マジックゾーン";
    public const string TRASH_ZONE = "トラッシュゾーン";

    public const string YOUR_PLAYER = "自身";
    public const string OTHER_PLAYER = "他魔術師";
    public const string ALL_PLAYER = "全魔術師";

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

            targetPlayer = act.playerType == 0 ? YOUR_PLAYER + "の" :
                act.playerType == 1 ? OTHER_PLAYER + "の" :
                ALL_PLAYER + "から";

            targetZoneName = targetZoneList[0];
            for (int i = 1;i< targetZoneList.Count;i++)
            {
                targetZoneName += "と" + targetZoneList[i];
            }

            targetZoneName = targetPlayer + targetZoneName;
        }

        string message = "";

        if (manager.GetErrorType() == ErrorType.None)
        {
            string tmp = act.selectMinCount > targetCards.Count ?
                $"残り:{act.selectMinCount - targetCards.Count}" :
                "選択済み";

            message = act.selectMinCount != act.selectMaxCount ?
                $"{targetZoneName}のカードを{act.selectMinCount}から{act.selectMaxCount}選択してください。\n{tmp}" :
                 $"{targetZoneName}のカードを{act.selectMinCount}選択してください。\n{tmp}";
        }
        else
        {

            manager.DownErrorMessageDrawCount();
            if (manager.GetErrorType() == ErrorType.IsRangeMaxOverCount) message = "これ以上選択できません";
            if (manager.GetErrorType() == ErrorType.IsRangeMinOverCount) message = $"{act.selectMinCount}以上選択してください";
            if (manager.GetErrorType() == ErrorType.IsNotTargetZone) message = $"選択した場所のカードを選ぶことはできません";
            if (manager.GetErrorType() == ErrorType.IsNotRemoveStones) message = $"その魔法を使うには取り除く石がありません";
            if (manager.GetErrorType() == ErrorType.IsNotPutItemZone) message = $"道具を置くためのスペースがありません";
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

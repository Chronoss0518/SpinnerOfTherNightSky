using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

public class ItemZoneManager : ZoneScriptBase
{
    public const int PUT_ITEM_COUNT = 3;

    const float PUT_POSITION = 1.5f;

    public ItemZoneManager()
    {
        zoneType = ScriptManager.ZoneType.ItemZone;
    }

    public override void Init(Player _player, GameManager _manager)
    {
        base.Init(_player, _manager);
        for (int i = 0; i<PUT_ITEM_COUNT; i++)
        {
            items[i].Init(this,i, manager);
        }
    }

    [SerializeField]
    private ItemZoneObject[] items = null;

    public int nowPutCount { get
        {
            int res = 0;

            for (int i = 0; i < PUT_ITEM_COUNT; i++)
            {
                if (!items[i].IsPutCard()) continue;
                res++;
            }

            return res;
        }
    }

    public int nowCanPutCount
    {
        get
        {
            int res = 0;

            for (int i = 0; i < PUT_ITEM_COUNT; i++)
            {
                if (items[i].IsPutCard()) continue;
                res++;
            }

            return res;
        }
    }

    override public void SelectTargetTest(ScriptManager.SelectCardArgument _action, Player _runPlayer)
    {
        for (int i = 0; i < PUT_ITEM_COUNT; i++)
        {
            if (!items[i].IsPutCard()) continue;
            items[i].itemCard.SetSelectTargetTest(_action, _runPlayer);
        }
    }

    override public void SelectTargetDown()
    {
        for (int i = 0; i < PUT_ITEM_COUNT; i++)
        {
            if (!items[i].IsPutCard()) continue;
            items[i].itemCard.SetSelectUnTarget();
        }
    }

    public void SelectPositionTargetUp()
    {
        for (int i = 0; i < PUT_ITEM_COUNT; i++)
        {
            items[i].SelectTargetTest();
        }
    }

    public void SelectPositionTargetDown()
    {
        for (int i = 0; i < PUT_ITEM_COUNT; i++)
        {
            items[i].SelectTargetEnd();
        }
    }

    public void PutCard(int _num, CardData _card,bool _openFlg = false)
    {
        if (_card == null) return;
        if (_card.cardType == (int)CardData.CardType.Magic) return;
        if (manager == null) return;
        if (manager.cardPrefab == null) return;
        if (!IsNumTest(_num)) return;
        items[_num].SetItemCard(_card, player, manager,this, _openFlg);
    }

    public void RemoveCard(int _num)
    {
        if (!IsNumTest(_num)) return;
        items[_num].RemoveCard();
    }

    override public void RemoveCard(CardData _card)
    {
        if (_card == null) return;

        for (int num = 0; num < PUT_ITEM_COUNT; num++)
        {
            if (!items[num].IsCardData(_card)) continue;
            items[num].RemoveCard();
            return;
        }
    }

    public void OpenCard(int _num)
    {
        if (!IsNumTest(_num)) return;
        if (items[_num] == null) return;
        items[_num].SetOpenFlg(true);
    }

    public void OpenCard(CardData _card)
    {
        if (_card == null) return;

        for (int num = 0; num < PUT_ITEM_COUNT; num++)
        {
            if (!items[num].IsCardData(_card)) continue;
            items[num].SetOpenFlg(true);
            return;
        }
    }

    private bool IsNumTest(int _num)
    {
        return (_num >= 0 && _num < PUT_ITEM_COUNT);
    }

}

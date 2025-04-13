using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ItemCardScript : CardScript.CardScriptBase
{

    [SerializeField,ReadOnly]
    protected ItemCardData.ItemType type = ItemCardData.ItemType.Normal;

    public void SetType(ItemCardData.ItemType _type) {  type = _type; }

    public bool IsTrap { get { return type == ItemCardData.ItemType.Trap; } }

    public override void Init(CardData _data)
    {
        var itemData = (ItemCardData)_data;
        SetType((ItemCardData.ItemType)itemData.itemType);
    }

    public override void SetSelectTargetTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
    {
        SetSelectItemTargetTest(_argument, _runPlayer);
        SetSelectTrapTargetTest(_argument, _runPlayer);
    }

    void SetSelectItemTargetTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
    {
        if (_argument.cardType != 0)
            if ((_argument.cardType & ScriptManager.SelectCardType.Item) <= 0) return;
        if (baseData.cardType != (int)CardData.CardType.Item) return;
        var item = (ItemCardData)baseData;
        if (item.itemType != (int)ItemCardData.ItemType.Normal) return;

        if (!SelectTargetArgumentTest(_argument, _runPlayer)) return;

        if (!IsPlayingUseItemTest(_argument, item) &&
            !IsPlayingSetItemTest(_argument, item)) return;

        SelectTargetTestSuccess();
    }

    void SetSelectTrapTargetTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
    {
        if (_argument.cardType != 0)
            if ((_argument.cardType & ScriptManager.SelectCardType.Trap) <= 0) return;
        if (baseData.cardType != (int)CardData.CardType.Item) return;
        var item = (ItemCardData)baseData;
        if (item.itemType != (int)ItemCardData.ItemType.Trap) return;

        if (!SelectTargetArgumentTest(_argument, _runPlayer)) return;

        if (!IsPlayingUseTrapTest(_argument, item) &&
            !IsPlayingSetItemTest(_argument, item)) return;

        SelectTargetTestSuccess();
    }

    bool IsPlayingUseItemTest(ScriptManager.SelectCardArgument _argument, ItemCardData _data)
    {
        if (!_argument.normalPlaying) return true;
        var val = (zoneType & ScriptManager.ZoneType.Book) |
            (zoneType & ScriptManager.ZoneType.ItemZone);

        if (val <= 0) return false;


        return true;
    }

    bool IsPlayingUseTrapTest(ScriptManager.SelectCardArgument _argument, ItemCardData _data)
    {
        if (!_argument.normalPlaying) return true;
        if (zoneType != ScriptManager.ZoneType.ItemZone) return false;




        return true;
    }


    bool IsPlayingSetItemTest(ScriptManager.SelectCardArgument _argument, ItemCardData _data)
    {
        return true;
    }

}

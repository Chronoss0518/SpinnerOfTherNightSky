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

    public override void SetSelectTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        Debug.Log("Start Item Select Target Test");
        SetSelectItemTargetTest(_action, _runPlayer);
        SetSelectTrapTargetTest(_action, _runPlayer);
    }

    void SetSelectItemTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if (_action.cardType != 0)
            if ((_action.cardType & ScriptManager.SelectCardType.Item) <= 0) return;
        Debug.Log("Pass Card Type Test");
        if (baseData.cardType != (int)CardData.CardType.Item) return;
        var item = (ItemCardData)baseData;
        if (item.itemType != (int)ItemCardData.ItemType.Normal) return;

        if (!SelectTargetArgmentTest(_action, _runPlayer)) return;

        Debug.Log("Pass Target Argment Test");
        if (!IsPlayingUseItemTest(_action, item) &&
            !IsPlayingSetItemTest(_action, item)) return;

        Debug.Log("Pass Is Plaing Test");
        SelectTargetTestSuccess();
    }

    void SetSelectTrapTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if (_action.cardType != 0)
            if ((_action.cardType & ScriptManager.SelectCardType.Trap) <= 0) return;
        if (baseData.cardType != (int)CardData.CardType.Item) return;
        var item = (ItemCardData)baseData;
        if (item.itemType != (int)ItemCardData.ItemType.Trap) return;

        if (!SelectTargetArgmentTest(_action, _runPlayer)) return;

        if (!IsPlayingUseTrapTest(_action, item) &&
            !IsPlayingSetItemTest(_action, item)) return;

        SelectTargetTestSuccess();
    }

    bool IsPlayingUseItemTest(ScriptManager.SelectCardAction _action, ItemCardData _data)
    {
        if (!_action.normalPlaying) return true;
        var val = (zoneType & ScriptManager.ZoneType.Book) |
            (zoneType & ScriptManager.ZoneType.ItemZone);

        Debug.Log($"Zone Type[{zoneType}]");
        if (val <= 0) return false;


        return true;
    }

    bool IsPlayingUseTrapTest(ScriptManager.SelectCardAction _action, ItemCardData _data)
    {
        if (!_action.normalPlaying) return true;
        if (zoneType != ScriptManager.ZoneType.ItemZone) return false;




        return true;
    }


    bool IsPlayingSetItemTest(ScriptManager.SelectCardAction _action, ItemCardData _data)
    {
        return true;
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MagicCardScript : CardScript.CardScriptBase
{

    [SerializeField,ReadOnly]
    private MagicCardData.CardAttribute attribute = MagicCardData.CardAttribute.Spring;

    public MagicCardData.CardAttribute attributeType { get { return attribute; } }

    [SerializeField, ReadOnly]
    private MagicCardData.CardAttributeMonth attributeMonth = 0;
    public MagicCardData.CardAttributeMonth month { get { return attributeMonth; } }

    public void SetAttribute(int _attribute) {
        if (_attribute < 0) return;
        if (_attribute >= 12) return;

        attributeMonth = (MagicCardData.CardAttributeMonth)_attribute;

        attribute = MagicCardData.CardAttribute.Winter;

        if (_attribute >= (int)(attribute) * 3) return;
        attribute = MagicCardData.CardAttribute.Autumn;

        if (_attribute >= (int)(attribute) * 3) return;
        attribute = MagicCardData.CardAttribute.Summer;

        if (_attribute >= (int)(attribute) * 3) return;
        attribute = MagicCardData.CardAttribute.Spring;
    }

    public int point { get; private set; } = 0;

    public void SetPoint(int _point) { point = _point; }

    public override void Init(CardData _data)
    {
        var magicData = (MagicCardData)_data;
        SetAttribute(magicData.month);
        SetPoint(magicData.point);
    }

    public override void SetSelectTargetTest(ScriptManager.SelectCardArgument _action, Player _runPlayer)
    {
        if (_action.cardType != 0)
            if ((_action.cardType & ScriptManager.SelectCardType.Magic) <= 0) return;
        if (baseData.cardType != (int)CardData.CardType.Magic) return;
        var magic = (MagicCardData)baseData;

        if (!SelectTargetArgumentTest(_action, _runPlayer)) return;

        if (!IsPlayingMagicTest(_action, magic)) return;

        SelectTargetTestSuccess();
    }

    bool IsPlayingMagicTest(ScriptManager.SelectCardArgument _action, MagicCardData _data)
    {
        if (!_action.normalPlaying) return true;

        if ((zoneType & ScriptManager.ZoneType.Book) <= 0) return false;

        if (_action.magicAttributeMonth.Count > 0)
        {
            int loopCount = 0;
            for (loopCount = 0; loopCount < _action.magicAttributeMonth.Count; loopCount++)
            {
                int month = _action.magicAttributeMonth[loopCount];
                if (_data.month == month) break;
            }

            if (loopCount >= _action.magicAttributeMonth.Count) return false;
        }


        return true;
    }

}

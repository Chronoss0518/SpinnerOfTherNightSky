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

    public override void SetSelectTargetTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
    {
        if (_argument.cardType != 0)
            if ((_argument.cardType & ScriptManager.SelectCardType.Magic) <= 0) return;
        if (baseData.cardType != (int)CardData.CardType.Magic) return;
        var magic = (MagicCardData)baseData;

        if (!SelectTargetArgumentTest(_argument, _runPlayer)) return;

        if (!IsPlayingMagicTest(_argument, magic)) return;

        SelectTargetTestSuccess();
    }

    bool IsPlayingMagicTest(ScriptManager.SelectCardArgument _argument, MagicCardData _data)
    {
        if (!_argument.normalPlaying) return true;

        if ((zoneType & ScriptManager.ZoneType.Book) <= 0) return false;

        if (_argument.magicAttributeMonth.Count > 0)
        {
            int loopCount = 0;
            for (loopCount = 0; loopCount < _argument.magicAttributeMonth.Count; loopCount++)
            {
                int month = _argument.magicAttributeMonth[loopCount];
                if (_data.month == month) break;
            }

            if (loopCount >= _argument.magicAttributeMonth.Count) return false;
        }


        return true;
    }

}

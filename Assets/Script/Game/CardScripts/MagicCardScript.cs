using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MagicCardScript : CardScriptBase
{
    public enum CardAttribute : int
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public enum CardAttributeMonth : int
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December,
    }

    [SerializeField,ReadOnly]
    private CardAttribute attribute = CardAttribute.Spring;

    public CardAttribute attributeType { get { return attribute; } }

    [SerializeField, ReadOnly]
    private CardAttributeMonth attributeMonth = 0;
    public CardAttributeMonth month { get { return attributeMonth; } }

    public void SetAttribute(int _attribute) {
        if (_attribute < 0) return;
        if (_attribute >= 12) return;

        attributeMonth = (CardAttributeMonth)_attribute;

        attribute = CardAttribute.Winter;

        if (_attribute >= (int)(attribute) * 3) return;
        attribute = CardAttribute.Autumn;

        if (_attribute >= (int)(attribute) * 3) return;
        attribute = CardAttribute.Summer;

        if (_attribute >= (int)(attribute) * 3) return;
        attribute = CardAttribute.Spring;
    }

    public int point { get; private set; } = 0;

    public void SetPoint(int _point) { point = _point; }

}

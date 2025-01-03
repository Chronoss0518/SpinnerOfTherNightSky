using System.Collections;
using System.Collections.Generic;
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

    public CardAttribute attribute { get; private set; } = CardAttribute.Spring;

    public int attributeNo { get; private set; } = 0;

    public void SetAttribute(int _attribute) {
        if (_attribute < 0) return;
        if (_attribute >= 12) return;

        attributeNo = _attribute;

        attribute = CardAttribute.Winter;

        if (_attribute < (int)(CardAttribute.Winter) * 3)
            attribute = CardAttribute.Autumn;

        if (_attribute < (int)(CardAttribute.Autumn) * 3)
            attribute = CardAttribute.Summer;

        if (_attribute < (int)(CardAttribute.Summer) * 3)
            attribute = CardAttribute.Spring;
    }

    public int point { get; private set; } = 0;

    public void SetPoint(int _point) { point = _point; }


}

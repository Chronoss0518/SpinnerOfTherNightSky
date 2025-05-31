using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MagicCardScript : CardScript.CardScriptBase
{
    FindStarFromMagicManager findStarFromMagicManager = FindStarFromMagicManager.ins;

    [SerializeField, ReadOnly]
    MagicCardData baseMagic = null;

    [SerializeField,ReadOnly]
    private MagicCardData.CardAttribute attribute = MagicCardData.CardAttribute.Spring;

    public MagicCardData.CardAttribute attributeType { get { return attribute; } }

    [SerializeField, ReadOnly]
    private MagicCardData.CardAttributeMonth attributeMonth = 0;
    public MagicCardData.CardAttributeMonth month { get { return attributeMonth; } }

    public int point { get { return baseMagic.point; } }
    public Vector2Int[] starPosList { get { return baseMagic.starPos; } }
    
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

    public override void Init(CardData _data)
    {
        baseMagic = (MagicCardData)_data;
        SetAttribute(baseMagic.month);

    }

    public override void SetSelectTargetTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
    {
        if (_argument.cardType != 0)
            if ((_argument.cardType & ScriptManager.SelectCardType.Magic) <= 0) return;
        if (baseData.cardType != (int)CardData.CardType.Magic) return;
        var magic = (MagicCardData)baseData;

        if (!SelectTargetArgumentTest(_argument, _runPlayer)) return;

        if (!IsPlayingMagicTest(_argument, magic)) return;

        if (!IsNormalPlayingMagicTest(_argument, magic, _runPlayer)) return;

        SelectTargetTestSuccess();
    }

    public void RemoveStone()
    {
        var magic = (MagicCardData)baseData;
        //FindStarPosOnBoard(magic);
    }

    bool IsPlayingMagicTest(ScriptManager.SelectCardArgument _argument, MagicCardData _data)
    {
        if (_argument.magicAttributeMonth.Count > 0)
        {
            bool findFlg = false;
            for (int loopCount = 0; loopCount < _argument.magicAttributeMonth.Count; loopCount++)
            {
                int month = _argument.magicAttributeMonth[loopCount];
                if (_data.month != month) continue;
                findFlg = true;
                break;
            }
            if (!findFlg) return false;
        }

        return true;
    }

    bool IsNormalPlayingMagicTest(ScriptManager.SelectCardArgument _argument, MagicCardData _data,Player _runPlayer)
    {
        if (!_argument.normalPlaying) return true;

        return findStarFromMagicManager.FindStarPosOnBoard(_data, _runPlayer);
    }

}

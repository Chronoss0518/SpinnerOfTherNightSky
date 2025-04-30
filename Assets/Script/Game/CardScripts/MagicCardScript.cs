using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MagicCardScript : CardScript.CardScriptBase
{
    RectInt starPosSize = new RectInt(0, 0, 0, 0);
    
    //àÍî‘ç∂ë§Ç…Ç†ÇËéûì_Ç≈àÍî‘è„ÇÃà íu((1,2)Ç∆(2,1)Ç≈ÇÕ(1,2)ÇóDêÊÇ∑ÇÈ)//
    Vector2Int starPosLeftTopPos = Vector2Int.zero;

    [SerializeField,ReadOnly]
    private MagicCardData.CardAttribute attribute = MagicCardData.CardAttribute.Spring;

    public MagicCardData.CardAttribute attributeType { get { return attribute; } }

    [SerializeField, ReadOnly]
    private MagicCardData.CardAttributeMonth attributeMonth = 0;
    public MagicCardData.CardAttributeMonth month { get { return attributeMonth; } }

    public bool removeStoneFailedFlg { get; private set; } = false;
    public int point { get; private set; } = 0;
    public void SetPoint(int _point) { point = _point; }

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
        var magicData = (MagicCardData)_data;
        SetAttribute(magicData.month);
        SetPoint(magicData.point);

        starPosSize.xMin =  starPosSize.yMin = 99;
        starPosSize.xMax = starPosSize.yMax = 0;

        foreach (var pos in magicData.starPos)
        {
            starPosSize.xMin = starPosSize.xMin > pos.x ? pos.x : starPosSize.xMin;
            starPosSize.xMax = starPosSize.xMax < pos.x ? pos.x : starPosSize.xMax;
            starPosSize.yMin = starPosSize.yMin > pos.y ? pos.y : starPosSize.yMin;
            starPosSize.yMax = starPosSize.yMax > pos.y ? pos.y : starPosSize.yMax;

            if (starPosLeftTopPos.x < pos.x) continue;
            if (starPosLeftTopPos.y < pos.y) continue;
            starPosLeftTopPos = pos;
        }
    }

    public override void SetSelectTargetTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
    {
        if (_argument.cardType != 0)
            if ((_argument.cardType & ScriptManager.SelectCardType.Magic) <= 0) return;
        if (baseData.cardType != (int)CardData.CardType.Magic) return;
        var magic = (MagicCardData)baseData;

        if (!SelectTargetArgumentTest(_argument, _runPlayer)) return;

        if (!IsPlayingMagicTest(_argument, magic)) return;

        if (!IsNormalPlayingMagicTest(_argument, magic)) return;

        SelectTargetTestSuccess();
    }

    public void RemoveStoneTest()
    {
        
    }

    bool IsPlayingMagicTest(ScriptManager.SelectCardArgument _argument, MagicCardData _data)
    {
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


    bool IsNormalPlayingMagicTest(ScriptManager.SelectCardArgument _argument, MagicCardData _data)
    {
        if (!_argument.normalPlaying) return true;

        var stoneBoard = manager.stoneBoardObj;

        for (int v = starPosLeftTopPos.y; v < stoneBoard.VERTICAL_SIZE - starPosSize.yMax; v++)
        {
            for (int h = starPosLeftTopPos.x; h < stoneBoard.VERTICAL_SIZE - starPosSize.xMax; h++)
            {
                if (!stoneBoard.IsPutStone(v, h)) continue;
                if (!FindStarPos(h, v, _data, stoneBoard)) continue;
                return true;
            }
        }

        return false;
    }


    bool FindStarPos(int targetX, int targetY, MagicCardData _data,StoneBoardManager _stoneBoard)
    {
        foreach(var pos in _data.starPos)
        {
            if (!_stoneBoard.IsPutStone(targetX + pos.x, targetY + pos.y))
                return false;
        }

        return true;
    }

}

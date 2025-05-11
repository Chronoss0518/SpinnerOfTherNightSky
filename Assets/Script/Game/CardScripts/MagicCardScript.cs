using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MagicCardScript : CardScript.CardScriptBase
{
    Vector2Int starPosMaxPos = Vector2Int.zero;
    Vector2Int starPosMinPos = new Vector2Int(99,99);
    
    //àÍî‘ç∂ë§Ç…Ç†ÇËéûì_Ç≈àÍî‘è„ÇÃà íu((1,2)Ç∆(2,1)Ç≈ÇÕ(1,2)ÇóDêÊÇ∑ÇÈ)//
    Vector2Int starPosLeftTopPos = new Vector2Int(99, 99);

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

        foreach (var pos in magicData.starPos)
        {
            starPosMinPos.x = starPosMinPos.x > pos.x ? pos.x : starPosMinPos.x;
            starPosMaxPos.x = starPosMaxPos.x < pos.x ? pos.x : starPosMaxPos.x;
            starPosMinPos.y = starPosMinPos.y > pos.y ? pos.y : starPosMinPos.y;
            starPosMaxPos.y = starPosMaxPos.y < pos.y ? pos.y : starPosMaxPos.y;

            
            if (starPosLeftTopPos.x == pos.x)
                if (starPosLeftTopPos.y > pos.y) 
                    starPosLeftTopPos.y = pos.y;

            if (starPosLeftTopPos.x > pos.x)
                starPosLeftTopPos = pos;
        }

        Debug.Log($"[{_data.name}] Is \n" +
            $"MinPos X[{starPosMinPos.x}] Y[{starPosMinPos.y}] \n" +
            $"MaxPos X[{starPosMaxPos.x}] Y[{starPosMaxPos.y}] \n" +
            $"LtPos X[{starPosLeftTopPos.x}] Y[{starPosLeftTopPos.y}] ");

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

    public void RemoveStone()
    {
        var magic = (MagicCardData)baseData;
        FindStarPosOnBoard(magic);

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


    bool IsNormalPlayingMagicTest(ScriptManager.SelectCardArgument _argument, MagicCardData _data)
    {
        if (!_argument.normalPlaying) return true;

        return FindStarPosOnBoard(_data);
    }

    bool FindStarPosOnBoard(MagicCardData _data)
    {
        var stoneBoard = manager.stoneBoardObj;

        for (int v = starPosLeftTopPos.y; v < stoneBoard.PANEL_COUNT_Y - (starPosMaxPos.y - starPosMinPos.y); v++)
        {
            for (int h = starPosLeftTopPos.x; h < stoneBoard.PANEL_COUNT_X -(starPosMaxPos.x - starPosMinPos.x); h++)
            {
                if (!stoneBoard.IsPutStone(h, v)) continue;
                if (!FindStarPos(h, v, _data, stoneBoard)) continue;
                return true;
            }
        }

        return false;
    }


    bool FindStarPos(int targetX, int targetY, MagicCardData _data,StoneBoardManager _stoneBoard)
    {

        foreach (var pos in _data.starPos)
        {
            if (_data.name == "ìÏè\éö-Complater")
                Debug.Log($"Test pos x[{targetX + pos.x - starPosLeftTopPos.x}] y[{targetY + pos.y - starPosLeftTopPos.y}]");

            if (!_stoneBoard.IsPutStone(targetX + pos.x - starPosLeftTopPos.x, targetY + pos.y - starPosLeftTopPos.y))
                return false;
        }

        return true;
    }

}

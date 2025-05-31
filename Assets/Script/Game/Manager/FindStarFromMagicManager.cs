using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Unity.Collections;


[System.Serializable]
public class FindStarFromMagicManager
{
    private FindStarFromMagicManager() { }

    static public FindStarFromMagicManager ins { get; private set; } = new FindStarFromMagicManager();

    [System.Serializable]
    public class FindData
    {
        public MagicCardData magicData = null;

        [SerializeField, ReadOnly]
        public Vector2Int maxPos = Vector2Int.zero;

        [SerializeField, ReadOnly]
        public Vector2Int minPos = new Vector2Int(99, 99);

        //àÍî‘ç∂ë§Ç…Ç†ÇËéûì_Ç≈àÍî‘è„ÇÃà íu((1,2)Ç∆(2,1)Ç≈ÇÕ(1,2)ÇóDêÊÇ∑ÇÈ)//
        [SerializeField, ReadOnly]
        public Vector2Int leftTopPos = new Vector2Int(99, 99);
    }

    private Dictionary<int, FindData> magicCardFindDataMap = new Dictionary<int, FindData>();

    [SerializeField,ReadOnly]
    FindData tmp = null;

    public void Init(GameManager _manager)
    {
        if (manager != null) return;
        manager = _manager;
    }

    public void Release()
    {
        manager = null;
        magicCardFindDataMap.Clear();
    }

    public void AddMagicCard(CardData _data)
    {
        if (_data == null) return;
        if (_data.cardType != (int)CardData.CardType.Magic) return;
        if (magicCardFindDataMap.ContainsKey(_data.id)) return;

        FindData data = new FindData();

        magicCardFindDataMap.Add(_data.id, data);

        data.magicData = (MagicCardData)_data;

        foreach (var pos in data.magicData.starPos)
        {
            data.minPos.x = data.minPos.x > pos.x ? pos.x : data.minPos.x;
            data.maxPos.x = data.maxPos.x < pos.x ? pos.x : data.maxPos.x;
            data.minPos.y = data.minPos.y > pos.y ? pos.y : data.minPos.y;
            data.maxPos.y = data.maxPos.y < pos.y ? pos.y : data.maxPos.y;


            if (data.leftTopPos.x == pos.x)
                if (data.leftTopPos.y > pos.y)
                    data.leftTopPos.y = pos.y;

            if (data.leftTopPos.x > pos.x)
                data.leftTopPos = pos;
        }

        if (_data.id == 0) tmp = data;

    }

    public bool FindStarPosOnBoard(MagicCardData _data, Player _runPlayer)
    {
        if (_data == null) return false;
        if (manager == null) return false;
        if (_runPlayer == null) return false;
        if (!magicCardFindDataMap.ContainsKey(_data.id)) return false;

        var stoneBoard = manager.stoneBoardObj;

        var findData = magicCardFindDataMap[_data.id];

        for (int v = findData.minPos.y; v < stoneBoard.PANEL_COUNT_Y - findData.maxPos.y; v++)
        {
            for (int h = findData.minPos.x; h < stoneBoard.PANEL_COUNT_X - findData.maxPos.x; h++)
            {
                if (!FindStarPos(h, v, _data, _runPlayer)) continue;
                return true;
            }
        }

        return false;
    }

    public bool FindStarPos(int targetX, int targetY, CardData _data, Player _runPlayer)
    {
        if (_data == null) return false;
        if (manager == null) return false;
        if (_runPlayer == null) return false;
        if (!magicCardFindDataMap.ContainsKey(_data.id)) return false;
        
        var stoneBoard = manager.stoneBoardObj;
       
        var tmpPos = stoneBoard.GetPlayerPositionPos(targetX, targetY, _runPlayer.position);

        if (!stoneBoard.IsPutStone(tmpPos.x , tmpPos.y))return false;

        var findData = magicCardFindDataMap[_data.id];

        foreach (var pos in findData.magicData.starPos)
        {
            if (pos.x == findData.leftTopPos.x &&
                pos.y == findData.leftTopPos.y) continue;

            tmpPos = stoneBoard.GetPlayerPositionPos(targetX + pos.x - findData.leftTopPos.x, targetY + pos.y - findData.leftTopPos.y, _runPlayer.position);

            if (_data.id == 0) Debug.Log($"Test Pos X[{tmpPos.x}],Y[{tmpPos.y}]");

            if (!stoneBoard.IsPutStone(tmpPos.x, tmpPos.y))
                return false;
        }

        return true;
    }

    private GameManager manager = null;


}

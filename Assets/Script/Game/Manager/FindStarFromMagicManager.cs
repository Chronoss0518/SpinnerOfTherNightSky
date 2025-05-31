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


    }

    public bool FindStarPosOnBoard(MagicCardData _data, Player _runPlayer)
    {
        if (_data == null) return false;
        if (manager == null) return false;
        if (_runPlayer == null) return false;
        if (!magicCardFindDataMap.ContainsKey(_data.id)) return false;

        var stoneBoard = manager.stoneBoardObj;

        var findData = magicCardFindDataMap[_data.id];

        //for (int v = findData.minPos.y; v < stoneBoard.PANEL_COUNT_Y - findData.maxPos.y; v++)
        for (int v = 0; v < stoneBoard.PANEL_COUNT_Y; v++)
        {
            //for (int h = findData.minPos.x; h < stoneBoard.PANEL_COUNT_X - findData.maxPos.x; h++)
            for (int h = 0; h < stoneBoard.PANEL_COUNT_X; h++)
            {
                if (FindStarPos(h, v, _data, _runPlayer) == null) continue;
                return true;
            }
        }

        return false;
    }

    public List<Vector2Int> FindStarPos(int targetX, int targetY, CardData _data, Player _runPlayer)
    {
        if (_data == null) return null;
        if (manager == null) return null;
        if (_runPlayer == null) return null;
        if (!magicCardFindDataMap.ContainsKey(_data.id)) return null;
        
        var stoneBoard = manager.stoneBoardObj;
       
        var startPos = GetPlayerPositionPos(targetX, targetY, _runPlayer.position, stoneBoard);

        if (!stoneBoard.IsPutStone(startPos.x , startPos.y))return null;
        var findData = magicCardFindDataMap[_data.id];

        var res = new List<Vector2Int>();

        res.Add(startPos);
        var leftTopPos = GetPlayerPositionPos(findData.leftTopPos.x, findData.leftTopPos.y, _runPlayer.position, findData);

        foreach (var pos in findData.magicData.starPos)
        {
            if (pos.x == findData.leftTopPos.x &&
                pos.y == findData.leftTopPos.y) continue;

            var tmpPos = GetPlayerPositionPos(pos.x, pos.y, _runPlayer.position,findData);
            tmpPos.x = startPos.x + tmpPos.x - leftTopPos.x;
            tmpPos.y = startPos.y + tmpPos.y - leftTopPos.y;

            if (!stoneBoard.IsPutStone(tmpPos.x,tmpPos.y))
                return null;
            res.Add(tmpPos);
        }

        return res;
    }

    private GameManager manager = null;


    public Vector2Int GetPlayerPositionPos(int _x, int _y, GameManager.PlayerPosition _playerPosition,StoneBoardManager _stoneBoard)
    {
        return GetPlayerPosotionPosBase(_x, _y, _playerPosition, _stoneBoard.PANEL_COUNT_X, _stoneBoard.PANEL_COUNT_Y);
    }



    public Vector2Int GetPlayerPositionPos(int _x, int _y, GameManager.PlayerPosition _playerPosition, FindData _data)
    {
        return GetPlayerPosotionPosBase(_x, _y, _playerPosition, _data.maxPos.x, _data.maxPos.y);
    }

    private Vector2Int GetPlayerPosotionPosBase(int _x, int _y, GameManager.PlayerPosition _playerPosition, int _maxX,int _maxY)
    {
        int posX = _x;
        int posY = _y;

        if (_playerPosition == GameManager.PlayerPosition.Right)
        {
            posX = _maxX - _y;
            posY = _x;
        }
        if (_playerPosition == GameManager.PlayerPosition.Back)
        {
            posX = _maxX - _x;
            posY = _maxY - _y;
        }

        if (_playerPosition == GameManager.PlayerPosition.Left)
        {
            posX = _y;
            posY = _maxY - _x;
        }

        return new Vector2Int(posX, posY);
    }
}

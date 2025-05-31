using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class StoneBoardManager : PanelPosBase
{

    [SerializeField]
    int RANDOM_COUNT = 100;

    [SerializeField]
    GameObject stonePosPrefab = null;

    [SerializeField]
    GameManager gameManager = null;

    public float stonePosTop { get; private set; } = 0.0f;

    [SerializeField,ReadOnly]
    StonePosScript[][] stoneList = null;

    [SerializeField, ReadOnly]
    private bool isBlockFlg = false;

    public bool isBlock { get { return isBlockFlg; } }

    protected override void Init()
    {
        base.Init();
    }

    public Vector2Int GetPlayerPositionPos(int _x,int _y,GameManager.PlayerPosition _playerPosition)
    {
        int posX = _x;
        int posY = _y;

        if (_playerPosition == GameManager.PlayerPosition.Right)
        {
            posX = _y;
            posY = PANEL_COUNT_Y - _x;
        }

        if (_playerPosition == GameManager.PlayerPosition.Back)
        {
            posX = PANEL_COUNT_X - _x;
            posY = PANEL_COUNT_Y - _y;
        }

        if (_playerPosition == GameManager.PlayerPosition.Left)
        {
            posX = PANEL_COUNT_X - _y;
            posY = _x;
        }
        return new Vector2Int(posX, posY);
    }

    public void SetBlockFlg(bool _flg)
    {
        isBlockFlg = _flg;
    }

    public void PutStone(int _x, int _y, GameObject _stone)
    {
        if (!IsRange(_x, _y)) return;

        stoneList[_y][_x].PutStone(_stone);
    }

    public void PutRandomStone(int putCount, GameObject _stone)
    {
        if (_stone == null) return;
        if (putCount <= 0) return;

        int fieldSize = (PANEL_COUNT_Y) * (PANEL_COUNT_X);
        Vector2Int[] positions = new Vector2Int[fieldSize];
        int[] numList = new int[fieldSize];

        int tmpLoopCount = 0;

        for (tmpLoopCount = 0; tmpLoopCount < fieldSize; tmpLoopCount++)
        {
            positions[tmpLoopCount] = new Vector2Int(tmpLoopCount % (PANEL_COUNT_X), tmpLoopCount / (PANEL_COUNT_Y));
            numList[tmpLoopCount] = tmpLoopCount;
        }

        int changeNum = 0;
        int baseNum = 0;

        for (tmpLoopCount = 0; tmpLoopCount < RANDOM_COUNT; tmpLoopCount++)
        {
            for (baseNum = 0; baseNum<numList.Length; baseNum++)
            {
                changeNum = Random.Range(0, fieldSize);

                if (baseNum == changeNum) continue;
                numList[baseNum] += numList[changeNum];
                numList[changeNum] = numList[baseNum] - numList[changeNum];
                numList[baseNum] = numList[baseNum] - numList[changeNum];
            }

        }
        Vector2Int pos = Vector2Int.zero;
        for (tmpLoopCount = 0; tmpLoopCount < putCount; tmpLoopCount++)
        {
            pos = positions[numList[tmpLoopCount]];
            stoneList[pos.x][pos.y].PutStone(_stone);
        }

    }

    public void RemoveStone(int _x, int _y)
    {
        if (IsPutStone(_x,_y)) return;

        stoneList[_y][_x].RemovePutStone();
    }

    public void SelectStonePos(int _x, int _y)
    {
        if (!IsRange(_x, _y)) return;

        stoneList[_y][_x].SelectStonePos();
    }

    public void UnSelectStonePos(int _x, int _y)
    {
        if (!IsSelectStonePos(_x, _y)) return;

        stoneList[_y][_x].UnSelectStonePos();
    }

    public bool IsRange(int _x, int _y)
    {
        return
            _x >= 0 && _x < PANEL_COUNT_X &&
            _y >= 0 && _y < PANEL_COUNT_Y;
    }

    public bool IsPutStone(int _x, int _y)
    {
        if (!IsRange(_x, _y)) return false;
        return stoneList[_y][_x].IsPutStone();
    }

    public bool IsSelectStonePos(int _x, int _y)
    {
        if (!IsRange(_x, _y)) return false;

        return stoneList[_y][_x].IsPutStone();
    }

    public StonePosScript GetStonePosScript(int _x,int _y)
    {
        if (!IsRange(_x, _y)) return null;
        return stoneList[_y][_x];
    }

    override protected void InitHoryzontalList(int _nowCount)
    {
        stoneList[_nowCount] = new StonePosScript[PANEL_COUNT_X];
    }

    override protected void InitVerticalList()
    {
        stoneList = null;
        stoneList = new StonePosScript[PANEL_COUNT_Y][];
    }

    override protected Vector3 SetStartPos(Vector3 _startPos)
    {
        stonePosTop = _startPos.y;
        return _startPos;
    }

    protected override void CreateObject(float _vPos, Vector2Int _pos, GameObject _verticalPos, PanelPosManager _builder)
    {
        var stonePos = Instantiate(stonePosPrefab, _verticalPos.transform);

        stonePos.name = $"X[{_pos.x}]Y[{_pos.y}]";
        stonePos.transform.localPosition = new Vector3(_vPos, 0.0f, 0.0f);
        var col = stonePos.GetComponent<BoxCollider>();
        col.size = new Vector3(Mathf.Abs(_builder.interval.x), 1.0f, Mathf.Abs(_builder.interval.y));

        var stonePosScript = stonePos.GetComponent<StonePosScript>();
        stonePosScript.Init(gameManager, _pos);

        stoneList[_pos.y][_pos.x] = stonePos.GetComponent<StonePosScript>();
    }


}

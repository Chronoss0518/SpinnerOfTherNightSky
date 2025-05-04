using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;



public class StoneBoardManager : MonoBehaviour
{

    [SerializeField]
    int RANDOM_COUNT = 100;

    //Horyzontal : êÖïΩ//
    [SerializeField]
    private int HOLYZONTAL = 13;

    public int HOLYZONTAL_SIZE { get { return HOLYZONTAL - 1; } }

    [SerializeField]
    //Vertical : êÇíº//
    private int VERTICAL = 13;

    public int VERTICAL_SIZE { get { return VERTICAL - 1; } }

    [SerializeField]
    MeshFilter boardObject = null;

    [SerializeField]
    GameObject stonePosPrefab = null;

    [SerializeField]
    GameManager gameManager = null;

    [SerializeField,ReadOnly]
    private Vector2 interval = Vector2.zero;

    [SerializeField, ReadOnly]
    private Vector3 startPos = Vector3.zero;

    [SerializeField,ReadOnly]
    List<List<StonePosScript>> stoneList = new List<List<StonePosScript>>();

    [SerializeField]
    List<int> putStoneList = new List<int>();

    [SerializeField, ReadOnly]
    private bool isBlockFlg = false;

    public bool isBlock { get { return isBlockFlg; } }

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

        int fieldSize = (VERTICAL_SIZE) * (HOLYZONTAL_SIZE);
        Vector2Int[] positions = new Vector2Int[fieldSize];
        int[] numList = new int[fieldSize];

        int tmpLoopCount = 0;

        for (tmpLoopCount = 0; tmpLoopCount < fieldSize; tmpLoopCount++)
        {
            positions[tmpLoopCount] = new Vector2Int(tmpLoopCount % (HOLYZONTAL_SIZE), tmpLoopCount / (VERTICAL_SIZE));
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
            _x >= 0 && _x < HOLYZONTAL_SIZE &&
            _y >= 0 && _y < VERTICAL_SIZE;
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

    public void Init()
    {
        if (stonePosPrefab == null) return;

        InitBoadrSize();

        Vector3 pos = startPos;

        stoneList.Clear();

        for (int i = 0; i<VERTICAL_SIZE; i++)
        {
            pos.x = startPos.x;
            stoneList.Add(new List<StonePosScript>());

            var verticalPos = new GameObject("VerticalStonePos");
            verticalPos.transform.SetParent(transform);
            verticalPos.transform.localPosition = pos;
            float tmpVPos = 0.0f;
            for (int j = 0; j < HOLYZONTAL_SIZE; j++)
            {
                var stonePos = Instantiate(stonePosPrefab, verticalPos.transform);

                InitStonePos(stonePos, tmpVPos, new Vector2Int(j,i));

                stoneList[i].Add(stonePos.GetComponent<StonePosScript>());
                tmpVPos += interval.x;
            }

            pos.z += interval.y;
        }
    }

    void InitBoadrSize()
    {

        if (boardObject == null) return;
        var bounds = boardObject.mesh.bounds;

        var size = bounds.max - bounds.min;


        size = boardObject.transform.localToWorldMatrix.MultiplyPoint(size);

        interval.y = size.z / VERTICAL;
        interval.x = size.x / HOLYZONTAL;

        var min = boardObject.transform.localToWorldMatrix.MultiplyPoint(bounds.min);
        var max = boardObject.transform.localToWorldMatrix.MultiplyPoint(bounds.max);

        startPos = min;

        startPos.x += interval.x;
        startPos.z += interval.y;
        startPos.y = max.y;

    }



    void InitStonePos(GameObject _obj,float _vPos,Vector2Int _pos)
    {
        _obj.transform.localPosition = new Vector3(_vPos, 0.0f, 0.0f);
        var col = _obj.GetComponent<BoxCollider>();
        col.size = new Vector3(Mathf.Abs(interval.x), 1.0f, Mathf.Abs(interval.y)) * 0.5f;

        var stonePosScript = _obj.GetComponent<StonePosScript>();
        stonePosScript.Init(gameManager, _pos);

    }


}

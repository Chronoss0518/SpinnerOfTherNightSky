using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections;
using UnityEngine;

public class StarPosSheet : MonoBehaviour
{
    //Horyzontal : êÖïΩ//
    [SerializeField]
    private int HOLYZONTAL = 7;

    public int HOLYZONTAL_SIZE { get { return HOLYZONTAL - 1; } }

    [SerializeField]
    //Vertical : êÇíº//
    private int VERTICAL = 7;

    public int VERTICAL_SIZE { get { return VERTICAL - 1; } }

    [SerializeField]
    GameObject starPosPrefab = null;

    [SerializeField, ReadOnly]
    GameObject[][] starPosList = null;

    [SerializeField]
    MeshFilter meshFilter = null;

    [SerializeField, ReadOnly]
    private Vector2 interval = Vector2.zero;

    public Vector3 size { get; private set; } = Vector3.zero;

    [SerializeField, ReadOnly]
    private Vector3 startPos = Vector3.zero;

    public Vector2 beforePoint = Vector2.zero;

    [SerializeField,ReadOnly]
    StoneBoardManager stoneBoard = null;

    bool movePanelFlg = false;

    public void PointDownGrip() { movePanelFlg = true; }
    
    public void PointUpGrip() { movePanelFlg = false; }


    public void InitPanelSize()
    {

        if (meshFilter == null) return;
        var bounds = meshFilter.mesh.bounds;

        var tmp = bounds.max - bounds.min;

        tmp = meshFilter.transform.localToWorldMatrix.MultiplyPoint(tmp);

        interval.y = tmp.z / VERTICAL;
        interval.x = tmp.x / HOLYZONTAL;

        var minPos = meshFilter.transform.localToWorldMatrix.MultiplyPoint(bounds.min);
        var maxPos = meshFilter.transform.localToWorldMatrix.MultiplyPoint(bounds.max);

        startPos = minPos;

        startPos.x += interval.x;
        startPos.z += interval.y;
        startPos.y = 0.0f;

        tmp.x = Mathf.Abs(tmp.x);
        tmp.y = Mathf.Abs(tmp.y);
        tmp.z = Mathf.Abs(tmp.z);
        size = tmp;
    }

    public void Init()
    {
        InitPanelSize();

        starPosList = new GameObject[HOLYZONTAL_SIZE][];


        Vector3 pos = startPos;
        for (int i = 0; i < HOLYZONTAL_SIZE; i++)
        {
            pos.x = startPos.x;
            starPosList[i] = new GameObject[HOLYZONTAL_SIZE];

            var verticalPos = new GameObject("VerticalStonePos");
            verticalPos.transform.SetParent(transform);
            verticalPos.transform.localPosition = pos;
            float tmpVPos = 0.0f;
            for (int j = 0; j < HOLYZONTAL_SIZE; j++)
            {
                var tmpPos = new Vector2Int(j, i);

                InitStoneSheetPos(tmpVPos, tmpPos, verticalPos);

                tmpVPos += interval.x;
            }

            pos.z += interval.y;
        }

    }

    public void SetStoneBoard(StoneBoardManager _stoneBoard)
    {
        stoneBoard = _stoneBoard;
        transform.localPosition = new Vector3(transform.localPosition.x, stoneBoard.stonePosTop, transform.localPosition.z);
    }

    public void AddStarPos(GameObject _obj, int _x, int _y)
    {
        if (!IsRange(_x, _y)) return;
        starPosList[_y][_x] = _obj;
    }

    public void SetStarPosFlg(int _x,int _y,bool _flg)
    {
        if (!IsRange(_x, _y)) return;
        starPosList[_y][_x].SetActive(_flg);
    }

    public bool IsRange(int _x, int _y)
    {
        return
            _x >= 0 && _x < HOLYZONTAL_SIZE &&
            _y >= 0 && _y < VERTICAL_SIZE;
    }

    void InitStoneSheetPos(float _vPos, Vector2Int _pos, GameObject _verticalPos)
    {
        var starPos = Instantiate(starPosPrefab, _verticalPos.transform);

        starPos.transform.localPosition = new Vector3(_vPos, 0.0f, 0.0f);

        starPosList[_pos.y][_pos.x] = starPos;
    }


    void Update()
    {
        if (stoneBoard == null) return;

        MoveTest();

        var pos = Vector3.zero;
        
        pos.x -= MinTest(transform.localPosition.x - size.x, stoneBoard.transform.localPosition.x - stoneBoard.size.x);
        pos.z -= MinTest(transform.localPosition.z - size.z, stoneBoard.transform.localPosition.z - stoneBoard.size.z);

        pos.x += MaxTest(transform.localPosition.x + size.x, stoneBoard.transform.localPosition.x + stoneBoard.size.x);
        pos.z += MaxTest(transform.localPosition.z + size.z, stoneBoard.transform.localPosition.z + stoneBoard.size.z);

        transform.localPosition += pos;
    }

    float MinTest(float _base, float _target)
    {
        float tmpSize = _base - _target;
        if (tmpSize < 0.0f)
            return tmpSize;
        return 0.0f;
    }
    float MaxTest(float _base, float _target)
    {
        float tmpSize = _target - _base;
        if (tmpSize < 0.0f)
            return tmpSize;
        return 0.0f;
    }

    void MoveTest()
    {
        if (!movePanelFlg) return;

        

    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;



public class StoneBoardManager : MonoBehaviour
{
    //Horyzontal : êÖïΩ//
    [SerializeField]
    private int HOLYZONTAL = 13;

    public int HOLYZONTAL_SIZE { get { return HOLYZONTAL; } }

    [SerializeField]
    //Vertical : êÇíº//
    private int VERTICAL = 13;

    public int VERTICAL_SIZE { get { return VERTICAL; } }

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

    [SerializeField, ReadOnly]
    List<GameObject> createVerticalStonePos = new List<GameObject>();

    [SerializeField, ReadOnly]
    private bool isBlockFlg = false;

    public bool isBlock { get { return isBlockFlg; } }

    public bool IsRange(int _x,int _y)
    {
        return 
            _x >= 0 && _x < HOLYZONTAL &&
            _y >= 0 && _y < VERTICAL;
    }

    public void SetBlockFlg(bool _flg)
    {
        isBlockFlg = _flg;
    }

    public void SetActive(bool _flg)
    {
        foreach(var pos in createVerticalStonePos)
        {
            pos.SetActive(_flg);
        }
    }

    public void SetActiveIsNotPut(bool _flg)
    {
        foreach (var pos in createVerticalStonePos)
        {
            if(pos.transform.childCount <= 0)
                pos.SetActive(_flg);
        }
    }

    public void SetActiveIsPut(bool _flg)
    {
        foreach (var pos in createVerticalStonePos)
        {
            if (pos.transform.childCount > 0)
                pos.SetActive(_flg);
        }
    }

    public void PutStone(int _x, int _y,GameObject _stone)
    {
        if (!IsRange(_x, _y)) return;

        stoneList[_x][_y].PutStone(_stone);
    }

    public void RemoveStone(int _x, int _y)
    {
        if (IsPutStone(_x,_y)) return;

        stoneList[_x][_y].RemovePutStone();
    }


    public bool IsPutStone(int _x, int _y)
    {
        if (!IsRange(_x, _y)) return false;

        return stoneList[_x][_y].IsPutStone();
    }


    public void SelectStonePos(int _x, int _y)
    {
        if (!IsRange(_x, _y)) return;

        stoneList[_x][_y].SelectStonePos();
    }

    public void UnSelectStonePos(int _x, int _y)
    {
        if (!IsSelectStonePos(_x, _y)) return;

        stoneList[_x][_y].UnSelectStonePos();
    }


    public bool IsSelectStonePos(int _x, int _y)
    {
        if (!IsRange(_x, _y)) return false;

        return stoneList[_x][_y].IsPutStone();
    }

    public StonePosScript GetStonePosScript(int _x,int _y)
    {
        if (!IsRange(_x, _y)) return null;
        return stoneList[_x][_y];
    }

    public void Init()
    {
        if (stonePosPrefab == null) return;

        InitBoadrSize();

        Vector3 pos = startPos;

        stoneList.Clear();

        for (int i = 0; i<VERTICAL - 1; i++)
        {
            pos.x = startPos.x;
            stoneList.Add(new List<StonePosScript>());

            var verticalPos = new GameObject("VerticalStonePos");
            verticalPos.transform.SetParent(transform);
            verticalPos.transform.localPosition = pos;
            float tmpVPos = 0.0f;
            for (int j = 0; j < HOLYZONTAL - 1; j++)
            {
                var stonePos = Instantiate(stonePosPrefab, verticalPos.transform);

                InitStonePos(stonePos, tmpVPos, new Vector2Int(i, j));

                stoneList[i].Add(stonePos.GetComponent<StonePosScript>());
                tmpVPos += interval.x;
            }

            createVerticalStonePos.Add(verticalPos);
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

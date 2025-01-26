using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;



public class StoneBoardManager : MonoBehaviour
{
    //Horyzontal : êÖïΩ//
    [SerializeField]
    private int HOLYZONTAL = 13;

    [SerializeField]
    //Vertical : êÇíº//
    private int VERTICAL = 13;

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
    List<List<GameObject>> stoneList = new List<List<GameObject>>();

    [SerializeField, ReadOnly]
    List<GameObject> createVerticalStonePos = new List<GameObject>();

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
        if (_x < 0 || _x >= HOLYZONTAL) return;
        if (_y < 0 || _y >= VERTICAL) return;

        var stone = Instantiate(_stone.gameObject, stoneList[_y][_x].transform);
    }

    public void RemoveStone(int _x, int _y)
    {
        if (!IsPutStone(_x, _y)) return;

        var child = stoneList[_x][_y].transform.GetChild(stoneList[_x][_y].transform.childCount - 1);
        child.parent = null;
        Destroy(child.gameObject);
    }


    public bool IsPutStone(int _x, int _y)
    {
        if (_x < 0 || _x >= HOLYZONTAL) return false;
        if (_y < 0 || _y >= VERTICAL) return false;

        return stoneList[_x][_y].transform.childCount > 0;
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
            stoneList.Add(new List<GameObject>());

            var verticalPos = new GameObject("VerticalStonePos");
            verticalPos.transform.SetParent(transform);
            verticalPos.transform.localPosition = pos;
            float tmpVPos = 0.0f;
            for (int j = 0; j < HOLYZONTAL - 1; j++)
            {
                var stonePos = Instantiate(stonePosPrefab, verticalPos.transform);

                InitStonePos(stonePos, tmpVPos, new Vector2Int(j, i));

                stoneList[i].Add(stonePos);
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
        startPos.y = max.y - 1.0f;

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

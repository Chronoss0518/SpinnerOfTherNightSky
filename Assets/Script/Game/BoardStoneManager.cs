using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;



public class BoardStoneManager : MonoBehaviour
{
    //Horyzontal : êÖïΩ//
    [SerializeField]
    private int HOLYZONTAL = 13;

    [SerializeField]
    //Vertical : êÇíº//
    private int VERTICAL = 13;

    [SerializeField]
    private Vector2 interval = new Vector2(0.46f, 0.46f);

    [SerializeField]
    private Vector3 startPos = Vector3.zero;

    [SerializeField,ReadOnly]
    List<List<GameObject>> stoneList = new List<List<GameObject>>();

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
        Vector3 pos = startPos;

        stoneList.Clear();

        for (int i = 0; i<VERTICAL; i++)
        {
            pos.x = startPos.x;
            stoneList.Add(new List<GameObject>());

            var verticalPos = new GameObject("VerticalStonePos");
            verticalPos.transform.SetParent(transform);
            verticalPos.transform.localPosition = pos;
            float tmpVPos = 0.0f;
            for (int j = 0; j < HOLYZONTAL; j++)
            {
                var stonePos = new GameObject("StonePos");
                stonePos.transform.SetParent(verticalPos.transform);
                stonePos.transform.localPosition = new Vector3(tmpVPos, 0.0f, 0.0f);
                stoneList[i].Add(stonePos);
                tmpVPos += interval.x;
            }


            pos.z += interval.y;
        }
    }

}

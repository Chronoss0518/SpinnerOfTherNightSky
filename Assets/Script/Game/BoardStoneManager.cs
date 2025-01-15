using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR_WIN
using UnityEditor;
#endif


[System.Serializable]
public class StoneHorizontal
{
    public List<GameObject> stoneList = new List<GameObject>();
}


public class BoardStoneManager : MonoBehaviour
{
    //Horyzontal : êÖïΩ//
    public const int h = 13;
    //Vertical : êÇíº//
    public const int v = 13;

    [SerializeField]
    List<StoneHorizontal> stoneList = new List<StoneHorizontal>();

    public void PutStone(int _x, int _y,GameObject _stone)
    {
        if (_x < 0 || _x >= h) return;
        if (_y < 0 || _y >= v) return;

        var stone = Instantiate(_stone.gameObject, stoneList[_y].stoneList[_x].transform);
    }

    public void RemoveStone(int _x, int _y)
    {
        if (!IsPutStone(_x, _y)) return;

        var child = stoneList[_x].stoneList[_y].transform.GetChild(stoneList[_x].stoneList[_y].transform.childCount - 1);
        child.parent = null;
        Destroy(child.gameObject);
    }


    public bool IsPutStone(int _x, int _y)
    {
        if (_x < 0 || _x >= h) return false;
        if (_y < 0 || _y >= v) return false;

        return stoneList[_x].stoneList[_y].transform.childCount > 0;
    }

#if UNITY_EDITOR_WIN

    [MenuItem("Create/StoneItem")]
    public static void CreateStoneList()
    {
        var manager = FindAnyObjectByType<BoardStoneManager>();

        if (manager == null) return;

        float tmpH = -2.78f;
        float tmpV = -2.78f;

        manager.stoneList.Clear();

        for (int i = 0;i<v;i++)
        {
            

            var verticalPos = new GameObject("VerticalStonePos");
            verticalPos.transform.position = new Vector3(0.0f, 0.48f, tmpH);

            manager.stoneList.Add(new StoneHorizontal());
            tmpV = -2.78f;
            for (int j = 0;j < h;j++)
            {
                var stonePos = new GameObject("StonePos");
                stonePos.transform.position = new Vector3(tmpV, 0.48f, verticalPos.transform.position.z);

                stonePos.transform.parent = verticalPos.transform;
                manager.stoneList[i].stoneList.Add(stonePos);
                tmpV += 0.46f;
                if (j == 5 || j == 7)
                    tmpV += 0.02f;
            }

            verticalPos.transform.parent = manager.transform;

            tmpH += 0.46f;
            if (i == 5 || i == 7)
                tmpH += 0.02f;
        }
    }

#endif


}

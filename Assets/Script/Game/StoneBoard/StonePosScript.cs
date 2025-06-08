using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class StonePosScript : MonoBehaviour
{
    [SerializeField, ReadOnly]
    Vector2Int position = Vector2Int.zero;

    [SerializeField, ReadOnly]
    GameObject putStoneObject = null;

    GameManager manager = null;

    [SerializeField]
    GameObject selectEnableObject = null, selectObject = null;

    public void Init(GameManager _manager, Vector2Int _pos)
    {
        manager = _manager;
        position = _pos;
    }

    public bool IsPutStone() { return putStoneObject != null; }

    public bool IsSelectPos() { return selectObject.activeSelf; }

    public void PushEvent()
    { 
        manager.SelectStonePos(position.x, position.y);
    }

    public void PutStone(GameObject _prefab)
    {
        if (_prefab == null) return;
        putStoneObject = Instantiate(_prefab, transform);

        InitializeObject(putStoneObject);
    }

    public void RemovePutStone()
    {
        if (putStoneObject == null) return;
        Destroy(putStoneObject);
    }

    public void SelectEnable()
    {
        ObjectActivater(selectEnableObject, true);
    }

    public void SelectDisable()
    {
        ObjectActivater(selectObject, false);
        ObjectActivater(selectEnableObject, false);
    }

    public void SelectStonePos()
    {
        ObjectActivater(selectObject, true);
        ObjectActivater(selectEnableObject, false);
    }

    public void UnSelectStonePos()
    {
        ObjectActivater(selectObject, false);
        ObjectActivater(selectEnableObject, true);
    }

    private void ObjectActivater(GameObject _obj,bool _flg)
    {
        if (_obj == null) return;
        _obj.SetActive(_flg);
    }

    void InitializeObject(GameObject _obj)
    {
        if (_obj == null) return;
        _obj.transform.localPosition = Vector3.zero;
        _obj.transform.localRotation = Quaternion.identity;
        _obj.transform.localScale = Vector3.one;
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class StonePosScript : MonoBehaviour
{
    public Vector2Int position { get; private set; } = Vector2Int.zero;

    [SerializeField, ReadOnly]
    GameObject putStoneObject = null;

    GameManager manager = null;

    [System.Serializable]
    public class ActiveFlagObject
    {
        public GameObject obj = null;

        [SerializeField,ReadOnly]
        public bool activeFlg = false;
    }
    
        [SerializeField,ReadOnly]
    ActiveFlagObject selectEnableObject = new ActiveFlagObject(),
        selectObject = new ActiveFlagObject();


    public void Init(GameManager _manager, Vector2Int _pos)
    {
        manager = _manager;
        position = _pos;
    }

    public bool IsPutStone() { return putStoneObject != null; }

    public bool IsSelectPos() { return selectObject.activeFlg; }

    public bool IsSelectEnable() { return selectEnableObject.activeFlg; }

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
        if (selectObject.activeFlg) return;
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

    private void ObjectActivater(ActiveFlagObject _obj,bool _flg)
    {
        if (_obj == null) return;
        if (_obj.obj == null) return;
        if (_obj.activeFlg == _flg) return;
        _obj.activeFlg = _flg;
    }

    private void UpdateObjectActivater(ActiveFlagObject _obj)
    {
        if (_obj == null) return;
        if (_obj.obj == null) return;
        if (_obj.activeFlg == _obj.obj.activeSelf) return;
        _obj.obj.SetActive(_obj.activeFlg);
    }

    void InitializeObject(GameObject _obj)
    {
        if (_obj == null) return;
        _obj.transform.localPosition = Vector3.zero;
        _obj.transform.localRotation = Quaternion.identity;
        _obj.transform.localScale = Vector3.one;
    }


    private void Update()
    {
        UpdateObjectActivater(selectObject);
        UpdateObjectActivater(selectEnableObject);
    }
}

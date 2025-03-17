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
    ParticleSystem system = null;

    public void Init(GameManager _manager, Vector2Int _pos)
    {
        manager = _manager;
        position = _pos;
        if (system == null) return;
        system.Stop();
    }

    public bool IsPutStone() { return putStoneObject != null; }

    public bool IsSelectPos() { return system.isPlaying; }

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

    public void SelectStonePos()
    {
        if (system == null) return;

        system.Play();
    }

    public void UnSelectStonePos()
    {
        if (system == null) return;
        system.Stop();
    }

    void InitializeObject(GameObject _obj)
    {
        if (_obj == null) return;
        _obj.transform.localPosition = Vector3.zero;
        _obj.transform.localRotation = Quaternion.identity;
        _obj.transform.localScale = Vector3.one;
    }

}

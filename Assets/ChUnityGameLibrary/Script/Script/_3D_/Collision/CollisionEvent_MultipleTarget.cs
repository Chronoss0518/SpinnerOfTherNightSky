using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent_MultipleTarget : CollisionEventBase
{
    public List<GameObject> targetObjectList = new List<GameObject>();

    public void AddTargetObject(GameObject _obj)
    {
        if (_obj == null) return;
        targetObjectList.Add(_obj);
    }

    public void RemoveTargetObject(GameObject _obj)
    {
        if (_obj == null) return;
        targetObjectList.Remove(_obj);
    }

    protected override bool IsTargetObject(GameObject _obj)
    {
        bool findTargetFlg = false;

        foreach(var obj in targetObjectList)
        {
            if (!ReferenceEquals(obj,_obj)) continue;
            findTargetFlg = true;
            break;
        }

        return findTargetFlg;
    }

    void OnCollisionEnter(Collision collision)
    {
        base.CollisionEnter(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        base.CollisionStay(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        base.CollisionExit(collision);
    }

    void OnTriggerEnter(Collider collision)
    {
        base.TriggerEnter(collision);
    }

    void OnTriggerStay(Collider collision)
    {
        base.TriggerStay(collision);
    }

    void OnTriggerExit(Collider collision)
    {
        base.TriggerExit(collision);
    }

}

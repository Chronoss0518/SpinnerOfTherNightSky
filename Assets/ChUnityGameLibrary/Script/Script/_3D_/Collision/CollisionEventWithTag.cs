using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEventWithTag : CollisionEventBase
{
    public string tagName = "";

    protected override bool IsTargetObject(GameObject _obj)
    {
        return _obj.tag == tagName;
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

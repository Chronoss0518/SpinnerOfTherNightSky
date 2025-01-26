using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionType
{
    CollisionEnter,
    CollisionStay,
    CollisionExit,
    TriggerEnter,
    TriggerStay,
    TriggerExit
}

public abstract class CollisionEventBase : MonoBehaviour
{
    public CollisionType type = CollisionType.CollisionEnter;

    public UnityEngine.Events.UnityEvent action = new UnityEngine.Events.UnityEvent();

    public bool moveSurfaceFlg = false;

    Vector3 hitPosition = Vector3.zero;

    Quaternion hitObjectDir = Quaternion.identity;

    public Vector3 getHitPos { get { return hitPosition; } }

    public Quaternion hitObjectDirection { get { return hitObjectDir; } }

    abstract protected bool IsTargetObject(GameObject _obj);


    void SetHitPosition(Collision _col)
    {
        float near = 100000;
        foreach (var contact in _col.contacts)
        {
            Vector3 hitPointVector = contact.point - transform.position;
            float distance = Mathf.Abs(hitPointVector.x) + Mathf.Abs(hitPointVector.y) + Mathf.Abs(hitPointVector.z);
            if (distance >= near) return;
            near = distance;
            hitPosition = contact.point;
            hitObjectDir = contact.thisCollider.transform.rotation;
        }

        if (moveSurfaceFlg)
        {
            transform.position = hitPosition;
        }
    }

    void SetHitPosition(Collider _col)
    {
        Vector3 hitPointVector = _col.ClosestPoint(transform.position);
        hitPosition = hitPointVector;
    }

    void EventUpdate(GameObject _obj)
    {
        if (!IsTargetObject(_obj)) return;
        action.Invoke();
    }

    protected void CollisionEnter(Collision collision)
    {
        if (type != CollisionType.CollisionEnter) return;
        SetHitPosition(collision);
        EventUpdate(collision.gameObject);
    }

    protected void CollisionStay(Collision collision)
    {
        if (type != CollisionType.CollisionStay) return;
        SetHitPosition(collision);
        EventUpdate(collision.gameObject);
    }

    protected void CollisionExit(Collision collision)
    {
        if (type != CollisionType.CollisionExit) return;
        SetHitPosition(collision);
        EventUpdate(collision.gameObject);
    }

    protected void TriggerEnter(Collider collision)
    {
        if (type != CollisionType.TriggerEnter) return;
        SetHitPosition(collision);
        EventUpdate(collision.gameObject);
    }

    protected void TriggerStay(Collider collision)
    {
        if (type != CollisionType.TriggerStay) return;
        SetHitPosition(collision);
        EventUpdate(collision.gameObject);
    }

    protected void TriggerExit(Collider collision)
    {
        if (type != CollisionType.TriggerExit) return;
        SetHitPosition(collision);
        EventUpdate(collision.gameObject);
    }
}

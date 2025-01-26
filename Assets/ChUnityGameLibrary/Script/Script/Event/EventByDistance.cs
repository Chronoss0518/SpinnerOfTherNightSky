using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventByDistance : MonoBehaviour
{

    public float distance = 0.0f;

    public GameObject targetObject = null;

    public UnityEngine.Events.UnityEvent action = new UnityEngine.Events.UnityEvent();

    public bool nearActFlg = false;

    bool playFlg = false;

    public void ReSet()
    {
        playFlg = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playFlg) return;

        Vector3 tmp = (targetObject != null ? targetObject.transform.position : Vector3.zero);

        Vector3 pos = tmp - gameObject.transform.position;

        if(nearActFlg && (Mathf.Abs(pos.x) + Mathf.Abs(pos.y) + Mathf.Abs(pos.z) > distance)) return;

        if(!nearActFlg && (Mathf.Abs(pos.x) + Mathf.Abs(pos.y) + Mathf.Abs(pos.z) < distance)) return;


        action.Invoke();

        playFlg = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookObjectEventWithOcclusionCulling : MonoBehaviour
{

    static Vector3[] mulPos = new Vector3[8]
    {
        new Vector3(-1.0f,1.0f,1.0f),
        new Vector3(1.0f,1.0f,1.0f),
        new Vector3(-1.0f,-1.0f,1.0f),
        new Vector3(1.0f,-1.0f,1.0f),
        new Vector3(-1.0f,1.0f,-1.0f),
        new Vector3(1.0f,1.0f,-1.0f),
        new Vector3(-1.0f,-1.0f,-1.0f),
        new Vector3(1.0f,-1.0f,-1.0f)
    };

    public Camera baseCamera = null;

    public BoxCollider targetCollider = null;

    public List<BoxCollider> otherCollider = new List<BoxCollider>();



    // Update is called once per frame
    void Update()
    {

        if (baseCamera == null) return;
        if (targetCollider == null) return;

        Matrix4x4 viewMat = baseCamera.worldToCameraMatrix;

        Matrix4x4 projMat = GL.GetGPUProjectionMatrix(baseCamera.projectionMatrix, false);

        /*
        
        Matrix4x4 worldMat = targetList[i].transform.localToWorldMatrix;

        var col = targetList[i].transform.GetChild(0).GetComponent<BoxCollider>();

        for (int j = 0; j<mulPos.Length; j++)
        {
            Vector4 pos = Vector4.zero;
            Vector3 worldPos = Vector3.zero;

            pos = mulPos[j];
            pos.x *= col.size.x / 2.0f;
            pos.y *= col.size.y / 2.0f;
            pos.z *= col.size.z / 2.0f;

            pos.x += col.center.x;
            pos.y += col.center.y;
            pos.z += col.center.z;
            pos.w = 1.0f;

            pos = worldMat * pos;
            worldPos = pos;
            pos = viewMat * pos;
            pos = projMat * pos;

            float z = pos.z / pos.w;

            if (Mathf.Abs(pos.x / pos.w) > 1.0f ||
                Mathf.Abs(pos.y / pos.w) > 1.0f ||
                z > 1.0f || z  < 0.0f) continue;


            RaycastHit[] hit;

            Vector3 tmpPos = targetCamera.transform.position;

            Vector3 tmp = worldPos - targetCamera.transform.position;

            hit = Physics.RaycastAll(tmpPos, tmp.normalized);

            if (hit.Length <= 0) continue;

            float minLen = 10000.0f;


            GameObject target = null;

            for (int k = 0; k<hit.Length; k++)
            {
                if (hit[k].collider.gameObject.tag == "RootObject") continue;
                if (hit[k].collider.name == move.name) continue;


                if (minLen < hit[k].distance) continue;

                minLen = hit[k].distance;
                target = hit[k].collider.gameObject;

            }


            if (target.name != targetList[i].name) continue;

            lookTarget.Add(targetList[i]);

            break;

        }
        */



    }
}

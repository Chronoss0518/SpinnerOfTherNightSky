using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Transform
{
    public class ObjectLook : Common.GameObjectTargetSelector
    {
        // Update is called once per frame
        void Update()
        {
            if (targetObject == null) return;

            transform.transform.LookAt(targetObject.transform.position);
        }
    }
}
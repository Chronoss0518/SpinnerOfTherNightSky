using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.ObjectControl
{
    public class DeleteThisObject : Common.GameObjectTargetSelector
    {
        public void Delete()
        {
            Destroy(target);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (target != null) return;

            target = gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            Start();
        }
    }

}
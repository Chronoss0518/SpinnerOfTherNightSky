using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Transform
{
    public class RotateCursol : Common.GameObjectTargetSelector
    {

        public float moveSize = 0.0f;

        Vector3 nowPos;

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

            var pos = UnityEngine.Input.mousePosition;

            if (target != null)
            {

                var rotSize = pos - nowPos;

                rotSize *= moveSize;

                var rot = Quaternion.Euler(-rotSize.y, rotSize.x, 0.0f);

                Rigidbody body = target.GetComponent<Rigidbody>();

                if (body != null) body.MoveRotation(rot * target.transform.rotation);
                else target.transform.rotation = rot * target.transform.rotation;


            }

            nowPos = pos;

        }
    }

}

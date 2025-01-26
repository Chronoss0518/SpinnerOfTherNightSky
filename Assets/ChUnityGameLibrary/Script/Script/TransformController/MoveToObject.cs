using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Transform
{

    public class MoveToObject : Common.GameObjectTargetSelector
    {
        [SerializeField]
        private GameObject moveToTarget = null;

        public GameObject moveTarget
        {
            get { return moveToTarget; }
            set
            {
                if (value == null) return;
                moveToTarget = value;
            }
        }

        [SerializeField]
        private float moveSpeed = 0.0f;

        public float speed
        {
            get { return moveSpeed; }
            set
            {
                if (value < 0.0f) return;
                moveSpeed = value;
            }
        }

        [SerializeField]
        AxisFlg axisFlg = AxisFlg.X | AxisFlg.Y | AxisFlg.Z;

        private void Start()
        {
            if (target == null)
            {
                target = (gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (moveToTarget == null) return;

            Vector3 dir = moveToTarget.transform.position - target.transform.position;

            dir.x = AxisFlgController.IsAxisFlg(axisFlg, AxisFlg.X) ? dir.x : 0.0f;
            dir.y = AxisFlgController.IsAxisFlg(axisFlg, AxisFlg.Y) ? dir.y : 0.0f;
            dir.z = AxisFlgController.IsAxisFlg(axisFlg, AxisFlg.Z) ? dir.z : 0.0f;

            dir.Normalize();

            var rd = GetComponent<Rigidbody>();

            if (rd != null) rd.MovePosition(target.transform.position + (dir * moveSpeed));
            else target.transform.position += (dir * moveSpeed);
        }
    }

}
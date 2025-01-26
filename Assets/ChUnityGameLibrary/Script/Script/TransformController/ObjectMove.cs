using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Transform
{

    public class ObjectMove : Common.GameObjectTargetSelector
    {
        [SerializeField]
        private float moveLen = 0.0f;

        public float len
        {
            get { return moveLen; }
            set
            {
                if (value < 0.0f) return;
                moveLen = value;
            }
        }

        public bool normalizeFlg = true;

        public bool moveDirFlg = true;

        private Vector3 dir = Vector3.zero;

        public Vector3 autoMoveDir = Vector3.zero;

        public void AddMoveLen(float _mLen) { moveLen += _mLen; }

        public void SubMoveLen(float _mLen) { moveLen -= _mLen; }

        public void MulMoveLen(float _mLen) { moveLen *= _mLen; }

        public void DivMoveLen(float _mLen) { moveLen /= _mLen != 0.0f ? _mLen : 1.0f; }

        public void SetMoveLen(float _mLen) { moveLen = _mLen; }

        public float GetMoveLen() { return moveLen; }

        public void MoveFoward()
        {
            dir += Vector3.forward;
        }

        public void MoveBack()
        {
            dir += Vector3.back;
        }

        public void MoveRight()
        {
            dir += Vector3.right;
        }

        public void MoveLeft()
        {
            dir += Vector3.left;
        }
        public void MoveUp()
        {
            dir += Vector3.up;
        }

        public void MoveDown()
        {
            dir += Vector3.down;
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

            if (target != null)
            {

                if (moveDirFlg) dir = target.transform.rotation * dir;
                if (normalizeFlg) dir.Normalize();

                dir *= moveLen;

                Rigidbody body = target.GetComponent<Rigidbody>();

                if (body != null) body.MovePosition(dir + target.transform.position);
                else target.transform.position += dir;

            }

            dir = autoMoveDir.normalized;

        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Transform
{

    public class ObjectScale : Common.GameObjectTargetSelector
    {
        public float scaleSize = 0.0f;

        private Vector3 scale = Vector3.zero;

        protected void ZeroTest()
        {
            var tmpScale = transform.localScale;

            tmpScale.x  = transform.localScale.x > 0.0f ? transform.localScale.x : 0.0f;
            tmpScale.y  = transform.localScale.y > 0.0f ? transform.localScale.y : 0.0f;
            tmpScale.z  = transform.localScale.z > 0.0f ? transform.localScale.z : 0.0f;

            transform.localScale = tmpScale;
        }

        public void AddMoveSize(float _mSize) { scaleSize += _mSize; }

        public void SubMoveSize(float _mSize) { scaleSize -= _mSize; }

        public void MulMoveSize(float _mSize) { scaleSize *= _mSize; }

        public void DivMoveSize(float _mSize) { scaleSize /= _mSize != 0.0f ? _mSize : 1.0f; }

        public void SetMoveSize(float _mSize) { scaleSize = _mSize; }

        public float GetMoveSize() { return scaleSize; }

        public void MoveFoward()
        {
            scale += Vector3.forward;
        }

        public void MoveBack()
        {
            scale += Vector3.back;
        }

        public void MoveRight()
        {
            scale += Vector3.right;
        }

        public void MoveLeft()
        {
            scale += Vector3.left;
        }
        public void MoveUp()
        {
            scale += Vector3.up;
        }

        public void MoveDown()
        {
            scale += Vector3.down;
        }

        public void MoveFloatSize(float _mSize)
        {
            scale += new Vector3(_mSize, _mSize, _mSize);
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
                Rigidbody body = target.GetComponent<Rigidbody>();

                if (body != null) body.transform.localScale = scale;
                else target.transform.localScale = scale;
            }

            ZeroTest();

            scale = Vector3.zero;

        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Transform
{

    public enum Axis
    {
        none, px, py, pz, mx, my, mz
    }

    public class ObjectRotate : Common.GameObjectTargetSelector
    {
        public float rotSize = 0.0f;

        private Quaternion rot = Quaternion.identity;

        public Axis autoRotateAxis = Axis.none;

        public void AddRotSize(float _rot) { rotSize += _rot; }

        public void SubRotSize(float _rot) { rotSize -= _rot; }

        public void MulRotSize(float _rot) { rotSize *= _rot; }

        public void DivRotSize(float _rot) { rotSize /= _rot != 0.0f ? _rot : 1.0f; }

        public void SetRotSize(float _rot) { rotSize = _rot; }

        public float GetRotSize() { return rotSize; }

        public void RotAxisZ()
        {
            rot = Quaternion.Euler(0.0f, 0.0f, rotSize) * rot;
        }

        public void RotAxisZInverse()
        {
            rot = Quaternion.Euler(0.0f, 0.0f, -rotSize) * rot;
        }

        public void RotAxisX()
        {
            rot = Quaternion.Euler(rotSize, 0.0f, 0.0f) * rot;
        }

        public void RotAxisXInverse()
        {
            rot = Quaternion.Euler(-rotSize, 0.0f, 0.0f) * rot;
        }

        public void RotAxisY()
        {
            rot = Quaternion.Euler(0.0f, rotSize, 0.0f) * rot;
        }

        public void RotAxisYInverse()
        {
            rot = Quaternion.Euler(0.0f, -rotSize, 0.0f) * rot;
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
                switch (autoRotateAxis)
                {
                    case Axis.px:
                        RotAxisX();
                        break;
                    case Axis.py:
                        RotAxisY();
                        break;
                    case Axis.pz:
                        RotAxisZ();
                        break;
                    case Axis.mx:
                        RotAxisXInverse();
                        break;
                    case Axis.my:
                        RotAxisYInverse();
                        break;
                    case Axis.mz:
                        RotAxisZInverse();
                        break;
                }


                Rigidbody body = target.GetComponent<Rigidbody>();

                if (body != null) body.MoveRotation(rot * target.transform.rotation);
                else target.transform.rotation = rot * target.transform.rotation;

            }

            rot = Quaternion.identity;

        }
    }

}
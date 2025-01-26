using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.ObjectControl
{
    public class LoadGameObject : Common.GameObjectTargetSelector
    {
        [SerializeField]
        private GameObject baseObejct = null;

        public GameObject baseGameObject
        {
            get { return baseObejct; }
            set
            {
                if (value == null) return;
                baseObejct = value;
            }
        }

        public void LoadResourceObject(string _path)
        {
            var obj = (GameObject)Resources.Load(_path);

            Instantiate(obj);

        }

        public void LoadResourceObject(string _path, Vector3 _positino, Quaternion _rotation)
        {
            var obj = (GameObject)Resources.Load(_path);

            Instantiate(obj, _positino, _rotation);

        }

        public void LoadResourceObject(string _path, GameObject _targetObjectPosition)
        {
            var obj = (GameObject)Resources.Load(_path);

            Instantiate(obj, _targetObjectPosition.transform);

        }


        public void LoadObject()
        {
            if (target == null) return;

            Instantiate(target);
        }

        public void LoadObject(GameObject _targetObjectPosition)
        {
            if (target == null) return;

            Instantiate(target, _targetObjectPosition.transform);
        }

        public void LoadObjectUnChild(GameObject _targetObjectPosition)
        {
            if (target == null) return;

            Instantiate(target, _targetObjectPosition.transform.position, _targetObjectPosition.transform.rotation);
        }

        private void Start()
        {
            if (baseObejct != null) return;
            baseObejct = gameObject;
        }

        private void Update()
        {
            Start();
        }

        public void LoadObjectBaseUnChild()
        {
            if (target == null) return;
            if (baseObejct == null) return;

            Instantiate(target, baseObejct.transform.position, baseObejct.transform.rotation);

        }

        public void LoadObjectBaseChild()
        {
            if (target == null) return;
            if (baseObejct == null) return;

            Instantiate(target, baseObejct.transform);

        }

    }

}

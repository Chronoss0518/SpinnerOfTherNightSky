using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Common
{
    public class GameObjectTargetSelector : MonoBehaviour
    {

        [SerializeField]
        protected GameObject targetObject = null;

        public GameObject target 
        {
            get { return targetObject; }
            set
            {
                if (value == null) return;
                targetObject = value;
            }
        }

        public void SetParentObject()
        {
            targetObject = transform.parent.gameObject;
        }

        private GameObject GetParentObject(GameObject _obj)
        {
            if (_obj.transform.parent is null) return _obj;

            return GetParentObject(_obj.transform.parent.gameObject);

        }

        public void SetTopParentObject()
        {
            targetObject =  GetParentObject(gameObject);
        }

        public void SetTopObjectWithTag(string _tag)
        {
            targetObject = GameObject.FindGameObjectWithTag(_tag);
        }

        public void SetTopObjectWithName(string _name)
        {
            targetObject = GameObject.Find(_name);
        }

        private GameObject GetFirstParentObjectWithTag(GameObject _obj, string _tag)
        {
            if (_obj.transform.parent is null) return _obj;
            if (_obj.tag == _tag) return _obj;

            return GetParentObject(_obj.transform.parent.gameObject);

        }

        public void SetFirstParentObjectWithTag(string _tag)
        {
            targetObject =  GetFirstParentObjectWithTag(gameObject, _tag);
        }

        private GameObject GetFirstParentObjectWithName(GameObject _obj, string _name)
        {
            if (_obj.transform.parent is null) return _obj;
            if (_obj.name == _name) return _obj;

            return GetParentObject(_obj.transform.parent.gameObject);

        }

        public void SetFirstParentObjectWithName(string _name)
        {
            targetObject =  GetFirstParentObjectWithName(gameObject, _name);
        }

    }

}

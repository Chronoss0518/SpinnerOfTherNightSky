using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.ObjectControl
{
    public class ObjectActivity : Common.GameObjectTargetSelector
    {
        bool flg = false;
        public void SetActivity(bool _flg)
        {
            if (target == null) return;
            flg = _flg;
            target.SetActive(_flg);
        }

        public void SetActive()
        {
            if (target == null) return;
            flg = true;
            target.SetActive(true);
        }

        public void SetUnActive()
        {
            if (target == null) return;
            flg = false;
            target.SetActive(false);
        }

        public void ActiveFlgInverce()
        {
            if (target == null) return;
            flg = !flg;
            target.SetActive(flg);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (target == null)
            {
                target = gameObject;
            }

            if (target == null) return;

            flg = target.activeSelf;
        }

        // Update is called once per frame
        void Update()
        {
            Start();
            
            

        }
    }

}

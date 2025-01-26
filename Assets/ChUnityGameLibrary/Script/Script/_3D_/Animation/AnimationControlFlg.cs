using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity._3D_
{
    public class AnimationControlFlg : AnimationControllerBase
    {
        private bool flg = false;

        public string flgName = "";

        public void SetFlg(bool _flg)
        {
            flg = _flg;
        }

        public void SetTrue() { flg = true; }

        public void SetFalse() { flg = false; }



        // Start is called before the first frame update
        override protected void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();

            if (string.IsNullOrWhiteSpace(flgName)) return;
            if (!IsInit()) return;

            SetBool(flgName, flg);

        }
    }

}

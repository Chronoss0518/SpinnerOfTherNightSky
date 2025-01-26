using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity._3D_
{
    public class AnimationControlInt : AnimationControllerBase
    {
        private int val = 0;

        public string flgName = "";

        public void SetValue(in int _val)
        {
            val = _val;
        }

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

            SetInteger(flgName, val);

        }
    }

}

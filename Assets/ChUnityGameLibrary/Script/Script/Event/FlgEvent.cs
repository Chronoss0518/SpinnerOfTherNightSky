using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Common.Event
{

    public class FlgEvent : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent action = new UnityEngine.Events.UnityEvent();

        public bool actionFlg = false;

        public void SetFlg(bool _flg)
        {
            actionFlg = _flg;
        }

        public void SetTrue()
        {
            actionFlg = true;
        }

        public void SetFalse()
        {
            actionFlg = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!actionFlg) return;

            action.Invoke();

            actionFlg = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Common.Event
{

    public class SwitchEvent : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent on = new UnityEngine.Events.UnityEvent();
        public UnityEngine.Events.UnityEvent off = new UnityEngine.Events.UnityEvent();

        public bool switchFlg = false;
        bool nowFlg = false;

        public void SetFlg(bool _flg)
        {
            switchFlg = _flg;
        }

        public void SetTrue()
        {
            switchFlg = true;
        }

        public void SetFalse()
        {
            switchFlg = false;
        }
        public void FlgInverse()
        {
            switchFlg = !switchFlg;
        }

        void Start()
        {
            nowFlg = switchFlg;
        }

        // Update is called once per frame
        void Update()
        {
            if (switchFlg == nowFlg) return;

            if(switchFlg)on.Invoke();
            else off.Invoke();

            nowFlg = switchFlg;
        }
    }
}

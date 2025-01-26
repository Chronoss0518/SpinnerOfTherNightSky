using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Common
{
    public class CountDownEvent : MonoBehaviour
    {
        public ulong maxCount = 0;

        public bool countingEventFlg = true;

        public UnityEngine.Events.UnityEvent action = new UnityEngine.Events.UnityEvent();

        ulong count = 0;
        ulong nowCount = 0;

        public ulong getNowCount { get { return nowCount; } }

        public void CountDown() { count = count > 0 ? count - 1 : 0; }

        public void ReSet()
        {
            Start();
        }

        // Start is called before the first frame update
        void Start()
        {
            count = maxCount;
            nowCount = count;
        }

        // Update is called once per frame
        void Update()
        {
            if (nowCount == count) return;
            if ((count <= 0 && countingEventFlg) || (count > 0 && !countingEventFlg)) return;
            nowCount = count;
            action.Invoke();
        }
    }
}
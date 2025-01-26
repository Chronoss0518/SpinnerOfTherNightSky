using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Common.Event
{

    public class AlarmEvent : MonoBehaviour
    {
        public int alarmSeconds = 3;

        public UnityEngine.Events.UnityEvent action = new UnityEngine.Events.UnityEvent();

        float alarmCount = 0.0f;
        float beforeTimeCount = 0.0f;

        bool startFlg = false;

        public int nowSeconds { get { return (int)alarmCount; } }
        public int nowMiliseconds { get { return (int)(alarmCount * 1000.0f); } }

        public void StartTimer()
        {
            if (startFlg) return;
            beforeTimeCount = Time.realtimeSinceStartup;
            startFlg = true;
        }

        void TimeCount()
        {
            float tmp = Time.realtimeSinceStartup;
            alarmCount += tmp - beforeTimeCount;
            beforeTimeCount = tmp;
        }

        public void PauseTimer()
        {
            startFlg = false;
        }

        public void StopTimer()
        {
            startFlg = false;
            alarmCount = 0.0f;
        }

        public void StartPause()
        {

            if (startFlg) return;
            if (alarmCount <= 0.0f) return;
            
            beforeTimeCount = Time.realtimeSinceStartup;
            startFlg = true;
        }

        // Update is called once per frame
        void Update()
        {


            if (!startFlg) return;

            TimeCount();

            if (alarmCount < alarmSeconds) return;

            action.Invoke();
            alarmCount = 0.0f;
            startFlg = false;
        }
    }
}

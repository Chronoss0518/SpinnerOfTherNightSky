using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Event
{

    public class ExistGameObjectEvent : Common.GameObjectTargetSelector
    {

        public UnityEngine.Events.UnityEvent trueEvent = new UnityEngine.Events.UnityEvent();
        public UnityEngine.Events.UnityEvent falseEvent = new UnityEngine.Events.UnityEvent();

        public void RunEvent()
        {
            if (target != null)
            {
                trueEvent.Invoke();
            }
            else
            {
                falseEvent.Invoke();
            }
        }

    }

}
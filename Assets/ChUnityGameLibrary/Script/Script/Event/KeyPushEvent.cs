using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChUnity.Event
{
    public enum InputType:int
    {
        Push,//‰Ÿ‚³‚ê‚Ä‚¢‚éŠÔ//
        Down,//‰Ÿ‚³‚ê‚½Žž//
        Up//—£‚µ‚½Žž//
    }


    public class KeyPushEvent : MonoBehaviour
    {
        public InputType type = InputType.Down;
        public KeyCode key = KeyCode.None;

        public UnityEngine.Events.UnityEvent action = new UnityEngine.Events.UnityEvent();

        delegate bool InputTypeFlg(KeyCode _key);

        static private bool GetPushKeyFlg(KeyCode _key)
        {
            return UnityEngine.Input.GetKey(_key);
        }

        static private bool GetKeyDownFlg(KeyCode _key)
        {
            return UnityEngine.Input.GetKeyDown(_key);
        }

        static private bool GetKeyUpFlg(KeyCode _key)
        {
            return UnityEngine.Input.GetKeyUp(_key);
        }

        InputTypeFlg[] GetFlgs = 
        { 
            GetPushKeyFlg,
            GetKeyDownFlg,
            GetKeyUpFlg
        };


        // Update is called once per frame
        void Update()
        {
            if (action.GetPersistentEventCount() <= 0) return;

            if (!GetFlgs[(int)type](key)) return;

            action.Invoke();

        }
    }

}

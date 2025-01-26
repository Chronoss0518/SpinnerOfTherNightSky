/**
* @file LifePointEditor.cs
* @brief LifePointスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/01/02
* @details LifePointスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   Custom InspectorBaseを継承したLifePointのCustomInspector
*/

namespace ChUnity.Event
{
    [CustomEditor(typeof(CounterEvent))]
    public class CounterEventEditor : CustomInspectorBase
    {
        SerializedProperty counter;

        SerializedProperty loopFlg;
        SerializedProperty loopMaxCount;

        SerializedProperty countAction;

        void OnEnable()
        {
            counter = serializedObject.FindProperty("counter");

            loopFlg = serializedObject.FindProperty("loopFlg");
            loopMaxCount = serializedObject.FindProperty("loopMaxCount");

            countAction = serializedObject.FindProperty("countAction");
        }

        public override void UpdateInspectorGUI()
        {
            
            InputField(counter, "counter");

            InputField(loopFlg, "loopFlg");

            if (loopFlg.boolValue)
            {
                InputField(loopMaxCount, "loopMaxCount");
            }

            InputField(countAction, "useCountAction");

        }

    }

}

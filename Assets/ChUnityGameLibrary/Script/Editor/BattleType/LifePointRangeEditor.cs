using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChUnity.BattleType
{
    [CustomEditor(typeof(LifePointRange))]
    public class LifePointRangeEditor : CustomInspectorBase
    {
        SerializedProperty lMaxLP;
        SerializedProperty hMaxLP;
        void OnEnable()
        {
            lMaxLP = serializedObject.FindProperty("lMaxLP");
            hMaxLP = serializedObject.FindProperty("hMaxLP");
        }

        public override void UpdateInspectorGUI()
        {
            BeginHorizontal();
            InputField(lMaxLP, "Low:");
            EndHorizontal();

            BeginHorizontal();
            InputField(hMaxLP, "High:");
            EndHorizontal();

        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChUnity.BattleType
{
    [CustomEditor(typeof(AttackPointRange))]
    public class AttackPointRangeEditor : CustomInspectorBase
    {
        SerializedProperty lATK;
        SerializedProperty hATK;
        void OnEnable()
        {
            lATK = serializedObject.FindProperty("lATK");
            hATK = serializedObject.FindProperty("hATK");
        }

        public override void UpdateInspectorGUI()
        {
            BeginHorizontal();
            InputField(lATK, "Low:");
            EndHorizontal();

            BeginHorizontal();
            InputField(hATK, "High:");
            EndHorizontal();

        }

    }
}
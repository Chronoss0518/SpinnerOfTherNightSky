/**
* @file GameObjectTargetSelectorEditor.cs
* @brief GameObjectTargetSelectorスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/03/29
* @details GameObjectTargetSelectorスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   Custom InspectorBaseを継承したLifePointのCustomInspector
*/

namespace ChUnity.Common
{
    [CustomEditor(typeof(GameObjectTargetSelector))]
    public class GameObjectTargetSelectorEditor : CustomInspectorBase
    {
        protected string targetObjectDescription = "操作対象のオブジェクト";
        SerializedProperty targetObject;

        protected virtual void OnEnable()
        {
            targetObject = serializedObject.FindProperty("targetObject");
        }

        public override void UpdateInspectorGUI()
        {
            InputField(targetObject, targetObjectDescription);
        }

    }

}

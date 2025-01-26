/**
* @file ObjectRotateEditor.cs
* @brief ObjectRotateスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/03/29
* @details ObjectRotateスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditorを継承したObjectRotateEditor
*/

namespace ChUnity.Transform
{

    [CustomEditor(typeof(ObjectRotate))]
    public class ObjectRotateEditor : Common.GameObjectTargetSelectorEditor
    {

        SerializedProperty rotSize;

        SerializedProperty autoRotateAxis;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetObjectDescription = "回転するオブジェクト";

            rotSize = serializedObject.FindProperty("rotSize");

            autoRotateAxis = serializedObject.FindProperty("autoRotateAxis");
        }

        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(rotSize, "回転速度");

            InputField(autoRotateAxis, "自動で回転を行うフラグ");
        }
    }

}

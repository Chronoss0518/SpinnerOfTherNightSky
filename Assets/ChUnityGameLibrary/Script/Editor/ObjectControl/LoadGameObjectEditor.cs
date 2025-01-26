/**
* @file LoadGameObjectEditor.cs
* @brief LoadGameObjectスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/01/02
* @details LoadGameObjectスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditorを継承したLoadGameObjectEditor
*/

namespace ChUnity.ObjectControl
{


    [CustomEditor(typeof(LoadGameObject))]
    public class LoadGameObjectEditor : Common.GameObjectTargetSelectorEditor
    {
        SerializedProperty baseObejct;
        protected override void OnEnable()
        {
            base.OnEnable();

            targetObjectDescription = "生成するオブジェクト";
            baseObejct = serializedObject.FindProperty("baseObejct");
        }

        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(baseObejct, "オブジェクトを生成する位置の基本となるオブジェクト");
        }

    }

}

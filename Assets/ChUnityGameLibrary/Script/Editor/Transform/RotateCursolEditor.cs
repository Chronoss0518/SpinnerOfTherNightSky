/**
* @file RotateCursolEditor.cs
* @brief RotateCursolスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/03/29
* @details RotateCursolスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditorを継承したRotateCursolEditor
*/

namespace ChUnity.Transform
{

    [CustomEditor(typeof(RotateCursol))]
    public class RotateCursolEditor : Common.GameObjectTargetSelectorEditor
    {
        SerializedProperty moveSize;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetObjectDescription = "回転するオブジェクト";

            moveSize = serializedObject.FindProperty("moveSize");
        }

        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(moveSize, "回転する量");
        }
    }

}

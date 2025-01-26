/**
* @file MoveToObjectEditor.cs
* @brief MoveToObjectスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/03/29
* @details MoveToObjectスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditorを継承したMoveToObjectEditor
*/

namespace ChUnity.Transform
{

    [CustomEditor(typeof(MoveToObject))]
    public class MoveToObjectEditor : Common.GameObjectTargetSelectorEditor
    {

        SerializedProperty moveToTarget;
        SerializedProperty moveSpeed;

        SerializedProperty axisFlg;

        protected override void OnEnable()
        {
            base.OnEnable();

            targetObjectDescription = "移動するオブジェクト";

            moveToTarget = serializedObject.FindProperty("moveToTarget");
            moveSpeed = serializedObject.FindProperty("moveSpeed");

            axisFlg = serializedObject.FindProperty("axisFlg");
        }

        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(moveToTarget, "移動先のオブジェクト");

            InputField(moveSpeed, "移動速度");

            Line();

            BeginHorizontal();
            InputField(axisFlg, "移動軸");
            EndHorizontal();

            Line();
        }
    }

}

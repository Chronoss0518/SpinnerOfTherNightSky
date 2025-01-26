/**
* @file ObjectMoveEditor.cs
* @brief ObjectMoveスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/03/29
* @details ObjectMoveスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditorを継承したObjectMoveEditor
*/

namespace ChUnity.Transform
{

    [CustomEditor(typeof(ObjectMove))]
    public class ObjectMoveEditor : Common.GameObjectTargetSelectorEditor
    {
        SerializedProperty moveLen;
        
        SerializedProperty normalizeFlg;
        SerializedProperty moveDirFlg;

        SerializedProperty autoMoveDir;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetObjectDescription = "移動するオブジェクト";

            moveLen = serializedObject.FindProperty("moveLen");

            normalizeFlg = serializedObject.FindProperty("normalizeFlg");
            moveDirFlg = serializedObject.FindProperty("moveDirFlg");

            autoMoveDir = serializedObject.FindProperty("autoMoveDir");
        }

        /**
       * @fn public void OnInspectorGUI()
       * @brief InspectorのGUIを変更する関数。
       */
        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(moveLen, "移動速度");

            InputField(normalizeFlg, "移動する際に正規化(ベクトルの長さを1にすること)を行うかのフラグ");
            InputField(moveDirFlg, "移動する際に現在向いている方向を元に移動させるかのフラグ");


            Label("移動軸");
            InputField(autoMoveDir, "自動で移動する方向(大きさは実行時に1になります。)");

        }
    }

}

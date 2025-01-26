/**
* @file DeleteThisObjectEditor.cs
* @brief DeleteThisObjectスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/01/02
* @details DeleteThisObjectスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditorを継承したDeleteThisObjectEditor
*/

namespace ChUnity.ObjectControl
{


    [CustomEditor(typeof(DeleteThisObject))]
    public class DeleteThisObjectEditor : Common.GameObjectTargetSelectorEditor
    {

        protected override void OnEnable()
        {
            base.OnEnable();

            targetObjectDescription = "削除対象のオブジェクト";
        }

    }

}

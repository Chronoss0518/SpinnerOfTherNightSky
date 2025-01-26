/**
* @file RotateCursolEditor.cs
* @brief RotateCursol�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/03/29
* @details RotateCursol�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditor���p������RotateCursolEditor
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
            targetObjectDescription = "��]����I�u�W�F�N�g";

            moveSize = serializedObject.FindProperty("moveSize");
        }

        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(moveSize, "��]�����");
        }
    }

}

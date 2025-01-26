/**
* @file ObjectRotateEditor.cs
* @brief ObjectRotate�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/03/29
* @details ObjectRotate�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditor���p������ObjectRotateEditor
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
            targetObjectDescription = "��]����I�u�W�F�N�g";

            rotSize = serializedObject.FindProperty("rotSize");

            autoRotateAxis = serializedObject.FindProperty("autoRotateAxis");
        }

        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(rotSize, "��]���x");

            InputField(autoRotateAxis, "�����ŉ�]���s���t���O");
        }
    }

}

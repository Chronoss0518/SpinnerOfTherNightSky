/**
* @file MoveToObjectEditor.cs
* @brief MoveToObject�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/03/29
* @details MoveToObject�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditor���p������MoveToObjectEditor
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

            targetObjectDescription = "�ړ�����I�u�W�F�N�g";

            moveToTarget = serializedObject.FindProperty("moveToTarget");
            moveSpeed = serializedObject.FindProperty("moveSpeed");

            axisFlg = serializedObject.FindProperty("axisFlg");
        }

        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(moveToTarget, "�ړ���̃I�u�W�F�N�g");

            InputField(moveSpeed, "�ړ����x");

            Line();

            BeginHorizontal();
            InputField(axisFlg, "�ړ���");
            EndHorizontal();

            Line();
        }
    }

}

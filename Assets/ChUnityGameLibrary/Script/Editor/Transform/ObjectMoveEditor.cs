/**
* @file ObjectMoveEditor.cs
* @brief ObjectMove�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/03/29
* @details ObjectMove�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditor���p������ObjectMoveEditor
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
            targetObjectDescription = "�ړ�����I�u�W�F�N�g";

            moveLen = serializedObject.FindProperty("moveLen");

            normalizeFlg = serializedObject.FindProperty("normalizeFlg");
            moveDirFlg = serializedObject.FindProperty("moveDirFlg");

            autoMoveDir = serializedObject.FindProperty("autoMoveDir");
        }

        /**
       * @fn public void OnInspectorGUI()
       * @brief Inspector��GUI��ύX����֐��B
       */
        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(moveLen, "�ړ����x");

            InputField(normalizeFlg, "�ړ�����ۂɐ��K��(�x�N�g���̒�����1�ɂ��邱��)���s�����̃t���O");
            InputField(moveDirFlg, "�ړ�����ۂɌ��݌����Ă�����������Ɉړ������邩�̃t���O");


            Label("�ړ���");
            InputField(autoMoveDir, "�����ňړ��������(�傫���͎��s����1�ɂȂ�܂��B)");

        }
    }

}

/**
* @file AttackPointEditor.cs
* @brief LifePoint�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/01/02
* @details LifePoint�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ChUnity.BattleType
{
    [CustomEditor(typeof(AttackPoint))]
    public class AttackPointEditor : CustomInspectorBase
    {
        SerializedProperty atk;

        SerializedProperty range;
        int tmpAtk = 0;

        void OnEnable()
        {
            atk = serializedObject.FindProperty("atk");
            range = serializedObject.FindProperty("range");
        }

        public override void UpdateInspectorGUI()
        {

            Label("Set Attack Point Range");
            InputField(range, "");
            
            var obj = target as AttackPoint;

            if (!obj.isSetRange)
            {
                Label("AttackPointRange���Z�b�g����Ă��܂���");
                HelpBox("���j���[��Assets/Create/BattleTypeRange����AttackPoint��I��сA�쐬�����I�u�W�F�N�g��range�ɃZ�b�g���Ă��������B");
                return;
            }

            tmpAtk = Slider(atk.intValue, obj.lATK, obj.hATK, "Attack Point");
        }


        public override void EndInspectorGUI()
        {
            var obj = target as AttackPoint;

            obj.attackPoint = tmpAtk;
        }
    }

}


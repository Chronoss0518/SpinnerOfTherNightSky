/**
* @file ObjectScaleEditor.cs
* @brief ObjectScale�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/03/29
* @details ObjectScale�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditor���p������ObjectScaleEditor
*/

namespace ChUnity.Transform
{

    [CustomEditor(typeof(ObjectScale))]
    public class ObjectScaleEditor : Common.GameObjectTargetSelectorEditor
    {

        SerializedProperty scaleSize;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetObjectDescription = "�g�k���s���I�u�W�F�N�g";

            scaleSize = serializedObject.FindProperty("scaleSize");
        }

        /**
       * @fn public void OnInspectorGUI()
       * @brief Inspector��GUI��ύX����֐��B
       */
        public override void UpdateInspectorGUI()
        {
            base.UpdateInspectorGUI();

            InputField(scaleSize,"�g�k���s���T�C�Y","ObjectScale�𗘗p����ꍇ�͊�{�I��Method�𗘗p���Ċg�k���s�킹�܂��B");

        }
    }

}

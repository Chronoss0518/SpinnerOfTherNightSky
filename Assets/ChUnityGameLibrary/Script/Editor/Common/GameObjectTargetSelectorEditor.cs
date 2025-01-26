/**
* @file GameObjectTargetSelectorEditor.cs
* @brief GameObjectTargetSelector�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/03/29
* @details GameObjectTargetSelector�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   Custom InspectorBase���p������LifePoint��CustomInspector
*/

namespace ChUnity.Common
{
    [CustomEditor(typeof(GameObjectTargetSelector))]
    public class GameObjectTargetSelectorEditor : CustomInspectorBase
    {
        protected string targetObjectDescription = "����Ώۂ̃I�u�W�F�N�g";
        SerializedProperty targetObject;

        protected virtual void OnEnable()
        {
            targetObject = serializedObject.FindProperty("targetObject");
        }

        public override void UpdateInspectorGUI()
        {
            InputField(targetObject, targetObjectDescription);
        }

    }

}

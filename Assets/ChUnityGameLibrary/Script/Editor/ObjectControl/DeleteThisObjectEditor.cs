/**
* @file DeleteThisObjectEditor.cs
* @brief DeleteThisObject�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/01/02
* @details DeleteThisObject�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditor���p������DeleteThisObjectEditor
*/

namespace ChUnity.ObjectControl
{


    [CustomEditor(typeof(DeleteThisObject))]
    public class DeleteThisObjectEditor : Common.GameObjectTargetSelectorEditor
    {

        protected override void OnEnable()
        {
            base.OnEnable();

            targetObjectDescription = "�폜�Ώۂ̃I�u�W�F�N�g";
        }

    }

}

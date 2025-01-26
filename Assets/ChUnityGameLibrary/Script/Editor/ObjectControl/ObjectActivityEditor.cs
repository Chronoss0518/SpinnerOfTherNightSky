/**
* @file ObjectActivityEditor.cs
* @brief ObjectActivity�X�N���v�g��Inspector���J�X�^�}�C�Y��������
* @author Chronoss0518
* @date 2022/03/29
* @details ObjectActivity�X�N���v�g�𑀍삵�₷���悤��Inspector���J�X�^�}�C�Y��������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   GameObjectTargetSelectorEditor���p������ObjectActivityEditor
*/

namespace ChUnity.ObjectControl
{

    [CustomEditor(typeof(ObjectActivity))]
    public class ObjectActivityEditor : Common.GameObjectTargetSelectorEditor
    {

        protected override void OnEnable()
        {
            base.OnEnable();

            targetObjectDescription = "�L���E�����𑀍삷��ΏۃI�u�W�F�N�g";
        }

    }

}

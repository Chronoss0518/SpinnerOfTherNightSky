/**
* @file AttackPointEditor.cs
* @brief LifePointスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/01/02
* @details LifePointスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
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
                Label("AttackPointRangeがセットされていません");
                HelpBox("メニューのAssets/Create/BattleTypeRangeからAttackPointを選んび、作成したオブジェクトをrangeにセットしてください。");
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


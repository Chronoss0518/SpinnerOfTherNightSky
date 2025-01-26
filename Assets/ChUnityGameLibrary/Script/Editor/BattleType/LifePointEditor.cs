/**
* @file LifePointEditor.cs
* @brief LifePointスクリプトのInspectorをカスタマイズしたもの
* @author Chronoss0518
* @date 2022/01/02
* @details LifePointスクリプトを操作しやすいようにInspectorをカスタマイズしたもの
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* @brief   Custom InspectorBaseを継承したLifePointのCustomInspector
*/

namespace ChUnity.BattleType
{
    [CustomEditor(typeof(LifePoint))]
    public class LifePointEditor : CustomInspectorBase
    {
        SerializedProperty maxLP;
        SerializedProperty action;

        SerializedProperty range;
        int tmpMaxLP = 0;

        void OnEnable()
        {
            maxLP = serializedObject.FindProperty("maxLP");
            action = serializedObject.FindProperty("action");

            range = serializedObject.FindProperty("range");

        }

        public override void UpdateInspectorGUI()
        {
            Label("Set Life Point Range");
            InputField(range, "");

            var obj = target as LifePoint;

            if (!obj.isSetRange)
            {
                Label("LifePointRangeがセットされていません");
                HelpBox("メニューのAssets/Create/BattleTypeRangeからLifePointを選んび、作成したオブジェクトをrangeにセットしてください。");
                return;
            }

            tmpMaxLP = Slider(maxLP.intValue, obj.lMaxLP, obj.hMaxLP, "Max Life Point");

            InputField(action, "On Death Action");

        }


        public override void EndInspectorGUI()
        {
            var obj = target as LifePoint;

            obj.maxLifePoint = tmpMaxLP;
        }
    }

}

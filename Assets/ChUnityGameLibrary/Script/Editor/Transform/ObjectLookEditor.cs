using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ChUnity.Transform
{

    [CustomEditor(typeof(ObjectLook))]
    public class ObjectLookEditor : Common.GameObjectTargetSelectorEditor
    {

        protected override void OnEnable()
        {
            base.OnEnable();

            targetObjectDescription = "このオブジェクトが向く先のオブジェクト";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

abstract public class ZoneScriptBase : MonoBehaviour
{
    abstract public void SelectTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer);
    abstract public void SelectTargetDown();

}

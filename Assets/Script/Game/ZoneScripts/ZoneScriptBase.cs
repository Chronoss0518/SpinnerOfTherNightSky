using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ZoneScriptBase : MonoBehaviour
{
    public ScriptManager.ZoneType zoneType { get; protected set; } = 0;

    abstract public void SelectTargetTest(ScriptManager.SelectCardArgument _action, Player _runPlayer);
    abstract public void SelectTargetDown();

    abstract public void RemoveCard(CardData _card);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ZoneScriptBase : MonoBehaviour
{
    public ScriptManager.ZoneType zoneType { get; protected set; } = 0;

    public virtual void Init(Player _player,GameManager _manager)
    {
        if (_player == null) return;
        if (_manager == null) return;
        player = _player;
        manager = _manager;
    }

    abstract public void SelectTargetTest(ScriptManager.SelectCardArgument _action, Player _runPlayer);
    abstract public void SelectTargetDown();

    abstract public void RemoveCard(CardData _card);

    public Player player { get; private set; } = null;
    public GameManager manager { get; private set; } = null;

}

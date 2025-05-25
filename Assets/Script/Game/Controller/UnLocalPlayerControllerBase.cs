using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnLocalPlayerControllerBase : ControllerBase
{
    public void SetGameManager(GameManager _manager)
    {
        gameManager = _manager;
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
    }

    protected GameManager gameManager { get; private set; } = null;

    protected Player player { get; private set; } = null;
}

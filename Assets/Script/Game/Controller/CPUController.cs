using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CPUController : UnLocalPlayerControllerBase
{
    [SerializeField]
    int maxWaitCount = 100;

    [SerializeField,ReadOnly]
    int nowWaitCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (!isControll) return;

        nowWaitCount++;
        if (maxWaitCount > nowWaitCount) return;

        nowWaitCount = 0;
        UseItemUpdate();
        PutStoneUpdate();
        PlayMagicUpdate();
        SetTrapUpdate();
    }

    void UseItemUpdate()
    {
        if (gameManager.turnStep != TurnManager.MainStep.UseItem) return;
        UpActionFlg();
    }

    void PutStoneUpdate()
    {
        if (gameManager.turnStep != TurnManager.MainStep.PutStone) return;

        for(int i = 0;i < 3;i++)
        {
            var x = Random.Range(0, gameManager.stoneBoardObj.PANEL_COUNT_X);
            var y = Random.Range(0, gameManager.stoneBoardObj.PANEL_COUNT_Y);
            if (gameManager.stoneBoardObj.IsPutStone(x,y)) continue;

            gameManager.SelectStonePos(x, y);
        }

        UpActionFlg();
    }

    void PlayMagicUpdate()
    {
        if (gameManager.turnStep != TurnManager.MainStep.PlayMagic) return;
        UpActionFlg();
    }

    void SetTrapUpdate()
    {
        if (gameManager.turnStep != TurnManager.MainStep.SetTrap) return;
        UpActionFlg();

    }


}

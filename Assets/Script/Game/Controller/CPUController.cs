using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CPUController : ControllerBase
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
        UpActionFlg();
    }
}

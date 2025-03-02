using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class DebugManager : MonoBehaviour
{
    [SerializeField,ReadOnly]
    Manager manager = Manager.ins;

    [SerializeField]
    Manager.GameType gameType = Manager.GameType.Normal;


    [SerializeField]
    Manager.MemberType[] cpuFlgs =
    {
        Manager.MemberType.CPU,
        Manager.MemberType.None,
        Manager.MemberType.None
    };

    [SerializeField]
    int useBookNo = 0;


    void Awake()
    {
        if (!Manager.ins.IS_DEBUG) return;
        manager.gameType = gameType;
        manager.cpuFlgs = cpuFlgs;
        manager.useBookNo = useBookNo;
    }
}

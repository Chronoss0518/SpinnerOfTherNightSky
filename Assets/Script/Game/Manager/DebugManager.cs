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
    Manager.MemberType[] memberFlgs =
    {
        Manager.MemberType.LocalPlayer,
        Manager.MemberType.CPU,
        Manager.MemberType.None,
        Manager.MemberType.None
    };

    public bool localGameFlg = true;

    [SerializeField]
    int useBookNo = 0;

    [SerializeField]
    int randomPutStone = 0;

    void Awake()
    {
        if (!Manager.ins.IS_DEBUG) return;
        manager.gameType = gameType;
        manager.memberFlgs = memberFlgs;
        manager.useBookNo = useBookNo;
        manager.randomPutStone = randomPutStone;
        manager.localGameFlg = localGameFlg;
    }
}

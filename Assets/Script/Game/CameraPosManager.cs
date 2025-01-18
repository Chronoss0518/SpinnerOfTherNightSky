using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CameraPosManager : MonoBehaviour
{
    public enum CameraPosition
    {
        Main,
        StoneBoard,
        MyItemZone,
        MyMagicZone,
        MyTrashZone,
        P1ItemZone,
        P1MagicZone,
        P1TrashZone,
        P2ItemZone,
        P2MagicZone,
        P2TrashZone,
        P3ItemZone,
        P3MagicZone,
        P3TrashZone,
    }

    [SerializeField,ReadOnly]
    CameraPosition pos = CameraPosition.StoneBoard;

    [SerializeField]
    Animator animator = null;


    public void SetPosition(CameraPosition _pos)
    {
        if (animator == null) return;
        if (pos == _pos) return;

        animator.SetInteger("PositionValue", (int)_pos);
        pos = _pos;
    }


    void Start()
    {
        SetPosition(CameraPosition.Main);
    }

}

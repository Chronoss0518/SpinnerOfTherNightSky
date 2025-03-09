using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ControllerBase : MonoBehaviour
{
    public bool isAction { get; private set; } = false;

    public bool isControll { get; private set; } = false;

    public void UpActionFlg() { isAction = true; }

    public void DownActionFlg() { isAction = false; }

    public void ActionStart() 
    {
        if (isControll) return;
        isAction = false;
        RunActionStart();
        isControll = true;
    }

    public void ActionEnd()
    {
        if (!isControll) return;
        RunActionEnd();
        isControll = false;
    }

    virtual protected void RunActionStart() { }
    virtual protected void RunActionEnd() { }


}

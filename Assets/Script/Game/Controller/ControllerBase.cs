using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ControllerBase : MonoBehaviour
{
    public bool isAction { get; private set; } = false;

    public void Action() { ActionStart(); }

    public void ActionStart() 
    {
        isAction = true;
        RunActionStart();
    }

    public void ActionEnd()
    {
        isAction = false;
        RunActionEnd();
    }

    virtual protected void RunActionStart() { }
    virtual protected void RunActionEnd() { }

}

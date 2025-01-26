using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{

    public UnityEngine.Events.UnityEvent act = new UnityEngine.Events.UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        act.Invoke();
    }

}

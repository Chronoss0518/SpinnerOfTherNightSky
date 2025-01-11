using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRatioChecker : MonoBehaviour
{
    Manager manager = Manager.ins;
    // Update is called once per frame
    void Update()
    {
        manager.aspectType = Manager.DisplayAspectType.None;

        manager.aspectType = Screen.height > Screen.width
            ? Manager.DisplayAspectType.VerticalScreen
            : manager.aspectType;

        manager.aspectType = Screen.width > Screen.height
            ? Manager.DisplayAspectType.LandscapeScreen
            : manager.aspectType;

    }
}

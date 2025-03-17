using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerController : ControllerBase
{
    PlayerControllerUI verticalUIs = null;
    PlayerControllerUI landscapeUIs = null;

    Manager manager = Manager.ins;

    public void SetVerticalUIs(PlayerControllerUI _uis)
    {
        if (_uis == null) return;
        verticalUIs = _uis;
    }

    public void SetLandscapeUIs(PlayerControllerUI _uis)
    {
        if (_uis == null) return;
        landscapeUIs = _uis;
    }

    public string buttonText { set; private get; } = "";

    PlayerControllerUI uis { 
        get {
            return manager.aspectType == Manager.DisplayAspectType.VerticalScreen ?
                verticalUIs :
                landscapeUIs;
        }
    }

    override protected void RunActionStart() 
    {
        if (uis == null) return;
        if (uis.buttonVisibleCanvas == null) return;
        if (uis.selectButtonText == null) return;
        if (uis.buttonVisibleCanvasController == null) return;
        //uis.selectButtonText.text = buttonText;
        uis.buttonVisibleCanvas.gameObject.SetActive(true);
        uis.buttonVisibleCanvasController.gameObject.SetActive(true);
    }

    override protected void RunActionEnd()
    {
        if (uis == null) return;
        if (uis.buttonVisibleCanvas == null) return;
        if (uis.buttonVisibleCanvasController == null) return;
        uis.buttonVisibleCanvas.gameObject.SetActive(false);
        uis.buttonVisibleCanvasController.gameObject.SetActive(false);
    }
}

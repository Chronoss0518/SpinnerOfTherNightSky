using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text = null;

    public void SetText(string _str)
    {
        if (text == null) return;
        text.text = _str;
    }
}

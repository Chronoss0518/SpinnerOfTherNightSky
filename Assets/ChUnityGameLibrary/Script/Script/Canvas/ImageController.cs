using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    public UnityEngine.UI.Image target = null;

    public List<Texture2D>textureList = new List<Texture2D>();

    public void SetTexture(int _no)
    {
        if (target == null) return;
        if (textureList.Count <= _no) return;

        Texture2D tex = (Texture2D)textureList[_no];

        target.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.zero);
    }
}

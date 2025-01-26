using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public Renderer render = null;
    
    private Color CreateColor(float _r, float _g, float _b, float _a)
    {

        Color tmp = new Color();
        tmp.r = _r;
        tmp.g = _g;
        tmp.b = _b;
        tmp.a = _a;
        return tmp;
    }

    public void SetColor(Color _color)
    {
        if (render == null) return;
        render.material.color = _color;
    }

    public void SetColorBlack()
    {
        SetColor(Color.black);
    }

    public void SetColorBlue()
    {
        SetColor(Color.blue);
    }

    public void SetColorCyan()
    {
        SetColor(Color.cyan);
    }

    public void SetColorGrey()
    {
        SetColor(Color.grey);
    }

    public void SetColorMagenta()
    {
        SetColor(Color.magenta);
    }

    public void SetColorRed()
    {
        SetColor(Color.red);
    }

    public void SetColorWhite()
    {
        SetColor(Color.white);
    }

    public void SetColorYellow()
    {
        SetColor(Color.yellow);
    }
    public void SetColor(string _nameID,Color _color)
    {
        if (render == null) return;
        render.material.color = _color;
    }

    public void SetColorBlack(string _nameID)
    {
        SetColor(_nameID, Color.black);
    }

    public void SetColorBlue(string _nameID)
    {
        SetColor(_nameID, Color.blue);
    }

    public void SetColorCyan(string _nameID)
    {
        SetColor(_nameID, Color.cyan);
    }

    public void SetColorGrey(string _nameID)
    {
        SetColor(_nameID, Color.grey);
    }

    public void SetColorMagenta(string _nameID)
    {
        SetColor(_nameID, Color.magenta);
    }

    public void SetColorRed(string _nameID)
    {
        SetColor(_nameID, Color.red);
    }

    public void SetColorWhite(string _nameID)
    {
        SetColor(_nameID, Color.white);
    }

    public void SetColorYellow(string _nameID)
    {
        SetColor(_nameID, Color.yellow);
    }

    public void SetAlbedoColor(Color _color)
    {
        if (render == null) return;
        render.material.SetColor("Albedo", _color);
    }

    public void SetAlbedoColorRGBA(float _r, float _g, float _b, float _a)
    {
        if (render == null) return;

        var tmp = CreateColor(_r,_g, _b, _a);

        render.material.SetColor("Albedo", tmp);
    }

    public void SetAlbedoColorRGB(float _r, float _g, float _b)
    {
        if (render == null) return;

        var tmp = CreateColor(_r, _g, _b, 1.0f);

        render.material.SetColor("Albedo", tmp);
    }
}

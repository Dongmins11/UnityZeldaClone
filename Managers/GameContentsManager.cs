using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using OccaSoftware.RadialBlur.Runtime;
using OccaSoftware.RadialBlur.Demo;

public class GameContentsManager
{
    Color DefalutMatColor = new Color(1f, 1f, 1f, 1f);
    float DefalutMatPower = 0.8f;

    public Action ColorChangeAction = null;

    public Action DefalutColorChangeAction = null;

    RadialBlurPostProcess My_RadialBlur = null;
    public Volume My_Volume;
    private VolumeProfile My_profile = null;


    private bool ValidateRadialBlurManager()
    {
        Check_Volume();

        My_profile = My_Volume.sharedProfile;
        return My_profile.TryGet(out My_RadialBlur);
    }

    public void Check_Volume()
    {
        if (null != My_Volume)
            return;

        Volume Object = GameObject.Find("Global Volume").GetComponent<Volume>();

        if (null != Object)
            My_Volume = Object;
    }

    public void Clear()
    {
        My_Volume = null;
        ColorChangeAction = null;
        DefalutColorChangeAction = null;
    }

    public void AddColorChangeAction(Action _Funtion)
    {
        ColorChangeAction -= _Funtion;
        ColorChangeAction += _Funtion;
    }

    public void DeleteColorChangeAction(Action _Funtion)
    {
        ColorChangeAction -= _Funtion;
    }

    public void AddDefalutColorChangeAction(Action _Funtion)
    {
        DefalutColorChangeAction -= _Funtion;
        DefalutColorChangeAction += _Funtion;
    }

    public void DeleteDefalutColorChangeAction(Action _Funtion)
    {
        DefalutColorChangeAction -= _Funtion;
    }


    public void ShaderColorChange(GameObject _Object, Color _Color, float _fPowerValue = 0.8f)
    {
        if (null == _Object)
            return;

        Renderer[] TempRenderer = _Object.GetComponentsInChildren<Renderer>();

        if (null != TempRenderer)
        {
            foreach (var Renderer in TempRenderer)
            {
                Color TempColor = Renderer.material.GetColor("_MainColor");
                Renderer.material.SetColor("_MainColor", _Color);
                Renderer.material.SetFloat("_MaiColPo", _fPowerValue);
            }
        }
    }

    public void ShaderColorChange(GameObject _Object, List<Color> _Colors, List<float> _fPowerValues)
    {
        Renderer[] TempRenderer = _Object.GetComponentsInChildren<Renderer>();

        if (null != TempRenderer)
        {
            for(int i =0; i < TempRenderer.Length; ++i)
            {
                Color TempColor = TempRenderer[i].material.GetColor("_MainColor");
                TempRenderer[i].material.SetColor("_MainColor", _Colors[i]);
                TempRenderer[i].material.SetFloat("_MaiColPo", _fPowerValues[i]);
            }
        }

    }


    public void RaidalBlur_Delay(float delay)
    {
        if (ValidateRadialBlurManager())
            My_RadialBlur.SetDelay(delay);
    }

    public void RadialBlur_Intensity(float value)
    {
        if (ValidateRadialBlurManager())
            My_RadialBlur.SetIntensity(value);
    }

    public void RadialBlur_Center(Vector2 value)
    {
        if (ValidateRadialBlurManager())
            My_RadialBlur.SetCenter(value);
    }

}

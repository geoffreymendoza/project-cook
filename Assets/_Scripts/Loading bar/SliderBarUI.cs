using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBarUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _image;
    public Color Low;
    public Color High;

    private bool _init = false;
    public void Initialize(float duration)
    {
        if (_init) return;
        _slider.maxValue = duration;
        _image.color = Color.Lerp(Low, High, _slider.normalizedValue);
        _init = true;
    }

    public void OnUpdateTimeUI(float duration)
    {
        _slider.value = duration;
        _image.color = Color.Lerp(Low, High, _slider.normalizedValue);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _tooltipIntro;

    private RectTransform _vfx = null;

    public void Setup(string text, RectTransform vfx = null)
    {
        _text.text = string.Format("{0}{1}", _tooltipIntro, text);
        gameObject.SetActive(true);
        if (vfx)
        {
            vfx.gameObject.SetActive(true);
            _vfx = vfx;
        }

    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if (_vfx)
            _vfx.gameObject.SetActive(false);
    }
}

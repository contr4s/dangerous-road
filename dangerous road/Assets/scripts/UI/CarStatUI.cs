using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarStatUI: MonoBehaviour
{
    [SerializeField] private CarParamsSO _carParams;
    [SerializeField] private eCarParameter _type;

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valText;


    private void Start()
    {
        if (_carParams.TryFindParam(_type, out CarParametr parametr))
        {
            _slider.value = parametr.curLvl;
            _slider.maxValue = parametr.maxLvl;
            _valText.text = string.Format("{0}/{1}", parametr.curVal.ToString("F1"), parametr.CalculteMaxVal().ToString("F1"));
        }
    }
}

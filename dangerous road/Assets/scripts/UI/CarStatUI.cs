using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarStatUI: MonoBehaviour
{
    private const string _maxLvl = "max level";

    [SerializeField] private CarParamsSO _carParams;
    [SerializeField] private eCarParameter _type;

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valText;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TextMeshProUGUI _upgradeButtonText;

    private CarParameter _parameter;
    public CarParameter Parameter { get => _parameter; private set => _parameter = value; }

    private void Start()
    {
        if (_carParams.TryFindParam(_type, out CarParameter parametr))
        {
            Parameter = parametr;
            UpdateUI(parametr);
        }
        else
            Debug.LogWarningFormat("parameter {0} not found on {1}", _type, _carParams);
    }

    public void LvlUp()
    {
        Parameter.LvlUp();
        UpdateUI(Parameter);
    }

    private void UpdateUI(CarParameter parametr)
    {
        _slider.value = parametr.curLvl;
        _slider.maxValue = parametr.maxLvl;
        _valText.text = string.Format("{0}/{1}", parametr.CurVal.ToString("F0"), parametr.CalculteMaxVal().ToString("F0"));
        if (parametr.curLvl >= parametr.maxLvl)
        {
            _upgradeButton.interactable = false;
            _upgradeButtonText.text = _maxLvl; 
        }
    }
}

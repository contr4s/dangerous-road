using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarStatUI: MonoBehaviour
{
    private const string _maxLvl = "max level";

    public Button upgradeButton;

    [SerializeField] private eCarParameterType _type;

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valText;  
    [SerializeField] private TextMeshProUGUI _upgradeButtonText;

    private CarParameter _parameter;
    public CarParameter Parameter { get => _parameter; private set => _parameter = value; }

    private CarParamsSO _carParams;
    public CarParamsSO CarParams
    {
        get => _carParams;
        set { 
            _carParams = value;
            SetupParameter();
        }
    }

    private void SetupParameter()
    {
        if (CarParams.TryFindParam(_type, out CarParameter parametr))
        {
            Parameter = parametr;
            UpdateUI(parametr);
        }
        else
            Debug.LogWarningFormat("parameter {0} not found on {1}", _type, CarParams);
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
            upgradeButton.interactable = false;
            _upgradeButtonText.text = _maxLvl;
        }
        else
        {
            upgradeButton.interactable = true;
            _upgradeButtonText.text = "upgrade";
        }
    }
}

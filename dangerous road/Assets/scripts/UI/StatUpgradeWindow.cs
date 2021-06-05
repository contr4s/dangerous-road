using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgradeWindow : MonoBehaviour
{
    public const string value = "value";
    public const string lvl = "level";
    public const string price = "price";

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _value;
    [SerializeField] private TextMeshProUGUI _lvl;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Button _upgradeButton;

    private CarStatUI _curStat;

    public void Setup(CarStatUI carStat)
    {
        _curStat = carStat;
        var parameter = carStat.Parameter;

        _title.text = parameter.type.ToString();
        _value.text = string.Format("{0}: {1} -> {2}", value, parameter.CurVal.ToString("F0"), parameter.NextVal.ToString("F0"));
        _lvl.text = string.Format("{0}: {1} -> {2}", lvl, parameter.curLvl, parameter.curLvl + 1);
        _price.text = string.Format("{0}: {1}$", price, parameter.CurPrice);
        if (parameter.CurPrice > GameManager.S.moneyManager.Money)
            _upgradeButton.interactable = false;
    }

    public void UpgradeButton()
    {
        if (_curStat is null)
            return;

        GameManager.S.moneyManager.Money -= _curStat.Parameter.CurPrice;
        _curStat.LvlUp();
    }
}

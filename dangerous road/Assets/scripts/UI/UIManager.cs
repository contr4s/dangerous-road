using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _distDisplay;
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    [SerializeField] private GameObject _loseDisplay;
    [SerializeField] private GameObject _pauseOverlay;
    [SerializeField] private Button[] _gameplayButtons;

    readonly NumberFormatInfo _nfi = new CultureInfo("en-US", false).NumberFormat;

    private float _dist = 0;

    public float Dist
    {
        get => _dist;
        set
        {
            if (value < 0)
                Debug.LogError("dist can't be less than 0");
            _dist = value;
            _distDisplay.text = string.Format("Dist: {0}km", _dist.ToString("F1"));
        }
    }

    private int _money = 0;
    public int Money
    {
        get => _money;
        set
        {
            if (value < 0)
                Debug.LogError("collected money can't be less than 0");
            _money = value;
            _moneyDisplay.text = string.Format("Money: {0}", _money.ToString("C", _nfi));
        }
    }

    private void OnEnable()
    {
        SandStorm.stormReachedDestination += SetupLoseDisplay;
    }

    private void OnDisable()
    {
        SandStorm.stormReachedDestination -= SetupLoseDisplay;
    }

    public void SetupLoseDisplay()
    {
        _loseDisplay.SetActive(true);
        _pauseOverlay.SetActive(true);        
    }

    public void BlockButtons()
    {
        foreach (var button in _gameplayButtons)
            button.gameObject.SetActive(false);
    }
}

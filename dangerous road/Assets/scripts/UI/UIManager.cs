using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager: MonoBehaviour
{
    public const string gameplayScene = "gameplay";

    [SerializeField] private TextMeshProUGUI _distDisplay;
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    [SerializeField] public TextMeshProUGUI winDisplay;
    [SerializeField] private GameObject _loseDisplay;

    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

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

    private float _money = 0;
    public float Money
    {
        get => _money;
        set
        {
            if (value < 0)
                Debug.LogError("collected money can't be less than 0");
            _money = value;
            _moneyDisplay.text = string.Format("Money: {0}", _money.ToString("C", nfi));
        }
    }

    public void SetupLoseDisplay()
    {
        _loseDisplay.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(gameplayScene);
    }
}

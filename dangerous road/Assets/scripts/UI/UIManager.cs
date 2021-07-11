using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _distDisplay;
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    [SerializeField] private LoseOverlay _loseOverlay;
    [SerializeField] private SecondChanseOverlay _secondChanceOverlay;
    [SerializeField] private float _delayBeforeSecondChanseOverlay = 2;
    [SerializeField] private float _delayAfterSecondChanseOverlay = 2;
    [SerializeField] private Button[] _gameplayButtons;
    [SerializeField] private SandStorm _storm;

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
        Car.clashWithObstacle += SetupSecondChanceOverlay;
    }

    private void OnDisable()
    {
        Car.clashWithObstacle -= SetupSecondChanceOverlay;
    }

    public void RunAwayAfterClash()
    {
        LevelManager.UnPause();
        _secondChanceOverlay.gameObject.SetActive(false);
        _storm.StartStorm();
        _loseOverlay.Setup(Dist, Money);
        StartCoroutine(SetupOverlayAfterDelay(_loseOverlay.gameObject, _delayAfterSecondChanseOverlay));
    }

    private void SetupSecondChanceOverlay()
    {
        StartCoroutine(SetupOverlayAfterDelay(_secondChanceOverlay.gameObject, _delayBeforeSecondChanseOverlay));
        _secondChanceOverlay.timeout += RunAwayAfterClash;
    }

    private IEnumerator SetupOverlayAfterDelay(GameObject overlay, float time, bool pauseGame = true)
    {
        yield return new WaitForSeconds(time);
        overlay.gameObject.SetActive(true);
        if (pauseGame)
            LevelManager.Pause();
    }

    public void BlockButtons()
    {
        foreach (var button in _gameplayButtons)
            button.gameObject.SetActive(false);
    }
}

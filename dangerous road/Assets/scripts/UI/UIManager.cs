using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [Header("Displays")]
    [SerializeField] private TextMeshProUGUI _pointsDisplay;
    [SerializeField] private TextMeshProUGUI _distDisplay;
    [SerializeField] private TextMeshProUGUI _moneyDisplay;

    [Header("Overlays")]
    [SerializeField] private LoseOverlay _loseOverlay;
    [SerializeField] private SecondChanseOverlay _secondChanceOverlay;
    [SerializeField] private float _delayBeforeSecondChanseOverlay = 2;
    [SerializeField] private float _delayAfterSecondChanseOverlay = 2;

    [Header("Other")]
    [SerializeField] private RectTransform[] _hudElements;
    [SerializeField] private FollowCar _storm;

    private float _points = 0;
    public float Points
    {
        get => _points;
        set
        {
            if (value < 0)
                Debug.LogError("points can't be less than 0");
            _points = value;
            _pointsDisplay.text = string.Format("POINTS {0}", _points.ToString("F0"));
        }
    }

    private float _dist = 0;
    public float Dist
    {
        get => _dist;
        set
        {
            if (value < 0)
                Debug.LogError("dist can't be less than 0");
            _dist = value;
            _distDisplay.text = string.Format("{0}m", _dist.ToString("F0"));
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
            _moneyDisplay.text = string.Format("{0}$", _money.ToString());
        }
    }

    private void OnEnable()
    {
        Car.clashWithObstacle += SetupSecondChanceOverlay;
        Money = 0;
    }

    private void OnDisable()
    {
        Car.clashWithObstacle -= SetupSecondChanceOverlay;
    }

    public void RunAwayAfterClash()
    {
        LevelManager.UnPause();
        _secondChanceOverlay.gameObject.SetActive(false);
        //_storm.StartStorm();
        StartCoroutine(_storm.Chase());
        _loseOverlay.Setup(Dist, Money, Points);
        GameManager.S.moneyManager.Money += Money;
        SaveGameManager.Save();
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

    public void SetActiveAllHudElements(bool active)
    {
        foreach (var button in _hudElements)
            button.gameObject.SetActive(active);
    }
}

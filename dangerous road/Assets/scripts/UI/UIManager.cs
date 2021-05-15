using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager: MonoBehaviour
{
    public const string gameplayScene = "gameplay";

    [SerializeField] private TextMeshProUGUI _distDisplay;
    [SerializeField] public TextMeshProUGUI winDisplay;
    [SerializeField] private GameObject _loseDisplay;

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

    public void SetupLoseDisplay()
    {
        _loseDisplay.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(gameplayScene);
    }
}

using TMPro;
using UnityEngine;

public class UIManager: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreDisplay;
    [SerializeField] public TextMeshProUGUI winDisplay;
    [SerializeField] public TextMeshProUGUI loseDisplay;

    private int _score = 0;

    public int Score
    {
        get => _score;
        set
        {
            if (value < 0)
                Debug.LogError("score can't be less than 0");
            _score = value;
            _scoreDisplay.text = string.Format("Score: {0}", _score);
        }
    }
}

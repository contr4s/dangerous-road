using TMPro;
using UnityEngine;

public class UIManager: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _distDisplay;
    [SerializeField] public TextMeshProUGUI winDisplay;
    [SerializeField] public TextMeshProUGUI loseDisplay;

    private float _dist = 0;

    public float Dist
    {
        get => _dist;
        set
        {
            if (value < 0)
                Debug.LogError("dist can't be less than 0");
            _dist = value;
            _distDisplay.text = string.Format("Dist: {0}km", _dist);
        }
    }
}

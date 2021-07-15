using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dist;
    [SerializeField] private TextMeshProUGUI _money;
    [SerializeField] private TextMeshProUGUI _points;


    public void Setup(float dist, int money)
    {
        _dist.text = $"{dist:F0}m";
        _money.text = $"{money}$";
    }
}

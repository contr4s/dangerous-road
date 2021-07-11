using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecondChanseOverlay : MonoBehaviour
{
    public Action timeout;

    [SerializeField] private TextMeshProUGUI _timeDisplay;
    [SerializeField] private QuantumTek.QuantumUI.QUI_Bar _timerDisplay;
    [SerializeField] private int _startTime;

    private int _curTime;
    int CurTime {
        get => _curTime;
        set
        {
            _curTime = value;
            _timeDisplay.text = CurTime.ToString();
            _timerDisplay.SetFill((float)CurTime / _startTime);
        }
    }

    private void OnEnable()
    {
        CurTime = _startTime;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        var oneSecond = new WaitForSecondsRealtime(1);
        while(CurTime > 0)
        {
            CurTime--;
            yield return oneSecond;
        }
        timeout?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseOverlay : MonoBehaviour
{
    [SerializeField] private int _countDownTime;
    [SerializeField] TextMeshProUGUI _timerDisplay;
    [SerializeField] Button _pauseButton;

    public void Close()
    {
        LevelManager.CloseOverlay(gameObject);
        _timerDisplay.gameObject.SetActive(true);
        _timerDisplay.StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        var oneSecond = new WaitForSecondsRealtime(1);
        var timer = _countDownTime;
        while (timer > 0)
        {
            _timerDisplay.text = timer.ToString();
            timer--;
            yield return oneSecond;
        }
        _timerDisplay.gameObject.SetActive(false);
        LevelManager.UnPause();
        GameplaySoundManager.SetPauseToAllSounds(false);
        _pauseButton.gameObject.SetActive(true);
    }
}

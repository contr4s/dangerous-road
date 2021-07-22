using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct TutorialTooltip
{
    public string descriptionText;
    public Tooltip tooltip;
    public RectTransform vfx;
    public float showingTime;
    public float delayAfterTooltip;
}

public class Tutorial : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] private Image _background;

    [SerializeField] private float _startDelay = 3;

    [SerializeField] private TutorialTooltip[] _tooltips;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_startDelay);
        StartTutorial();
    }

    public void StartTutorial()
    {
        StartCoroutine(ShowTooltips());
    }

    private IEnumerator ShowTooltips()
    {      
        for (int i = 0; i < _tooltips.Length; i++)
        {
            _background.gameObject.SetActive(true);
            var tooltip = _tooltips[i];
            tooltip.tooltip.Setup(tooltip.descriptionText, tooltip.vfx);
            Time.timeScale = .5f;

            yield return new WaitForSecondsRealtime(tooltip.showingTime);

            Time.timeScale = 1f;
            tooltip.tooltip.Hide();
            _background.gameObject.SetActive(false);

            yield return new WaitForSeconds(tooltip.delayAfterTooltip);
        }
    }
}

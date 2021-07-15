using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zindea.Sounds;

[RequireComponent(typeof(SoundManager))]
public class MusisManager: MonoBehaviour
{
    private static SoundManager _soundManager;
    public static bool muted;

    [SerializeField] private SoundSet _music;
    [SerializeField] private Toggle _musicToggle;

    private void Awake()
    {
        _soundManager = GetComponent<SoundManager>();
    }

    private void Start()
    {
        _musicToggle.isOn = !muted;
        PlayMusic();
    }

    public void SetMute(bool toggleValue)
    {
        muted = !toggleValue;
        if (muted)
        {
            StopSound();
        }
        else
        {
            PlayMusic();
        }
    }

    public void PlayMusic()
    {
        if (muted)
            return;
        _soundManager.PlaySound(_music);
    }

    public void ChangeSoundVolume(float amount)
    {
        _soundManager.ChangeVolume(_music, amount);
    }

    public void StopSound()
    {
        _soundManager.StopSound(_music);
    }

    public void SetPause(bool pause)
    {
        _soundManager.SetPitch(_music, pause ? 0 : 1);
    }
}

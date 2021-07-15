using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zindea.Sounds;

public enum eSoundType {
    startEngine,
    engine,
    drive,
    collision,
    sparks,
    coins,
    tornado
}

[RequireComponent(typeof(SoundManager))]
public class GameplaySoundManager : MonoBehaviour
{
    private static SoundManager _soundManager;
    private static SerializableDictionary<eSoundType, SoundSet> _soundsStaticCopy;
    public static bool muted;

    [SerializeField] private SerializableDictionary<eSoundType, SoundSet> _sounds;
    [SerializeField] private float _tornadoStartVolume;
    [SerializeField] private Toggle _soundToggle;

    public float TornadoStartVolume { get => _tornadoStartVolume; }

    private void Awake()
    {
        _soundManager = GetComponent<SoundManager>();
        _soundsStaticCopy = _sounds;       
    }

    private void Start()
    {
        ChangeSoundVolume(eSoundType.tornado, _tornadoStartVolume);
        PlaySound(eSoundType.tornado);
        _soundToggle.isOn = !muted;
    }

    public void SetMute(bool toggleValue)
    {
        muted = !toggleValue;
        if (muted)
        {
            foreach (var sound in _soundsStaticCopy)
            {
                _soundManager.StopSound(sound.Value);
            }
        }
        else
        {
            foreach (var sound in _soundsStaticCopy)
            {
                if (sound.Value.LoopedSounds)
                    PlaySound(sound.Key);
            }
            SetPauseToAllSounds(true);
        }
    }

    public void PlaySound(eSoundType type)
    {
        if (muted)
            return;
        _soundManager.PlaySound(_sounds[type]);
    }

    public IEnumerator PlaySoundCoroutine(eSoundType type)
    {
        PlaySound(type);
        yield return new WaitForSeconds(_soundManager.GetSoundDuration(_sounds[type]));
    }

    public void ChangeSoundVolume(eSoundType type, float amount)
    {
        _soundManager.ChangeVolume(_sounds[type], amount);
    }

    public void StopSound(eSoundType type)
    {
        _soundManager.StopSound(_sounds[type]);
    }

    public static void SetPauseToAllSounds(bool pause)
    {
        foreach(var sound in _soundsStaticCopy)
        {
            _soundManager.SetPitch(sound.Value, pause ? 0 : 1);
        }
    }
}

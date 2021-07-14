using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private SerializableDictionary<eSoundType, SoundSet> _sounds;
    [SerializeField] private float _tornadoStartVolume;

    public float TornadoStartVolume { get => _tornadoStartVolume; }

    private void Awake()
    {
        _soundManager = GetComponent<SoundManager>();
        _soundsStaticCopy = _sounds;
        ChangeSoundVolume(eSoundType.tornado, _tornadoStartVolume);
        PlaySound(eSoundType.tornado);
    }

    public void PlaySound(eSoundType type)
    {
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

    public static void ReactToChangingTimescale()
    {
        foreach(var sound in _soundsStaticCopy)
        {
            _soundManager.SetPitch(sound.Value);
        }
    }
}

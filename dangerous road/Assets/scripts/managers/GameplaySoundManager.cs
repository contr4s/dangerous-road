using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zindea.Sounds;

public enum eSoundType {
    startEngine,
}

[RequireComponent(typeof(SoundManager))]
public class GameplaySoundManager : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<eSoundType, SoundSet> _sounds;

    private SoundManager _soundManager;

    private void Awake()
    {
        _soundManager = GetComponent<SoundManager>();
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
}

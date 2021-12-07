using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
class Invisibility: SpecialAbility
{
    [SerializeField] private Image _activeEffect;

    protected override IEnumerator Ability(float duration)
    {
        var car = CarSpawnManager.SpawnedCar;
        car.ChangeInvisibleState(true);
        _activeEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        car.ChangeInvisibleState(false);
        _activeEffect.gameObject.SetActive(false);
    }
}

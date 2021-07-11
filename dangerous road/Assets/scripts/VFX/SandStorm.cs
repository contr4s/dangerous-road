using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStorm: MonoBehaviour
{
    public static Action stormReachedDestination;

    [SerializeField] private ParticleSystem _storm;
    [SerializeField] private float _stormSpeed;
    [SerializeField] private float _targetZPos;

    public void StartStorm()
    {
        _storm.gameObject.SetActive(true);
        if (TryGetComponent(out FollowCar followCar))
        {
            followCar.follow = false;
            StartCoroutine(Move(followCar.CalculateZPos(0)));
        }
        else
            StartCoroutine(Move(_targetZPos));
    }

    private IEnumerator Move(float targetZ)
    {
        float i = 0;
        var pos = _storm.transform.localPosition;
        var dist = targetZ - pos.z;
        while (i < dist)
        {
            var z = Mathf.MoveTowards(_storm.transform.localPosition.z, targetZ, _stormSpeed * Time.deltaTime);
            _storm.transform.localPosition = new Vector3(pos.x, pos.y, z);
            i += _stormSpeed * Time.deltaTime;
            yield return null;
        }
        stormReachedDestination?.Invoke();
    }
}

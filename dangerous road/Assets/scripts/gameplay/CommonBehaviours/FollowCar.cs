using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    [SerializeField] private float _yPos;
    [SerializeField] private float _xPos;
    [SerializeField] private bool _changeXPos = false;
    [SerializeField] private float _distToCar;
    [SerializeField] private bool _UseCarRotationYAxis = false;

    private Car _car;

    private void OnEnable()
    {
        CarSpawnManager.carSpawned += Init;
    }

    private void OnDisable()
    {
        CarSpawnManager.carSpawned -= Init;
    }

    private void LateUpdate()
    {
        var xPos = _changeXPos ? _car.transform.position.x : _xPos;
        transform.position = new Vector3(xPos, _yPos, _car.transform.position.z - _distToCar);

        if (_UseCarRotationYAxis)
            transform.rotation = new Quaternion(transform.rotation.x, _car.transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    private void Init(Car car)
    {
        _car = car;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar: MonoBehaviour
{
    public bool follow = true;

    [SerializeField] private float _yPos;
    [SerializeField] private float _xPos;
    [SerializeField] private bool _changeXPos = false;
    [SerializeField] private float _distToCar;
    [SerializeField] private bool _UseCarRotationYAxis = false;
    [SerializeField] private float _chaseSpeed = 0;
    [SerializeField] private float _chaseOffset = 40;

    private Car _car;

    private void OnEnable()
    {
        CarSpawnManager.CarSpawned += Init;
    }

    private void OnDisable()
    {
        CarSpawnManager.CarSpawned -= Init;
    }

    private void LateUpdate()
    {
        if (!follow)
            return;

        var xPos = _changeXPos ? _car.transform.position.x : _xPos;
        transform.position = new Vector3(xPos, _yPos, CalculateZPos());

        if (_UseCarRotationYAxis)
            transform.rotation = new Quaternion(transform.rotation.x, _car.transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    public float CalculateZPos(float distToCar)
    {
        return _car.transform.position.z - distToCar;
    }
    public float CalculateZPos()
    {
        return CalculateZPos(_distToCar);
    }

    public IEnumerator Chase()
    {
        while (_distToCar > _chaseOffset)
        {
            _distToCar -= _chaseSpeed * Time.deltaTime;
            yield return null;
        }
        _distToCar = _chaseOffset;
    }

    private void Init(Car car)
    {
        _car = car;
    }  
}

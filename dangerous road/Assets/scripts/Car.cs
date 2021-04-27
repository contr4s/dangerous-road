using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float _motorForce;
    [SerializeField] private WheelCollider[] _wheels;

    private void Start()
    {
        foreach (var wheel in _wheels)
        {
            wheel.motorTorque = _motorForce;
        }
    }
}

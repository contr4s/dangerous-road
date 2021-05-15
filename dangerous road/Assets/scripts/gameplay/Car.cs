using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car: MonoBehaviour
{
    public const int hundredMeters = 100;
    public const int kilometer = 1000;

    public Action passedHundredMeters;
    public Action passedOneKilometer;
    public float passedDist;

    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;

    [SerializeField] private float _turnSpeed;


    [SerializeField] private UIManager _uIManager;

    private Vector3 _curreneAccceleration = new Vector3();
    private int _multiplier = 1;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {       
        passedDist = transform.position.z;

        if (passedDist > hundredMeters * _multiplier)
        {
            passedHundredMeters?.Invoke();
            if (_multiplier % 10 == 0)
                passedOneKilometer?.Invoke();
            _uIManager.Dist += 0.1f;
            _multiplier++;
        }
    }

    private void FixedUpdate()
    {
        _curreneAccceleration.z = _acceleration;
        if (_rigidbody.velocity.z > _maxSpeed)
            _curreneAccceleration.z = 0;

        _rigidbody.AddForce(_curreneAccceleration, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            _rigidbody.isKinematic = true;

            if (!_uIManager)
                Debug.LogError("you must assign UIManager on the inspector");
            else
                _uIManager.SetupLoseDisplay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("finish"))
        {
            _rigidbody.isKinematic = true;
            if (!_uIManager)
                Debug.LogError("you must assign UIManager on the inspector");
            else
                _uIManager.winDisplay.gameObject.SetActive(true);
        }
    }

    public void TurnButton(bool turningRight)
    {
        if (turningRight)
            Turn(_turnSpeed);
        else
            Turn(-_turnSpeed);
    }

    private void Turn(float coef)
    {
        _rigidbody.AddForce(coef, 0, 0, ForceMode.VelocityChange);
    }
}

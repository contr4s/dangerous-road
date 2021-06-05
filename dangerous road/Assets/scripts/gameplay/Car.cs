using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car: MonoBehaviour
{
    public const string obstacleTag = "obstacle";
    public const int hundredMeters = 100;
    public const int kilometer = 1000;

    public static Action gameOver;
    public bool isLosed;

    public static Action passedHundredMeters;
    public static Action passedOneKilometer;
    private float _passedDist;

    public CarParamsSO parametrs;

    [SerializeField] private float _acceleration;
    private float _maxSpeed;

    private float _turnSpeed;
    [SerializeField] private float _turnDist = 3.5f;
    private int _curLane = 0;
    [SerializeField] private int _maxLane = 1;
    private Queue<IEnumerator> _turnQueue = new Queue<IEnumerator>();
    private Coroutine _turnCoroutine;

    [SerializeField] private UIManager _uIManager;
    [SerializeField] private SandStorm _storm;

    private int _multiplier = 1;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (parametrs.TryFindParam(eCarParameterType.maxSpeed, out var param))
            _maxSpeed = param.CurVal;
        else
            Debug.LogWarning("there is no max speed in params");

        if (parametrs.TryFindParam(eCarParameterType.turnSpeed, out param))
            _turnSpeed = param.CurVal;
        else
            Debug.LogWarning("there is no turn speed in params");
    }

    private void Update()
    {
        _passedDist = transform.position.z;

        if (_passedDist > hundredMeters * _multiplier)
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
        if (isLosed)
            Brake();
        else
            Accelerate();
    }

    private void Accelerate()
    {
        if (_rigidbody.velocity.z > _maxSpeed)
            return;

        _rigidbody.AddForce(Vector3.forward * _acceleration, ForceMode.VelocityChange);
    }

    private void Brake()
    {
        if (_rigidbody.velocity.z < _acceleration)
            return;

        _rigidbody.AddForce(Vector3.back * _acceleration, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isLosed)
            return;

        if (collision.gameObject.CompareTag(obstacleTag))
        {
            isLosed = true;
            gameOver?.Invoke();
            _storm.StartStorm();

            if (!_uIManager)
                Debug.LogError("you must assign UIManager on the inspector");
            else
            {
                _uIManager.BlockButtons();
                GameManager.S.moneyManager.Money += _uIManager.Money;
                SaveGameManager.Save();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Coin coin))
        {
            _uIManager.Money += coin.Value;
            coin.DestroyMe();
        }
    }

    public void TurnButton(bool turningRight)
    {
        if (turningRight)
            Turn(1);
        else
            Turn(-1);
    }

    private void Turn(int coef)
    {
        if (Math.Abs(_curLane + coef) > _maxLane)
            return;

        _turnQueue.Enqueue(Turn((_curLane + coef) * _turnDist));
        if (_turnCoroutine is null)
        {
            _turnCoroutine = StartCoroutine(_turnQueue.Dequeue());
        }
        _curLane += coef;
        return;
    }

    private IEnumerator Turn(float targetX)
    {
        float i = 0;
        while (i < _turnDist)
        {
            var x = Mathf.MoveTowards(transform.position.x, targetX, _turnSpeed * Time.deltaTime);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            i += _turnSpeed * Time.deltaTime;
            yield return null;
        }
        if (_turnQueue.Count > 0)
            _turnCoroutine = StartCoroutine(_turnQueue.Dequeue());
        else
            _turnCoroutine = null;
    }
}

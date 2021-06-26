using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
    public RoadSO road;

    public Vector3 startPos;

    public UIManager uIManager;

    public bool canAccelerate = false;
    [SerializeField] private float _acceleration;
    private float _maxSpeed;

    private float _turnSpeed;
    private int _curLane = 0;
    private Queue<IEnumerator> _turnQueue = new Queue<IEnumerator>();
    private Coroutine _turnCoroutine;

    public GameObject view;
    [SerializeField] private SandStorm _storm;
    [SerializeField] private GameObject _sparksVFX;

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
            uIManager.Dist += 0.1f;
            _multiplier++;
        }
    }

    private void FixedUpdate()
    {
        if (isLosed)
            Brake();
        else if (canAccelerate)
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
        {
            _sparksVFX.SetActive(false);
            return;
        }          

        _rigidbody.AddForce(Vector3.back * _acceleration, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isLosed)
            return;

        if (collision.gameObject.CompareTag(obstacleTag))
        {
            _sparksVFX.SetActive(true);
            isLosed = true;
            gameOver?.Invoke();
            _storm.StartStorm();

            if (!uIManager)
                Debug.LogError("you must assign UIManager on the inspector");
            else
            {
                uIManager.BlockButtons();
                GameManager.S.moneyManager.Money += uIManager.Money;
                SaveGameManager.Save();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Coin coin))
        {
            uIManager.Money += coin.Value;
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
        if (Math.Abs(_curLane + coef) > road.maxLane)
            return;

        _turnQueue.Enqueue(Turn((_curLane + coef) * road.laneWidth, coef));
        if (_turnCoroutine is null)
        {
            _turnCoroutine = StartCoroutine(_turnQueue.Dequeue());
        }
        _curLane += coef;
        return;
    }

    private IEnumerator Turn(float targetX, int coef)
    {
        float i = 0;
        while (i < road.laneWidth)
        {
            var xPos = Mathf.MoveTowards(transform.position.x, targetX, _turnSpeed * Time.deltaTime);
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

            float smoothCoef = _turnSpeed / _maxSpeed;
            i += _turnSpeed * Time.deltaTime;
            float x = Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, road.laneWidth, i));
            float angle = coef * Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan(1 / Mathf.Sqrt(1 - x * x)) - 90) * smoothCoef;
            view.transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }
        if (_turnQueue.Count > 0)
            _turnCoroutine = StartCoroutine(_turnQueue.Dequeue());
        else
            _turnCoroutine = null;
    }
}

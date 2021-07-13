using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class Car: MonoBehaviour
{
    public const string obstacleTag = "obstacle";
    public const int hundredMeters = 100;
    public const int kilometer = 1000;
    public const float windRate = 5;

    public static Action clashWithObstacle;
    public static Action passedHundredMeters;
    public static Action passedOneKilometer;

    public bool isLosed;

    [Header("SO references")]
    public CarParamsSO parametrs;
    public RoadSO road;   

    [HideInInspector] public UIManager uIManager;
    [HideInInspector] public VisualEffect wind;

    [Header("Car params")]
    public Vector3 startPos;
    [SerializeField] private float _accelerationSpeed = 5;
    [SerializeField] private float _brakeSpeed = 10;
    private float _maxSpeed;

    [Header("Vfx params")]
    [SerializeField] private GameObject _sparksVFX;

    [Space]
    [SerializeField] private ParticleSystem _exhaustVfx;
    [SerializeField] private int _exhaustEmmisionStart = 400;
    [SerializeField] private float _exhaustEmmisionCoef1 = .1f;
    [SerializeField] private float _exhaustEmmisionCoef2 = 5;

    private float _turnSpeed;
    private int _curLane = 0;
    private Queue<IEnumerator> _turnQueue = new Queue<IEnumerator>();
    private Coroutine _turnCoroutine;

    private int _multiplier = 1;
    private Rigidbody _rigidbody;

    private float _xPos;
    private float _passedDist;

    [SerializeField] private float _curSpeed;
    public float CurSpeed
    {
        get => _curSpeed;
        set
        {
            if (value < 0)
                value = 0;
            SetExhaustEmmision(value, value > _curSpeed);
            _curSpeed = value;
        }
    }

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

    private float CalculateZPos()
    {
        return transform.position.z + CurSpeed * Time.deltaTime;
    }

    public IEnumerator Acelerate()
    {
        _exhaustVfx.gameObject.SetActive(true);
        while (CurSpeed < _maxSpeed)
        {
            CurSpeed += _accelerationSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Brake()
    {
        while (CurSpeed > 0)
        {
            CurSpeed -= _brakeSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(new Vector3(_xPos, startPos.y, CalculateZPos()));

        //wind.SetFloat("Speed", _rigidbody.velocity.z);
        //wind.SetFloat("Spawn rate", _rigidbody.velocity.z * windRate);
    }

    private void SetExhaustEmmision(float speed, bool accelerate)
    {
        var exhaustEmmision = _exhaustVfx.emission;
        if (accelerate)
            exhaustEmmision.rateOverTime = _exhaustEmmisionStart * (_exhaustEmmisionCoef1 / (_exhaustEmmisionCoef1 + speed / _exhaustEmmisionCoef2));
        else
            exhaustEmmision.rateOverTime = _exhaustEmmisionCoef1 / (_maxSpeed - speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isLosed)
            return;

        if (collision.gameObject.CompareTag(obstacleTag))
        {
            _sparksVFX.SetActive(true);
            _sparksVFX.transform.localPosition = collision.contacts[0].point - transform.position;          
            isLosed = true;
            StartCoroutine(Brake());
            clashWithObstacle?.Invoke();

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

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(obstacleTag))
        {
            _sparksVFX.SetActive(false);
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
            var distDelta = _turnSpeed * Time.deltaTime;
            i += distDelta;
            _xPos += distDelta * coef;

            float smoothCoef = _turnSpeed / _maxSpeed;
            float x = Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, road.laneWidth, i));
            float angle = coef * Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan(1 / Mathf.Sqrt(1 - x * x)) - 90) * smoothCoef;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }
        _xPos = targetX;
        if (_turnQueue.Count > 0)
            _turnCoroutine = StartCoroutine(_turnQueue.Dequeue());
        else
            _turnCoroutine = null;
    }
}

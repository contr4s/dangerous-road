using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
public class Car: MonoBehaviour
{
    [SerializeField] private float _motorForce;
    [SerializeField] private WheelCollider[] _frontWheels;
    [SerializeField] private WheelCollider[] _backWheels;
    [SerializeField] private UIManager _uIManager;

    [SerializeField] private float _maxSteer = 30;
    [SerializeField] private Transform _com;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.centerOfMass = _com.localPosition;
        Move(_motorForce);
    }

    private void FixedUpdate()
    {
        var steer = Input.GetAxis("Horizontal");

        foreach (WheelCollider col in _frontWheels)
        {
            col.steerAngle = steer * _maxSteer; 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            _rigidbody.isKinematic = true;

            if (!_uIManager)
                Debug.LogError("you must assign UIManager on the inspector");
            else
                _uIManager.loseDisplay.gameObject.SetActive(true);
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

    private void Move(float force)
    {
        foreach (WheelCollider wheel in _backWheels)
        {
            wheel.motorTorque = force;
        }
    }
}

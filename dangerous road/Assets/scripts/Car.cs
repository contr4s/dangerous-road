using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car: MonoBehaviour
{
    [SerializeField] private float _motorForce;
    [SerializeField] private WheelCollider[] _wheels;
    [SerializeField] private UIManager _UIManager;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Move(_motorForce);
    }

    private void Move(float force)
    {
        foreach (WheelCollider wheel in _wheels)
        {
            wheel.motorTorque = force;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            _rigidbody.isKinematic = true;

            if (!_UIManager)
                Debug.LogError("you must assign UIManager on the inspector");
            else
                _UIManager.loseDisplay.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("finish"))
        {
            _rigidbody.isKinematic = true;
            if (!_UIManager)
                Debug.LogError("you must assign UIManager on the inspector");
            else
                _UIManager.winDisplay.gameObject.SetActive(true);
        }
    }
}

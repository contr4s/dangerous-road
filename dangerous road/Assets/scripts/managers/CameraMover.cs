using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMover: MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetOffset;

    [SerializeField] private float _distance = 5.0f;
    [SerializeField] private float _maxDistance = 20;
    [SerializeField] private float _minDistance = .6f;

    [SerializeField] private float _xSpeed = 200.0f;
    [SerializeField] private float _ySpeed = 200.0f;

    [SerializeField] private int _yMinLimit = 10;
    [SerializeField] private int _yMaxLimit = 80;

    [SerializeField] private float _zoomRate = 40;
    [SerializeField] private float _zoomDampening = 5.0f;

    private float _currentDistance;
    private float _desiredDistance;

    private Quaternion _currentRotation;
    private Quaternion _desiredRotation;

    private float _xDeg = 0.0f;
    private float _yDeg = 0.0f;
    private Quaternion _rotation;
    private Vector3 _position;

    void Start() {
        Init(); 
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            RotateCamera();
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            ZoomCamera();
        }

        _desiredDistance = Mathf.Clamp(_desiredDistance, _minDistance, _maxDistance);
        _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, _zoomDampening);

        _position = _target.position - (_rotation * Vector3.forward * _currentDistance + _targetOffset);
        transform.position = _position;
    }

    public void Init()
    {
        if (!_target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * _distance);
            _target = go.transform;
        }

        _currentDistance = _distance;
        _desiredDistance = _distance;
        _position = transform.position;
        _rotation = transform.rotation;
        _currentRotation = transform.rotation;
        _desiredRotation = transform.rotation;

        _xDeg = Vector3.Angle(Vector3.right, transform.right);
        _yDeg = Vector3.Angle(Vector3.up, transform.up);

    }   

    private void RotateCamera()
    {
        _xDeg += Input.GetAxis("Mouse X") * _xSpeed * 0.02f;
        _yDeg -= Input.GetAxis("Mouse Y") * _ySpeed * 0.02f;

        _yDeg = ClampAngle(_yDeg, _yMinLimit, _yMaxLimit);

        _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
        _currentRotation = transform.rotation;

        _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, 1);
        transform.rotation = _rotation;
    }

    private void ZoomCamera()
    {
        _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * _zoomRate * Mathf.Abs(_desiredDistance);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}

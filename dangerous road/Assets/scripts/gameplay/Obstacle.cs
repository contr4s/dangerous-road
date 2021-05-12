using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle: MonoBehaviour, IDestroyable
{
    [SerializeField] private float _swipeTime = 0.1f;
    [SerializeField] private float _swipeForceScale = 20;
    [SerializeField] private float _maxDistToSwipe = 150;
    [SerializeField] private float _minDistToSwipe = 15;

    [SerializeField] private float _fallTime = 10f;

    private bool _hasBeenSwipedAway = false;
    private Vector3 _cashedPosition;

    private Rigidbody _rigidbody;
    private Camera _mainCam;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void OnMouseEnter()
    {
        var distToCam = transform.position.z - _mainCam.transform.position.z;
        if (distToCam < _maxDistToSwipe && distToCam > _minDistToSwipe)
            StartCoroutine(Swipe(_swipeTime, distToCam));
    }

    private IEnumerator Swipe(float swipeTime, float distToCam)
    {
        _cashedPosition = CalulateMousePosition();
        yield return new WaitForSeconds(swipeTime);
        Vector3 newPosition = CalulateMousePosition();
        _rigidbody.AddForce((newPosition - _cashedPosition) * _swipeForceScale / distToCam, ForceMode.Impulse);
        if (!_hasBeenSwipedAway)
        {
            _hasBeenSwipedAway = true;            

            yield return new WaitForSeconds(_fallTime);
            DestroyMe();
        }
    }

    private Vector3 CalulateMousePosition()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = Vector3.Distance(_mainCam.transform.position, transform.position);
        return _mainCam.ScreenToWorldPoint(mPos);
    }

    public void DestroyMe()
    {
        gameObject.SetActive(false);
    }
}

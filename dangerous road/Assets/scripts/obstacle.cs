using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class obstacle: MonoBehaviour
{
    [SerializeField] private float _swipeTime = 0.1f;
    [SerializeField] private float _swipeForceScale = 10;
    [SerializeField] private UIManager _UIManager;

    private bool _hasBeenSwipedAway = false;
    private Vector3 _cashedPosition;

    private Rigidbody _rigidbody;
    private Camera _mainCam;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
    }

    private void OnMouseEnter()
    {
        StartCoroutine(Swipe(_swipeTime));
    }

    private IEnumerator Swipe(float swipeTime)
    {
        _cashedPosition = CalulateMousePosition();
        yield return new WaitForSeconds(swipeTime);
        Vector3 newPosition = CalulateMousePosition();
        _rigidbody.velocity = (newPosition - _cashedPosition) * _swipeForceScale;
        if (!_hasBeenSwipedAway)
        {
            if (!_UIManager)
                Debug.LogError("you must assign UIManager on the inspector");
            _UIManager.Score++;
            _hasBeenSwipedAway = true;
        }
    }

    private Vector3 CalulateMousePosition()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = Vector3.Distance(_mainCam.transform.position, transform.position);
        return _mainCam.ScreenToWorldPoint(mPos);
    }
}

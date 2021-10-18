using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle: MonoBehaviour, IDestroyable
{
    private const int swipeSoundConst = 40;

    public UIManager uIManager;
    public GameplaySoundManager soundManager;

    [SerializeField] private Vector3[] _possibleRotations;
    [SerializeField] private GameObject _outline;

    [SerializeField] private RoadSO _road;
    [SerializeField] private SwipeSO _swipeSO;
    [SerializeField] private BoxCollider _swipeCollider;
    [SerializeField] private Vector3 _defaultColliderSize;
    [SerializeField] private float _colliderYAxisMaxSize = 10;

    private Camera _mainCam;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.isKinematic = false;
        if (_possibleRotations.Length > 0)
            transform.rotation = Quaternion.Euler(_possibleRotations[Random.Range(0, _possibleRotations.Length)]);
        else
            transform.rotation = Quaternion.identity;
        _swipeCollider.size = new Vector3(_defaultColliderSize.x, _colliderYAxisMaxSize, _defaultColliderSize.z);
        StartCoroutine(ControlSwipeColiderSize());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetupOutline(bool active)
    {
        if (_outline)
            _outline.SetActive(active);
    }

    public void AddForce(float force, Vector3 direction, float distToCam)
    {
        direction.Normalize();
        var scaledForce = force / distToCam;
        _rigidbody.AddForce(direction * scaledForce, ForceMode.Impulse);
        var acceleration = scaledForce / _rigidbody.mass;
        uIManager.Points += acceleration;
        soundManager.PlaySound(eSoundType.obstacleSwipe);
        soundManager.ChangeSoundVolume(eSoundType.obstacleSwipe, Mathf.Lerp(0, 1, Mathf.InverseLerp(0, swipeSoundConst, acceleration)));
    }

    public void DestroyMe()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator DestroyAfterSwipe(float activeTimeAfterSwipe)
    {
        yield return new WaitForSeconds(activeTimeAfterSwipe);
        DestroyMe();
    }

    private IEnumerator ControlSwipeColiderSize()
    {
        if (_swipeCollider is null)
            yield break;
        if (!_swipeCollider.isTrigger)
            Debug.LogWarning("swipe collider must be trigger");
        var distToCam = transform.position.z;
        while (distToCam > _swipeSO.distWhereObstacleHasNormalColliderSize)
        {
            distToCam = transform.position.z - _mainCam.transform.position.z;
            if (distToCam <= _swipeSO.distWhereObstacleHasMaxColliderSize)
            {
                var size = _defaultColliderSize;
                var scaledDistToCam = Mathf.InverseLerp(_swipeSO.distWhereObstacleHasNormalColliderSize, _swipeSO.distWhereObstacleHasMaxColliderSize, distToCam);
                size.y = Mathf.Lerp(_defaultColliderSize.y, _colliderYAxisMaxSize, scaledDistToCam);
                size.x = Mathf.Lerp(_defaultColliderSize.x, _road.laneWidth, scaledDistToCam);
                _swipeCollider.size = size;
            }
            yield return null;
        }
    }
}

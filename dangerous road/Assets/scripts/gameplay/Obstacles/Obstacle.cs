using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle: MonoBehaviour, IDestroyable
{
    private const int swipeSoundConst = 40;

    public float Weight => _rigidbody.mass;
    public bool HasBeenSwipedAway { get; set; }

    public SpawnedObjectsManager SpawnedObjectsManager { get; set; }
    public UIManager UIManager { get; set; }
    public GameplaySoundManager SoundManager { get; set; }

    [SerializeField] private Vector3[] _possibleRotations;
    [SerializeField] private GameObject _outline;

    [SerializeField] private SwipeSO _swipeSO;
    [SerializeField] private BoxCollider _swipeCollider;
    [SerializeField] private Vector3 _defaultColliderSize;

    private Vector3 _maxColliderSize;
    private Camera _mainCam;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        Init();
    }

    protected virtual void Init()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.isKinematic = false;
        if (_possibleRotations.Length > 0)
            transform.rotation = Quaternion.Euler(_possibleRotations[Random.Range(0, _possibleRotations.Length)]);
        else
            transform.rotation = Quaternion.identity;
        _maxColliderSize = _swipeCollider.size;
        StartCoroutine(ControlSwipeColiderSize());
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
        UIManager.Points += acceleration;
        SoundManager.PlaySound(eSoundType.obstacleSwipe);
        SoundManager.ChangeSoundVolume(eSoundType.obstacleSwipe, Mathf.Lerp(0, 1, Mathf.InverseLerp(0, swipeSoundConst, acceleration)));
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
                var scaledDistToCam = Mathf.InverseLerp(_swipeSO.distWhereObstacleHasNormalColliderSize, _swipeSO.distWhereObstacleHasMaxColliderSize, distToCam);
                var size = Vector3.Lerp(_defaultColliderSize, _maxColliderSize, scaledDistToCam);
                _swipeCollider.size = size;
            }
            yield return null;
        }
    }
}

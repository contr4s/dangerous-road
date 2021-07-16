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

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = Vector3.zero;
        if (_possibleRotations.Length > 0)
            transform.rotation = Quaternion.Euler(_possibleRotations[Random.Range(0, _possibleRotations.Length)]);
        else
            transform.rotation = Quaternion.identity;
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
}

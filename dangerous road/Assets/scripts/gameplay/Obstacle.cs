using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle: MonoBehaviour, IDestroyable
{
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
        _rigidbody.AddForce(direction * force / distToCam, ForceMode.Impulse);
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

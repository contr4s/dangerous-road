using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle: MonoBehaviour, IDestroyable
{
    [SerializeField] private SwipeSO _swipeSO;

    [SerializeField] private float _fallTime = 10f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void AddForce(Vector3 direction, float distToCam)
    {
        direction.Normalize();
        _rigidbody.AddForce(direction * _swipeSO.swipeForceScale / distToCam, ForceMode.Impulse);
    }

    public void DestroyMe()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator DestroyAfterSwipe()
    {
        yield return new WaitForSeconds(_fallTime);
        DestroyMe();
    }
}

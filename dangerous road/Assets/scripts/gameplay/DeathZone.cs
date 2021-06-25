using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowCar))]
public class DeathZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDestroyable destroyable))
            destroyable.DestroyMe();
        if (other.CompareTag("road"))
            StartCoroutine(ReturnToPool(other.gameObject));
        if (other.CompareTag("environment"))
            StartCoroutine(ReturnToPool(other.gameObject));
    }

    private IEnumerator ReturnToPool(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        go.SetActive(false);
    }
}

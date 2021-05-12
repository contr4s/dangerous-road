using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDestroyable destroyable))
            destroyable.DestroyMe();
        if (other.CompareTag("road"))
            other.gameObject.SetActive(false);
        if (other.CompareTag("environment"))
            other.gameObject.SetActive(false);
    }
}

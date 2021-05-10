using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print("df");
        if (collision.gameObject.TryGetComponent(out IDestroyable destroyable))
            destroyable.DestroyMe();
        if (collision.gameObject.CompareTag("road"))
            collision.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDestroyable destroyable))
            destroyable.DestroyMe();
        if (other.CompareTag("road"))
            other.gameObject.SetActive(false);
    }
}

using UnityEngine;

class AbilitiesManager: MonoBehaviour
{
    [SerializeField] Invisibility _invisibility;

    private void Awake()
    {
        _invisibility.Init();
    }
}


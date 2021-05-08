using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>: MonoBehaviour where T : Component
{
    public T spawnObjectPrefab;

    [SerializeField] private int _poolDepth;
    [SerializeField] private bool _canGrow = true;

    private List<T> _pool = new List<T>();

    private void Awake()
    {
        for (int i = 0; i < _poolDepth; i++)
        {
            T pooledObject = Instantiate(spawnObjectPrefab);
            pooledObject.gameObject.SetActive(false);
            _pool.Add(pooledObject);
            pooledObject.transform.parent = gameObject.transform;
        }
    }

    public T GetAvailableObject()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].gameObject.activeInHierarchy)
                return _pool[i];
        }

        if (_canGrow == true)
        {
            T pooledObject = Instantiate(spawnObjectPrefab);
            pooledObject.gameObject.SetActive(false);
            _pool.Add(pooledObject);

            return pooledObject;
        }
        else
        {
            Debug.LogError("не хватает глубины пула");
            return null;
        }
    }
}

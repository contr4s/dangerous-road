using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : Component
{
    protected event Action<T> OnSpawn;

    [SerializeField] protected SpawnedObjectsManager _manager;
    [SerializeField] protected float[] _xAxisSpawnPositions;
    [SerializeField] protected float _distanceToCam;

    [SerializeField] protected Vector3 _startPosition;
    [SerializeField] protected float _startStep = 50;

    protected float _lastSpawnedPos;   

    protected virtual void Start()
    {
        SpawnObjects(_startPosition.z, _distanceToCam, _startStep);       
    }

    protected void SpawnObjects(float startPos, float lastPos, float step, float xPos = float.MaxValue)
    {
        float zPos = startPos;
        for (; zPos < lastPos; zPos += step)
        {
            if (xPos == float.MaxValue)
            {
                SpawnObject(new Vector3(GetRandomXPos(), _startPosition.y, zPos));
            }
            else
            {
                SpawnObject(new Vector3(xPos, _startPosition.y, zPos));
                SpawnObject(new Vector3(-xPos, _startPosition.y, zPos));
            }
        }
        _lastSpawnedPos = zPos;
    }

    protected abstract ObjectPool<T> GetObjectPool();   
    protected virtual void InitObject(T gameObject) { }

    protected abstract bool IsSpawnedOnRoad { get; }

    private void SpawnObject(Vector3 position)
    {
        var spawnedObject = GetObjectPool().GetAvailableObject();

        InitObject(spawnedObject);
        spawnedObject.transform.position = position;
        spawnedObject.gameObject.SetActive(true);
        if (IsSpawnedOnRoad)
        {
            spawnedObject.transform.SetParent(_manager.FindAppropriateLane(position));           
        }
        OnSpawn?.Invoke(spawnedObject);
    }

    private float GetRandomXPos()
    {
        return _xAxisSpawnPositions[UnityEngine.Random.Range(0, _xAxisSpawnPositions.Length)];
    }
}

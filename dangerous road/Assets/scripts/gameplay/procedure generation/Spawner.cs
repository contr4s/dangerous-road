using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : Component
{   
    [SerializeField] protected float[] _xAxisSpawnPositions;
    [SerializeField] protected float _distanceToCam;
    [SerializeField] protected float _timeBetweenSpawning;

    [SerializeField] protected Vector3 _startPosition;
    [SerializeField] protected float _startStep;

    protected float _lastSpawnedPos;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    protected virtual void Start()
    {
        LevelManager.UnPause();
        SpawnObjects(_startPosition.z, _distanceToCam, _startStep);       
    }

    protected void SpawnObjects(float startPos, float lastPos, float step)
    {
        float zPos = startPos;
        for (; zPos < lastPos; zPos += step)
        {
            SpawnObject(zPos);
        }
        _lastSpawnedPos = zPos;
    }

    protected abstract ObjectPool<T> GetObjectPool();   

    protected virtual IEnumerator SpawnObjects()
    {
        while (true)
        {
            SpawnObject(_mainCam.transform.position.z + _distanceToCam);
            yield return new WaitForSeconds(_timeBetweenSpawning);
        }
    }

    private void SpawnObject(float zPos)
    {
        SpawnObject(new Vector3(GetRandomXPos(), _startPosition.y, zPos));
    }

    private void SpawnObject(Vector3 position)
    {
        var obstacle = GetObjectPool().GetAvailableObject();

        obstacle.transform.position = position;
        obstacle.gameObject.SetActive(true);
    }

    private float GetRandomXPos()
    {
        return _xAxisSpawnPositions[Random.Range(0, _xAxisSpawnPositions.Length)];
    }
}

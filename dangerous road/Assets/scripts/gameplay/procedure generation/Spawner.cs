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

    [SerializeField] protected float _lastSpawnedPos;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    protected virtual void Start()
    {
        SpawnObjects(_startPosition.z, _distanceToCam, _startStep);       
    }

    protected void SpawnObjects(float startPos, float lastPos, float step)
    {
        float zPos = startPos;
        for (; zPos < lastPos; zPos += step)
        {
            SpawnObstacle(zPos);
        }
        _lastSpawnedPos = zPos;
    }

    protected abstract ObjectPool<T> GetObjectPool();   

    protected virtual IEnumerator SpawnObjects()
    {
        while (true)
        {
            SpawnObstacle(_mainCam.transform.position.z + _distanceToCam);
            yield return new WaitForSeconds(_timeBetweenSpawning);
        }
    }

    private void SpawnObstacle(float zPos)
    {
        SpawnObstacle(new Vector3(GetRandomXPos(), _startPosition.y, zPos));
    }

    private void SpawnObstacle(Vector3 position)
    {
        //var obstacle = _obstaclePools[Random.Range(0, _obstaclePools.Length)].GetAvailableObject();
        var obstacle = GetObjectPool().GetAvailableObject();

        obstacle.transform.position = position;
        obstacle.gameObject.SetActive(true);
    }

    private float GetRandomXPos()
    {
        return _xAxisSpawnPositions[Random.Range(0, _xAxisSpawnPositions.Length)];
    }
}

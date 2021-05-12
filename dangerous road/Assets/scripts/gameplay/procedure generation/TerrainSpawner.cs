using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner: Spawner<Terrain>
{
    [SerializeField] TerrainPool[] _terrainPools;


    [SerializeField] private float _distToCover;

    [SerializeField] private Car _car;

    private int multiplier = 1;

    private void Update()
    {
        if (_car.passedDist > _distToCover * multiplier)
        {
            SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + _distToCover, _startStep);
            multiplier++;
        }
    }

    protected override ObjectPool<Terrain> GetObjectPool()
    {
        return _terrainPools[Random.Range(0, _terrainPools.Length)];
    }
}

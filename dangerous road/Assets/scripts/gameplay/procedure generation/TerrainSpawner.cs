using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner: Spawner<Terrain>
{
    [SerializeField] TerrainPool[] _terrainPools;

    [SerializeField] private float _distToCover;

    [SerializeField] private Car _car;

    private void OnEnable()
    {
        _car.passedOneKilometer += () => SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.kilometer, _startStep);
    }

    protected override ObjectPool<Terrain> GetObjectPool()
    {
        return _terrainPools[Random.Range(0, _terrainPools.Length)];
    }
}

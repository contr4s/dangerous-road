using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner: Spawner<Terrain>
{
    [SerializeField] TerrainPool[] _terrainPools;

    [SerializeField] private float _distToCover;

    private void OnEnable()
    {
        Car.passedOneKilometer += Spawn;
    }

    private void OnDisable()
    {
        Car.passedOneKilometer -= Spawn;
    }

    protected override ObjectPool<Terrain> GetObjectPool()
    {
        return _terrainPools[Random.Range(0, _terrainPools.Length)];
    }

    void Spawn()
    {
        SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.kilometer, _startStep);
    }
}

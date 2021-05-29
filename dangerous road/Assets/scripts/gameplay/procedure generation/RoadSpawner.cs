using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner: Spawner<MeshRenderer>
{
    [SerializeField] private RoadPool _roadPool;

    private void OnEnable()
    {
        Car.passedHundredMeters += Spawn;
    }

    private void OnDisable()
    {
        Car.passedHundredMeters -= Spawn;
    }

    protected override ObjectPool<MeshRenderer> GetObjectPool()
    {
        return _roadPool;
    }

    private void Spawn()
    {
        SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.hundredMeters, _startStep);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner: Spawner<Transform>
{
    [SerializeField] private RoadPool _roadPool;

    protected override bool IsSpawnedOnRoad => true;

    private void OnEnable()
    {
        Car.passedHundredMeters += Spawn;
    }

    private void OnDisable()
    {
        Car.passedHundredMeters -= Spawn;
    }

    protected override ObjectPool<Transform> GetObjectPool()
    {
        return _roadPool;
    }

    private void Spawn()
    {
        SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.hundredMeters, _startStep);
    }
}

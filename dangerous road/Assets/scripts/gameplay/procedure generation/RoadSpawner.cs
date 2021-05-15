using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner: Spawner<MeshRenderer>
{
    [SerializeField] private RoadPool _roadPool;

    [SerializeField] private float _distToCover;

    [SerializeField] private Car _car;

    private void OnEnable()
    {
        _car.passedHundredMeters += () => SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.hundredMeters, _startStep);
    }

    protected override ObjectPool<MeshRenderer> GetObjectPool()
    {
        return _roadPool;
    }    
}

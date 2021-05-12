using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner: Spawner<MeshRenderer>
{
    [SerializeField] private RoadPool _roadPool;

    [SerializeField] private float _distToCover;

    [SerializeField] private Car _car;

    private int multiplier = 1;

    private void OnEnable()
    {
        _car.passedHundredMeters += () => SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.hundredMeters, _startStep);
    }

    //private void Update()
    //{
    //    if (_car.passedDist > _distToCover * multiplier)
    //    {
    //        SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + _distToCover, _startStep);
    //        multiplier++;
    //    }           
    //}

    protected override ObjectPool<MeshRenderer> GetObjectPool()
    {
        return _roadPool;
    }    
}

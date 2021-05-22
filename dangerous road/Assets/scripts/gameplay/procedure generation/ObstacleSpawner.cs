using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner: Spawner<Obstacle>
{
    [Range(0, 100)]
    public float complexityPercentage1 = 5;
    [Range(0, 100)]
    public float complexityPercentage2 = 10;

    [SerializeField] private ObstaclePool[] _obstaclePools;

    private void OnEnable()
    {
        Car.passedHundredMeters += () => _timeBetweenSpawning *= 1 - complexityPercentage1 / 100;
        Car.passedOneKilometer += () => _timeBetweenSpawning *= 1 - complexityPercentage2 / 100;
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(SpawnObjects());
    }

    protected override ObjectPool<Obstacle> GetObjectPool()
    {
        return _obstaclePools[Random.Range(0, _obstaclePools.Length)];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner: Spawner<Obstacle>
{
    [SerializeField] private ObstaclePool[] _obstaclePools;

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

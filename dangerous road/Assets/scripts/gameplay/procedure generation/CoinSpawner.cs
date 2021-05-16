using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner: Spawner<Coin>
{
    [SerializeField] private CoinPool[] _coinPools;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(SpawnObjects());
    }

    protected override ObjectPool<Coin> GetObjectPool()
    {
        return _coinPools[Random.Range(0, _coinPools.Length)];
    }
}

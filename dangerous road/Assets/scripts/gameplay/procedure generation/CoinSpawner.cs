using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner: Spawner<Coin>
{
    [SerializeField] private CoinPool[] _coinPools;
    [SerializeField] private float _step;

    [Range(0, 100)]
    public float complexityPercentage100m = 5;
    [Range(0, 100)]
    public float complexityPercentage1km = 10;

    private void OnEnable()
    {
        _step = _startStep;
        Car.passedHundredMeters += Spawn;
        Car.passedHundredMeters += () => ChangeComplexity(complexityPercentage100m);
        Car.passedOneKilometer += () => ChangeComplexity(complexityPercentage1km);
    }

    private void OnDisable()
    {
        Car.passedHundredMeters -= Spawn;
    }

    protected override ObjectPool<Coin> GetObjectPool()
    {
        return _coinPools[Random.Range(0, _coinPools.Length)];
    }

    private void ChangeComplexity(float percentage)
    {
        _step *= 1 - percentage / 100;
    }

    private void Spawn()
    {
        _lastSpawnedPos -= _lastSpawnedPos % 100; //to control grow of last spawned pos
        SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.hundredMeters, _step);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObstacleType
{
    light,
    medium,
    heavy
}

[Serializable]
public struct ObstacleVariant
{
    public eObstacleType type;
    public float spawnChance;
    public ObstaclePool[] pools;
}

public class ObstacleSpawner: Spawner<Obstacle>
{
    [Range(0, 100)]
    public float complexityPercentage100m = 5;
    [Range(0, 100)]
    public float complexityPercentage1km = 10;

    private int _hundredMetersCounter = 1;
    private int _kilometerCounter = 1;
    [SerializeField] private float _step;

    [SerializeField] private ObstacleVariant[] _variants;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private GameplaySoundManager _soundManager;

    private void OnEnable()
    {
        _step = _startStep;
        Car.passedHundredMeters += Spawn;
        Car.passedHundredMeters += () => ChangeComplexity(complexityPercentage100m, ref _hundredMetersCounter);
        Car.passedOneKilometer += () => ChangeComplexity(complexityPercentage1km, ref _kilometerCounter);
    }

    private void OnDisable()
    {
        Car.passedHundredMeters -= Spawn;
    }

    protected override ObjectPool<Obstacle> GetObjectPool()
    {
        float sumChance = 0;
        for (int i = 0; i < _variants.Length; i++)
        {
            sumChance += _variants[i].spawnChance;
        }
        float randNum = UnityEngine.Random.Range(0, sumChance);
        for (int i = 0; i < _variants.Length; i++)
        {
            if (randNum < _variants[i].spawnChance)
                return _variants[i].pools[UnityEngine.Random.Range(0, _variants[i].pools.Length)];
            else
                randNum -= _variants[i].spawnChance;
        }
        int j = _variants.Length - 1;
        return _variants[j].pools[UnityEngine.Random.Range(0, _variants[j].pools.Length)];
    }

    protected override void InitObject(Obstacle gameObject)
    {
        gameObject.uIManager = _uIManager;
        gameObject.soundManager = _soundManager;
    }

    private void ChangeComplexity(float percentage, ref int counter)
    {
        _step *= 1 - percentage / 100 / counter;
        counter++;
    }

    private void Spawn()
    {
        _lastSpawnedPos -= _lastSpawnedPos % 100; //to control grow of last spawned pos
        SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.hundredMeters, _step);
    }
}

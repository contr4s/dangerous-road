using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnviromentChunk
{
    public EnviromentChunckPool pool;
    [Range(1, 100)]
    public int spawnChance;
}

public class EnviromentChunkSpawner: Spawner<Transform>
{
    [SerializeField]
    private EnviromentChunk[] _chunks;
    [SerializeField] private float _xPos;

    private void OnEnable()
    {
        Car.passedHundredMeters += Spawn;
    }

    private void OnDisable()
    {
        Car.passedHundredMeters -= Spawn;
    }

    protected override void Start()
    {
        SpawnObjects(_startPosition.z, _distanceToCam, _startStep, _xPos);
    }

    protected override ObjectPool<Transform> GetObjectPool()
    {      
        float sumChance = CalculateSumChance();
        float randNum = UnityEngine.Random.Range(0, sumChance);
        for (int i = 0; i < _chunks.Length; i++)
        {
            if (randNum <= _chunks[i].spawnChance)
                return _chunks[i].pool;
            else
                randNum -= _chunks[i].spawnChance;
        }
        return _chunks[_chunks.Length - 1].pool;
    }

    private float CalculateSumChance()
    {
        float sumChance = 0;
        for (int i = 0; i < _chunks.Length; i++)
        {
            sumChance += _chunks[i].spawnChance;
        }
        return sumChance;
    }

    void Spawn()
    {
        SpawnObjects(_lastSpawnedPos, _lastSpawnedPos + Car.hundredMeters, _startStep, _xPos);
    }
}

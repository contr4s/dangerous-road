using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner: MonoBehaviour
{
    [SerializeField] private ObstaclePool[] _obstaclePools;

    [SerializeField] private float[] _xAxisSpawnPositions;
    [SerializeField] private float _distanceToCam;
    [SerializeField] private float _timeBetweenSpawning;

    [SerializeField] private Vector3 _startObstaclePosition;
    [SerializeField] private float _startObstaclesStep;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Start()
    {
        for (float zPos = _startObstaclePosition.z; zPos < _distanceToCam; zPos += _startObstaclesStep)
        {
            SpawnObstacle(zPos);
        }

        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            SpawnObstacle(_mainCam.transform.position.z + _distanceToCam);
            yield return new WaitForSeconds(_timeBetweenSpawning);
        }
    }

    private void SpawnObstacle(float zPos)
    {
        SpawnObstacle(new Vector3(GetRandomXPos(), _startObstaclePosition.y, zPos));
    }

    private void SpawnObstacle(Vector3 position)
    {
        var obstacle = _obstaclePools[Random.Range(0, _obstaclePools.Length)].GetAvailableObject();

        obstacle.transform.position = position;
        obstacle.gameObject.SetActive(true);
    }

    private float GetRandomXPos()
    {
        return _xAxisSpawnPositions[Random.Range(0, _xAxisSpawnPositions.Length)];
    }
}

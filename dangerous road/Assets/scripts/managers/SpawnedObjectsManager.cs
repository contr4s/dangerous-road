using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectsManager: MonoBehaviour
{
    public GameObject[] Lanes { get => _lanes; }
    public Dictionary<GameObject, List<Obstacle>> ObstaclesOnLane => _obstaclesOnLane;

    [SerializeField] private RoadSO road;
    [SerializeField] private GameObject[] _lanes;
    [SerializeField] private float _laneMovingSpeed = 3;

    private readonly Dictionary<GameObject, List<Obstacle>> _obstaclesOnLane = new Dictionary<GameObject, List<Obstacle>>();

    private void Awake()
    {
        foreach (var lane in _lanes)
            _obstaclesOnLane.Add(lane, new List<Obstacle>());
    }

    public Transform FindAppropriateLane(Vector3 position)
    {
        if (position.x < road.laneWidth / 2 && position.x > -road.laneWidth / 2)
            return Lanes[1].transform;
        else if (position.x > road.laneWidth / 2 && position.x < road.laneWidth + road.laneWidth / 2)
            return Lanes[2].transform;
        else if (position.x < -road.laneWidth / 2 && position.x > -road.laneWidth - road.laneWidth / 2)
            return Lanes[0].transform;
        else
        {
            Debug.LogWarning($"can't find lane for this x position: {position}");
            return null;
        }
    }  

    public bool TrySwapLanes(int laneindex, float maxWeight, float startZPos, float endZPos)
    {
        if (!CheckIfCanRaiseLane(laneindex, maxWeight, startZPos, endZPos))
            return false;
        StartCoroutine(MoveLane(_lanes[1].transform, 0, _lanes[laneindex].transform.position.x));
        StartCoroutine(MoveLane(_lanes[laneindex].transform, _lanes[laneindex].transform.position.x, 0));
        var tmp = _lanes[1];
        _lanes[1] = _lanes[laneindex];
        _lanes[laneindex] = tmp;
        return true;
    }

    private bool CheckIfCanRaiseLane(int LaneIndex, float maxWeight, float startZPos, float endZPos)
    {
        float totalWeight = 0;
        foreach (var obstacle in _obstaclesOnLane[_lanes[LaneIndex]])
        {
            if (obstacle.transform.position.z > startZPos && obstacle.transform.position.z < endZPos)
                totalWeight += obstacle.Weight;
        }
        return totalWeight < maxWeight;
    }

    private IEnumerator MoveLane(Transform lane, float startPos, float endPos)
    {
        while (!Mathf.Approximately(lane.position.x, endPos))
        {
            lane.position = new Vector3(Mathf.Lerp(startPos, endPos, _laneMovingSpeed), 0, 0);
            yield return null;
        }
    }
}

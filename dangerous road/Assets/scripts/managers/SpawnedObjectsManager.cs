using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectsManager : MonoBehaviour
{
    [SerializeField] private RoadSO road;
    [SerializeField] private GameObject[] _lanes;
    [SerializeField] private float _laneMovingSpeed = 3;
    public GameObject[] Lanes { get => _lanes; }

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
            Debug.LogError($"can't find lane for this x position: {position}");
            return null;
        }
    }

    public void SwapLanes(int index)
    {
        StartCoroutine(MoveLane(_lanes[1].transform, 0, _lanes[index].transform.position.x));
        StartCoroutine(MoveLane(_lanes[index].transform, _lanes[index].transform.position.x, 0));
        var tmp = _lanes[1];
        _lanes[1] = _lanes[index];
        _lanes[index] = tmp;
    }

    private IEnumerator MoveLane(Transform lane, float startPos, float endPos)
    {
        while(!Mathf.Approximately(lane.position.x, endPos))
        {
            lane.position = new Vector3(Mathf.Lerp(startPos, endPos, _laneMovingSpeed), 0, 0);
            yield return null;
        }
    }
}

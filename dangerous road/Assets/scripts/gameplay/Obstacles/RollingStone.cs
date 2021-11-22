using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStone : Obstacle
{
    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _timeDelay = 10f;
    [SerializeField] private RoadSO _road;

    protected override void Init()
    {
        base.Init();
        StartCoroutine(PeriodicalMovement());
    }

    private IEnumerator PeriodicalMovement()
    {
        var waitTime = new WaitForSeconds(_timeDelay);
        while (true)
        {
            yield return waitTime;
            yield return StartCoroutine(Move(CalculateTargetPosition()));           
        }
    }

    private float CalculateTargetPosition()
    {
        if (Mathf.Approximately(transform.position.x, 0))
            return GetRandomDirection();
        else
            return 0;
    }

    private float GetRandomDirection()
    {
        var rand = Random.Range(0, 2);
        if (rand == 0)
            rand = -1;
        return rand * _road.laneWidth;
    }

    private IEnumerator Move(float targetPosX)
    {
        transform.SetParent(transform.root);
        while (!Mathf.Approximately(transform.position.x, targetPosX) && !HasBeenSwipedAway)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosX, transform.position.y, transform.position.z), _speed * Time.deltaTime);
            transform.SetParent(SpawnedObjectsManager.FindAppropriateLane(transform.position));
            yield return null;
        }
        if (!HasBeenSwipedAway)
        {
            transform.SetParent(SpawnedObjectsManager.FindAppropriateLane(transform.position));
        }
    }
}

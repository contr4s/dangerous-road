using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    [SerializeField] protected SwipeSO _swipeSO;

    protected Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        CarSpawnManager.carSpawned += Init;
    }

    private void OnDisable()
    {
        CarSpawnManager.carSpawned -= Init;
    }

    protected bool CheckIfCanSwipeObstacle(Vector2 mousePos, out Obstacle obstacle, out float distToObstacle)
    {
        obstacle = null;
        distToObstacle = 0;
        Ray ray = _mainCam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, _swipeSO.maxDistToSwipe))
        {
            if (hit.distance < _swipeSO.minDistToSwipe)
                return false;

            if (hit.collider.TryGetComponent(out Obstacle _obstacle))
            {
                distToObstacle = hit.distance;
                obstacle = _obstacle;
                return true;
            }
        }
        return false;
    }

    private void Init(Car car)
    {
        if (car.parametrs.TryFindParam(eCarParameterType.swipeForce, out var param))
            _swipeSO.swipeForceScale = param.CurVal;
        else
            Debug.LogWarning("there is no max swipeForce in car params");

        if (car.parametrs.TryFindParam(eCarParameterType.maxSwipeDist, out param))
            _swipeSO.maxDistToSwipe = param.CurVal;
        else
            Debug.LogWarning("there is no maxSwipeDist in car params");
    }
}

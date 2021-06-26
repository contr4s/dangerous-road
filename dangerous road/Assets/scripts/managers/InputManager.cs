using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    [SerializeField] protected SwipeSO _swipeSO;
    [SerializeField] protected float swipeForceScale = 20;
    [SerializeField] protected float maxDistToSwipe = 150;

    protected Obstacle _targetObstacle;
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

    protected bool CheckIfCanSwipeObstacle(Vector2 mousePos, out Obstacle obstacle)
    {
        obstacle = null;
        Ray ray = _mainCam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistToSwipe))
        {
            if (hit.distance < _swipeSO.minDistToSwipe)
                return false;

            if (hit.collider.TryGetComponent(out Obstacle _obstacle))
            {
                obstacle = _obstacle;              
                return true;
            }
        }
        return false;
    }

    protected void SetupTargetObstacle(Obstacle obstacle)
    {
        if (!(_targetObstacle is null))
            return;
        _targetObstacle = obstacle;
        _targetObstacle.SetupOutline(true);
    }

    protected abstract Vector3 CalculateSwipeDirection();

    protected void SwipeOutObstacle()
    {
        if (_targetObstacle is null)
            return;

        var direction = CalculateSwipeDirection();
        _targetObstacle.AddForce(swipeForceScale, direction, _targetObstacle.transform.position.z - _mainCam.transform.position.z);
        StartCoroutine(_targetObstacle.DestroyAfterSwipe(_swipeSO.activeTimeAfterSwipe));
        _targetObstacle.SetupOutline(false);
        _targetObstacle = null;
    }

    private void Init(Car car)
    {
        if (car.parametrs.TryFindParam(eCarParameterType.swipeForce, out var param))
            swipeForceScale = param.CurVal;
        else
            Debug.LogWarning("there is no max swipeForce in car params");

        if (car.parametrs.TryFindParam(eCarParameterType.maxSwipeDist, out param))
            maxDistToSwipe = param.CurVal;
        else
            Debug.LogWarning("there is no maxSwipeDist in car params");
    }
}

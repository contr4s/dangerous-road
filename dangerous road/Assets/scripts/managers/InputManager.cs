using System.Collections;
using System.Collections.Generic;
using QuantumTek.QuantumUI;
using UnityEngine;

public enum eTurnInputType
{
    touch,
    swipe
}

public abstract class InputManager: MonoBehaviour
{
    private const float delayBeforeStopObstacleSelectSound = .5f;

    public static eTurnInputType turnInputType;

    [SerializeField] protected SpawnedObjectsManager spawnedObjectsManager;
    [SerializeField] private LayerMask _obstacleLayerMask = 64;

    [Header("Swipe Params")]
    [SerializeField] protected SwipeSO _swipeSO;
    [SerializeField] protected float swipeForceScale = 20;
    [SerializeField] protected float maxDistToSwipe = 150;
    [Header("Lanes Swap Params")]
    [Range(0, 1)]
    [SerializeField] protected float carInputBound = .3f;
    [SerializeField] protected float maxWeight = 1000;
    [SerializeField] protected float farDist = 300;

    protected Obstacle _targetObstacle;
    protected Camera _mainCam;
    protected Car _car;

    [SerializeField] private QUI_OptionList _optionList;
    [SerializeField] private SerializableDictionary<int, eTurnInputType> _optionListParamsMap;

    [SerializeField] private GameplaySoundManager _soundManager;

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

    private void Start()
    {
        foreach (var pair in _optionListParamsMap)
        {
            if (pair.Value == turnInputType)
                _optionList.SetOption(pair.Key);
        }       
    }

    public void ChangeTurnInputType(int optionIndex)
    {
        turnInputType = _optionListParamsMap[optionIndex];
    }

    protected bool CheckIfCanSwipeObstacle(Vector2 mousePos, out Obstacle obstacle)
    {
        obstacle = null;
        Ray ray = _mainCam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistToSwipe, _obstacleLayerMask))
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
        _soundManager.PlaySound(eSoundType.obsatacleSelect);
    }

    protected abstract Vector3 CalculateSwipeDirection();

    protected void SwipeOutObstacle()
    {
        if (_targetObstacle is null)
            return;

        var direction = CalculateSwipeDirection();
        if (direction == Vector3.zero)
        {
            direction.x = Random.value;
            direction.y = Random.value;
            direction.Normalize();
        }
        _targetObstacle.AddForce(swipeForceScale, direction, _targetObstacle.transform.position.z - _mainCam.transform.position.z);
        _targetObstacle.transform.SetParent(transform.root);
        StartCoroutine(_targetObstacle.DestroyAfterSwipe(_swipeSO.activeTimeAfterSwipe));
        _targetObstacle.SetupOutline(false);
        _soundManager.StopSoundAfterDelay(delayBeforeStopObstacleSelectSound, eSoundType.obsatacleSelect);
        _targetObstacle = null;
    }

    protected bool CanTurnCar(Vector2 mousePos)
    {
        return (mousePos.y / Screen.height < carInputBound) && CarSpawnManager.canTurn;
    }
    protected bool IsMousePosInCarInputBounds(Vector2 mousePos)
    {
        return mousePos.y / Screen.height < carInputBound;
    }

    protected bool TryTurnCar(Vector2 mousePos)
    {
        if (!CanTurnCar(mousePos))
            return false;
        if (turnInputType == eTurnInputType.touch)
            return CarSpawnManager.TryTurn(mousePos.x > Screen.width / 2);
        else
        {
            return CarSpawnManager.TryTurn(CalculateSwipeDirection().x > 0);
        }

    }

    private void Init(Car car)
    {
        _car = car;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneInputManager: MonoBehaviour
{
    [SerializeField] private SwipeSO _swipeSO;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        CarManager.carSpawned += Init;
    }

    private void OnDisable()
    {
        CarManager.carSpawned -= Init;
    }

    void Update()
    {
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _swipeSO.maxDistToSwipe))
        {
            if (hit.distance < _swipeSO.minDistToSwipe)
                return;

            if (hit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(Swipe(obstacle, _swipeSO.swipeTime, hit.distance));
            }
        }        
    }

    public IEnumerator Swipe(Obstacle obstacle, float swipeTime, float distToCam)
    {
        var _cashedPosition = CalulateMousePosition();
        yield return new WaitForSeconds(swipeTime);
        Vector3 newPosition = CalulateMousePosition();
        Vector3 direction = (newPosition - _cashedPosition).normalized;
        obstacle.AddForce(_swipeSO.swipeForceScale, direction, distToCam);
        StartCoroutine(obstacle.DestroyAfterSwipe());
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

    private Vector3 CalulateMousePosition()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = Vector3.Distance(_mainCam.transform.position, transform.position);
        return _mainCam.ScreenToWorldPoint(mPos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    [SerializeField] private SwipeSO _swipeSO;
    private Obstacle _targetObstacle;
    private Vector2 _prevTouchPos;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = _mainCam.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit, _swipeSO.maxDistToSwipe))
                {
                    if (hit.distance < _swipeSO.minDistToSwipe)
                        return;

                    if (hit.collider.TryGetComponent(out Obstacle obstacle))
                    {
                        _targetObstacle = obstacle;
                        _prevTouchPos = touch.position;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (_targetObstacle is null)
                    return;

                Vector3 direction;
                direction.x = touch.position.x - _prevTouchPos.x;
                direction.y = touch.position.y - _prevTouchPos.y;
                direction.z = 0;
                _targetObstacle.AddForce(direction, _targetObstacle.transform.position.z - _mainCam.transform.position.z);
                StartCoroutine(_targetObstacle.DestroyAfterSwipe());
                _targetObstacle = null;
            }
        }
    }
}
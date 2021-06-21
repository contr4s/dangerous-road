using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager : InputManager
{
    private Obstacle _targetObstacle;
    private Vector2 _prevTouchPos;

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            var touch = Input.GetTouch(i);
            if (_targetObstacle is null)
            {
                if (CheckIfCanSwipeObstacle(Input.mousePosition, out var obstacle, out float dist))
                {
                    _targetObstacle = obstacle;
                    _prevTouchPos = touch.position;
                }

            }
            else 
            {
                Vector3 direction;
                direction.x = touch.position.x - _prevTouchPos.x;
                direction.y = touch.position.y - _prevTouchPos.y;
                direction.z = 0;
                _targetObstacle.AddForce(swipeForceScale, direction, _targetObstacle.transform.position.z - _mainCam.transform.position.z);
                StartCoroutine(_targetObstacle.DestroyAfterSwipe(_swipeSO.activeTimeAfterSwipe));
                _targetObstacle = null;
            }
        }
    }
}

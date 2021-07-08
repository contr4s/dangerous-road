using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager: InputManager
{
    private Vector2 _prevTouchPos;
    private Vector2 _curTouchPos;

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                if (CanTurnCar(touch.position))
                    _prevTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _curTouchPos = touch.position;
                TryTurnCar(touch.position);
            }               

            if (_targetObstacle is null)
            {
                if (CheckIfCanSwipeObstacle(Input.mousePosition, out var obstacle))
                {
                    _prevTouchPos = touch.position;
                    SetupTargetObstacle(obstacle);
                }

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _curTouchPos = touch.position;
                SwipeOutObstacle();
            }
        }
    }

    protected override Vector3 CalculateSwipeDirection()
    {
        Vector3 direction;
        direction.x = _curTouchPos.x - _prevTouchPos.x;
        direction.y = _curTouchPos.y - _prevTouchPos.y;
        direction.z = 0;
        return direction.normalized;
    }
}

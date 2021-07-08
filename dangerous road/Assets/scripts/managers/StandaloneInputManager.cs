using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneInputManager: InputManager
{
    private Vector3 _prevTouchPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryTurnCar(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            if (_targetObstacle is null)
            {
                if (CheckIfCanSwipeObstacle(Input.mousePosition, out var obstacle))
                {
                    _prevTouchPos = Input.mousePosition;
                    SetupTargetObstacle(obstacle);
                }
            }          
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SwipeOutObstacle();
        }
    }

    protected override Vector3 CalculateSwipeDirection()
    {
        var direction = (Input.mousePosition - _prevTouchPos);
        direction.z = 0;
        direction.Normalize();
        return direction;
    }
}

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
            if (CanTurnCar(Input.mousePosition))
                _prevTouchPos = Input.mousePosition;           
        }
        else if (Input.GetMouseButton(0))
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
            TryTurnCar(Input.mousePosition);
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

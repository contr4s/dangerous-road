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
            if (IsMousePosInCarInputBounds(Input.mousePosition))
            {
                SwapLanes();
            }
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
        }
       
    }

    protected override Vector3 CalculateSwipeDirection()
    {
        var direction = (Input.mousePosition - _prevTouchPos);
        direction.z = 0;
        direction.Normalize();
        return direction;
    }

    private void SwapLanes()
    {
        if (Input.mousePosition.x > Screen.width / 2)
            spawnedObjectsManager.TrySwapLanes(2, maxWeight, _car.transform.position.z, _car.transform.position.z + farDist);
        else
            spawnedObjectsManager.TrySwapLanes(0, maxWeight, _car.transform.position.z, _car.transform.position.z + farDist);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneInputManager: InputManager
{
    private Vector3 _prevTouchPos;
    private Obstacle _targetObstacle;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (CheckIfCanSwipeObstacle(Input.mousePosition, out var obstacle, out float dist))
            {
                //StartCoroutine(Swipe(obstacle, _swipeSO.swipeTime, dist));
                _prevTouchPos = Input.mousePosition;
                _targetObstacle = obstacle;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_targetObstacle is null)
                return;

            var direction = (Input.mousePosition - _prevTouchPos);
            direction.z = 0;
            direction.Normalize();
            _targetObstacle.AddForce(swipeForceScale, direction, _targetObstacle.transform.position.z - _mainCam.transform.position.z);
            StartCoroutine(_targetObstacle.DestroyAfterSwipe(_swipeSO.activeTimeAfterSwipe));
            _targetObstacle = null;
        }
    }

    public IEnumerator Swipe(Obstacle obstacle, float swipeTime, float distToCam)
    {
        yield return new WaitForSeconds(swipeTime);
        var direction = (Input.mousePosition - _prevTouchPos);
        direction.z = 0;
        direction.Normalize();
        obstacle.AddForce(swipeForceScale, direction, distToCam);
        StartCoroutine(obstacle.DestroyAfterSwipe(_swipeSO.activeTimeAfterSwipe));
    }
}

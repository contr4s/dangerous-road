using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneInputManager: InputManager
{
    private Vector3 _prevTouchPos;

    void Update()
    {
        if (CheckIfCanSwipeObstacle(Input.mousePosition, out var obstacle, out float dist))
        {
            StartCoroutine(Swipe(obstacle, _swipeSO.swipeTime, dist));
            _prevTouchPos = Input.mousePosition;
        }

    }

    public IEnumerator Swipe(Obstacle obstacle, float swipeTime, float distToCam)
    {
        yield return new WaitForSeconds(swipeTime);
        var direction = (Input.mousePosition - _prevTouchPos);
        direction.z = 0;
        direction.Normalize();
        obstacle.AddForce(_swipeSO.swipeForceScale, direction, distToCam);
        StartCoroutine(obstacle.DestroyAfterSwipe());
    }
}

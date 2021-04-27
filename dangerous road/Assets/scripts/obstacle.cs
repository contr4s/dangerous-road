using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class obstacle: MonoBehaviour
{
    private bool _selected = false;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void OnMouseDown()
    {
        if (!_selected)
            _selected = true;
        else
            _selected = false;
    }

    private void Update()
    {
        if (_selected)
            FollowMouse();
    }

    private void FollowMouse()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z += _mainCam.transform.position.z;
        transform.position = _mainCam.ScreenToWorldPoint(mPos);
    }
}

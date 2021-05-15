using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/SwipeSO")]
public class SwipeSO : ScriptableObject
{
    public float swipeTime = 0.1f;
    public float swipeForceScale = 20;
    public float maxDistToSwipe = 150;
    public float minDistToSwipe = 15;
}

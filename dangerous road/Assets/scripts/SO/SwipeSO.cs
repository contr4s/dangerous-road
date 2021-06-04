using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Swipe")]
public class SwipeSO : ScriptableObject
{
    public float swipeForceScale = 20;
    public float maxDistToSwipe = 150;
    public float swipeTime = 0.1f;
    public float minDistToSwipe = 15;
}

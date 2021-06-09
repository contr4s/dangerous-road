using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Swipe")]
public class SwipeSO : ScriptableObject
{
    public float swipeTime = 0.1f;
    public float minDistToSwipe = 15;
    public float activeTimeAfterSwipe = 10f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Swipe")]
public class SwipeSO : ScriptableObject
{
    public float minDistToSwipe = 15;
    public float distWhereObstacleHasNormalColliderSize = 40;
    public float distWhereObstacleHasMaxColliderSize = 150;
    public float activeTimeAfterSwipe = 10f;
}

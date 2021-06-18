using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Road")]
public class RoadSO : ScriptableObject
{
    public int maxLane = 1;
    public float laneWidth = 4f;
}

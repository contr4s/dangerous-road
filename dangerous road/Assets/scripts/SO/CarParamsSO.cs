using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCarParameter
{
    maxSpeed,
    turnSpeed,
    swipeForce,
    maxSwipeDist
}

[Serializable]
public struct CarParametr
{
    public eCarParameter paramType;
    public float startVal;
    public float curVal;
    public byte curLvl;
    public byte maxLvl;
    public float lvlUpPercentage;

    public float CalculteMaxVal()
    {
        float result = startVal;
        for(int i = 1; i < maxLvl; i++)
        {
            result *= 1 + lvlUpPercentage / 100;
        }
        return result;
    }
}

[CreateAssetMenu(menuName = "Assets/CarParams")]
public class CarParamsSO : ScriptableObject
{
    public CarParametr[] parametrs;

    public bool TryFindParam(eCarParameter type, out CarParametr parametr)
    {
        foreach (var param in parametrs)
        {
            if (param.paramType == type)
            {
                parametr = param;
                return true;
            }           
        }
        parametr = new CarParametr();
        return false;
    }
}

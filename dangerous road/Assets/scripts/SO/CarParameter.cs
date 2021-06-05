using System;
using UnityEngine;

public enum eCarParameterType
{
    maxSpeed,
    turnSpeed,
    swipeForce,
    maxSwipeDist
}

[Serializable]
public class CarParameter
{
    public eCarParameterType type;

    public float startVal;   
    public byte curLvl;
    public byte maxLvl;
    [Range(0, 100)]
    public float lvlUpValuePercentage = 10;   

    public int startUpgradePrice;
    [Range(0, 100)]
    public float lvlUpPricePercentage = 20;

    [SerializeField] private float _curVal;

    public float CurVal
    {
        get {
            _curVal = CalculateVal(curLvl);
            return _curVal; 
        }
        set => _curVal = value;
    }

    public float NextVal { get => CalculateVal((byte)(curLvl + 1)); }
    public int CurPrice { get => (int)CalculateValChangingByLevel(startUpgradePrice, lvlUpPricePercentage, curLvl); }

    public float CalculateVal(byte lvl)
    {
        return CalculateValChangingByLevel(startVal, lvlUpValuePercentage, lvl);
    }

    public float CalculteMaxVal()
    {
        return CalculateVal(maxLvl);
    }

    public void LvlUp()
    {
        if (curLvl >= maxLvl)
            return;

        curLvl++;
        CurVal = CalculateVal(curLvl);
    }

    private float CalculateValChangingByLevel(float startVal, float percentage, byte lvl)
    {
        float result = startVal;
        for (int i = 1; i < lvl; i++)
        {
            result *= (1 + percentage / 100);
        }
        return result;
    }
}

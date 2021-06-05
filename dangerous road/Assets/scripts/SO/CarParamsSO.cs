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
public class CarParameter
{
    public eCarParameter paramType;

    public float startVal;   
    public byte curLvl;
    public byte maxLvl;
    public float lvlUpPercentage;

    public int upgradePrice;


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

    public float CalculateVal(byte lvl)
    {
        float result = startVal;
        for (int i = 1; i < lvl; i++)
        {
            result *= 1 + lvlUpPercentage / 100;
        }
        return result;
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
}

[CreateAssetMenu(menuName = "Assets/CarParams")]
public class CarParamsSO: ScriptableObject
{
    public CarParameter[] parametrs;

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach(var param in parametrs)
        {
            param.CurVal = param.CalculateVal(param.curLvl);
        }
    }
#endif

    public bool TryFindParam(eCarParameter type, out CarParameter parametr)
    {
        foreach (var param in parametrs)
        {
            if (param.paramType == type)
            {
                parametr = param;
                return true;
            }
        }
        parametr = new CarParameter();
        return false;
    }
}

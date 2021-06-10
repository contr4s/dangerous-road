using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/CarParams")]
public class CarParamsSO: ScriptableObject
{
    public new string name;
    public CarParameter[] parametrs;

    public int purchasePrice;
    public bool isPurchased;

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach(var param in parametrs)
        {
            param.CurVal = param.CalculateVal(param.curLvl);
        }
    }
#endif

    public bool TryFindParam(eCarParameterType type, out CarParameter parametr)
    {
        foreach (var param in parametrs)
        {
            if (param.type == type)
            {
                parametr = param;
                return true;
            }
        }
        parametr = new CarParameter();
        return false;
    }
}

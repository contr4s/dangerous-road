using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IDestroyable
{
    [SerializeField] private int _value;
    public int Value { get => _value; private set => _value = value; }
    

    [SerializeField] private GameObject _vfx;
    public GameObject Vfx { get => _vfx; private set => _vfx = value; }


    public void OnEnable()
    {
        CoinVfxManager.allCoins.Add(this);
    }

    public void OnDisable()
    {
        CoinVfxManager.allCoins.Remove(this);
    }

    public void DestroyMe()
    {
        gameObject.SetActive(false);
    }
}

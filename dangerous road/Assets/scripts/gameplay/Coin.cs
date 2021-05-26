using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IDestroyable
{
    [SerializeField] private int _value;

    public int Value { get => _value; private set => _value = value; }

    public void OnEnable()
    {
        CoinRotator.allCoins.Add(this);
    }

    public void OnDisable()
    {
        CoinRotator.allCoins.Remove(this);
    }

    public void DestroyMe()
    {
        gameObject.SetActive(false);
    }
}

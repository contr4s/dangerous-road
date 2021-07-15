using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCoinType
{
    bronze,
    silver,
    gold,
}

public class Coin: MonoBehaviour, IDestroyable
{
    [HideInInspector] public CoinVfxManager vfxManager;

    [SerializeField] private eCoinType _type;
    public eCoinType Type { get => _type; }

    [SerializeField] private int _value;
    public int Value { get => _value; }

    [SerializeField] private GameObject _vfx;
    public GameObject Vfx { get => _vfx; }

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
        if (vfxManager)
            vfxManager.CreateExplosion(transform.position, _type);
    }
}

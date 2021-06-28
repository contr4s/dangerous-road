using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinVfxManager: MonoBehaviour
{
    public static List<Coin> allCoins;

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _distToShowVfx = 75;

    private Camera _mainCam;

    private void Awake()
    {
        allCoins = new List<Coin>();
        _mainCam = Camera.main;
    }

    void Update()
    {
        for (int i = 0; i < allCoins.Count; i++)
        {
            allCoins[i].transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            allCoins[i].Vfx.SetActive(allCoins[i].transform.position.z - _mainCam.transform.position.z < _distToShowVfx);
        }
    }
}

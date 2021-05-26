using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    public static List<Coin> allCoins;

    [SerializeField] private float _rotationSpeed;

    private void Awake()
    {
        allCoins = new List<Coin>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < allCoins.Count; i++)
        {
            allCoins[i].transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }
    }
}

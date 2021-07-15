using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinVfxManager: MonoBehaviour
{
    public static List<Coin> allCoins;

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _distToShowVfx = 75;
    [SerializeField] private SerializableDictionary<eCoinType, CoinExplosionPool> _explosionPools;

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

    public void CreateExplosion(Vector3 position, eCoinType type)
    {
        var explosion = _explosionPools[type].GetAvailableObject();
        explosion.transform.position = position;
        explosion.gameObject.SetActive(true);
        StartCoroutine(ReturnExplosionToPool(explosion));
    }

    private IEnumerator ReturnExplosionToPool(ParticleSystem explosion)
    {
        yield return new WaitForSeconds(explosion.main.duration);
        explosion.gameObject.SetActive(false);
    }
}

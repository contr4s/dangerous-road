using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowCar))]
public class CameraShaking : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _magnitude;
    [SerializeField] private float _noize;

    private FollowCar _followCar;

    void Awake()
    {
        _followCar = GetComponent<FollowCar>();
    }

    void OnEnable()
    {
        Car.clashWithObstacle += ShakeCamera;
    }

    void OnDisable()
    {
        Car.clashWithObstacle -= ShakeCamera;
    }

    public void ShakeCamera()
    {
        _followCar.follow = false;
        StartCoroutine(ShakeCameraCor(_duration, _magnitude, _noize));
    }

    private IEnumerator ShakeCameraCor(float duration, float magnitude, float noize)
    {
        float elapsed = 0f;
        Vector3 startPosition = transform.localPosition;
        Vector2 noizeStartPoint0 = Random.insideUnitCircle * noize;
        Vector2 noizeStartPoint1 = Random.insideUnitCircle * noize;

        while (elapsed < duration)
        {
            Vector2 currentNoizePoint0 = Vector2.Lerp(noizeStartPoint0, Vector2.zero, elapsed / duration);
            Vector2 currentNoizePoint1 = Vector2.Lerp(noizeStartPoint1, Vector2.zero, elapsed / duration);
            Vector3 cameraPostionDelta = new Vector3(Mathf.PerlinNoise(currentNoizePoint0.x, currentNoizePoint0.y), Mathf.PerlinNoise(currentNoizePoint1.x, currentNoizePoint1.y));
            cameraPostionDelta *= magnitude;

            transform.localPosition = new Vector3(startPosition.x + cameraPostionDelta.x, startPosition.y + cameraPostionDelta.y, _followCar.CalculateZPos());
            elapsed += Time.deltaTime;
            yield return null;
        }
        _followCar.follow = true;
    }
}

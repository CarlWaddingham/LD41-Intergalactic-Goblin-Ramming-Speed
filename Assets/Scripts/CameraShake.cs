using System.Collections;
using System;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public float linearIntensity = 0.25f;
    public float angularIntensity = 5f;

    [NonSerialized] public bool isShaking = false;

    private bool angularShaking = true;

    void Update()
    {
        if (isShaking)
        {
            LinearShaking();
            if (angularShaking)
                AngularShaking();
        }
    }

    private void LinearShaking()
    {
        Vector2 shake = UnityEngine.Random.insideUnitCircle * linearIntensity;
        Vector3 newPosition = transform.localPosition;
        newPosition.x = shake.x;
        newPosition.y = shake.y;
        transform.localPosition = newPosition;
    }

    private void AngularShaking()
    {
        float shake = UnityEngine.Random.Range(-angularIntensity, angularIntensity);
        transform.localRotation = Quaternion.Euler(0f, 0f, shake);
    }

    public void SetAngularShaking(bool state)
    {
        angularShaking = state;
        if (!angularShaking)
            transform.localRotation = Quaternion.identity;
    }

    public void Shake(float time)
    {
        CancelInvoke();
        isShaking = true;
        Invoke("Stop", time);
    }

    public void Stop()
    {
        isShaking = false;
    }

    public void OnEnable()
    {
        instance = this;
        isShaking = false;
    }

    public void OnDisable()
    {
        isShaking = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
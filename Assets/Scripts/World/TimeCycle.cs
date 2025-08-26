using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCycle : MonoBehaviour
{
    public Light directionalLight;
    public float cycleDurationInHours = 24.0f;
    public float startHour = 0.0f;
    private float elapsedTime = 0f;
    private void Start()
    {
        UpdateLightRotation();
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= (cycleDurationInHours * 3600.0f))
        {
            elapsedTime = 0f;
        }

        UpdateLightRotation();
    }
    private void UpdateLightRotation()
    {
        float cycleFraction = elapsedTime / (cycleDurationInHours * 3600.0f);
        float targetRotationAngle = 360.0f * cycleFraction + (startHour / 24.0f) * 360.0f;

        targetRotationAngle = Mathf.Repeat(targetRotationAngle, 360.0f);

        directionalLight.transform.rotation = Quaternion.Euler(targetRotationAngle, 0f, 0f);
    }
}
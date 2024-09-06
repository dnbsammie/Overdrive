using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public enum CameraType
{
    Interior,
    Exterior
}
public enum CameraFX
{
    Static,
    Dynamic
}
[Serializable]
public class CameraSlot
{
    public Camera cameras;
    public CameraType cameraType;
    public CameraFX cameraFX;
    public int camFOV;
}
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CameraSlot[] cameraSlots;
    [Header("Settings")]
    public float camRotation;
    public float blurLevel;
    public float rotationSpeed;
    public TMP_Text textCamHeight;
    private float lastHeightChangeTime; 
    private Vector3 originalCameraPosition;
    [Header("Inputs")]
    public KeyCode changeCamera;
    public KeyCode lookRight;
    public KeyCode lookLeft;
    public KeyCode incHeight;
    public KeyCode decHeight;
    public void Start()
    {
        for (int i = 0; i < cameraSlots.Length; i++)
        {
            cameraSlots[i].cameras.gameObject.SetActive(i == 0);
        }
        originalCameraPosition = cameraSlots[0].cameras.transform.position;
    }
    public void Update()
    {
        if (Input.GetKeyDown(changeCamera))
        {
            int currentCameraIndex = GetCurrentCameraIndex();
            cameraSlots[currentCameraIndex].cameras.gameObject.SetActive(false);
            int nextCameraIndex = (currentCameraIndex + 1) % cameraSlots.Length;
            cameraSlots[nextCameraIndex].cameras.gameObject.SetActive(true);
        }
        RotateCamera();
        AdjustFOV();
        AdjustHeight();
    }
    int GetCurrentCameraIndex()
    {
        for (int i = 0; i < cameraSlots.Length; i++)
        {
            if (cameraSlots[i].cameras.gameObject.activeSelf)
            {
                return i;
            }
        }
        return 0;
    }
    void RotateCamera()
    {
        int currentCameraIndex = GetCurrentCameraIndex();
        Camera currentCamera = cameraSlots[currentCameraIndex].cameras;

        if (cameraSlots[currentCameraIndex].cameraType == CameraType.Interior)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);

            if (Input.GetKey(lookLeft))
            {
                targetRotation = Quaternion.Euler(0f, -camRotation, 0f);
            }
            else if (Input.GetKey(lookRight))
            {
                targetRotation = Quaternion.Euler(0f, camRotation, 0f);
            }
            currentCamera.transform.rotation = Quaternion.RotateTowards(currentCamera.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    void AdjustFOV()
    {

    }
    void AdjustHeight()
    {
        int currentCameraIndex = GetCurrentCameraIndex();
        Camera currentCamera = cameraSlots[currentCameraIndex].cameras;

        float heightChange = 0.01f;

        if (Input.GetKeyDown(incHeight) || Input.GetKeyDown(decHeight))
        {
            Vector3 newPosition;

            if (Input.GetKeyDown(incHeight))
            {
                newPosition = currentCamera.transform.position + new Vector3(0f, heightChange, 0f);
                newPosition.y = Mathf.Clamp(newPosition.y, originalCameraPosition.y, originalCameraPosition.y + 0.05f);
            }
            else
            {
                newPosition = currentCamera.transform.position - new Vector3(0f, heightChange, 0f);
                newPosition.y = Mathf.Clamp(newPosition.y, originalCameraPosition.y - 0.05f, originalCameraPosition.y);
            }

            currentCamera.transform.position = newPosition;

            UpdateCamHeightText(newPosition.y);
            lastHeightChangeTime = Time.time;
        }
        if (Time.time - lastHeightChangeTime > 2f)
        {
            textCamHeight.text = string.Empty;
        }
    }
    void UpdateCamHeightText(float currentHeight)
    {
        textCamHeight.text = "Cam Height: " + (currentHeight - originalCameraPosition.y).ToString("+#0.00;-#0.00;0.00");
    }
}
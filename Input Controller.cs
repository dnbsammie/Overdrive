using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class InputController : MonoBehaviour
{
    [Header("UI")]
    public Slider throttleSlider;
    public Slider brakeSlider;
    public Slider clutchSlider;
    public Slider handbrakeSlider;
    public Image steeringImage;
    [Header("Rates")]
    public Slider sRate;
    public Slider sSensitivity;
    public Slider tRate;
    public Slider tSensitivity;
    public Slider bRate;
    public Slider bSensitivity;
    public Slider cRate;
    public Slider cSensitivity;
    public Slider hbRate;
    public Slider hbSensitivity;
    // values
    public float steeringRate = 100f;
    public float steeringSensitivity = 100f;
    public float throttleRate = 100f;
    public float throttleSensitivity = 100f;
    public float brakeRate = 100f;
    public float brakeSensitivity = 100f;
    public float clutchRate = 100f;
    public float clutchSensitivity = 100f;
    public float handbrakeRate = 100f;
    public float handbrakeSensitivity = 100f;
    [Header("Actions")]
    public TMP_Text steeringText;
    public TMP_Text throttleText;
    public TMP_Text brakeText;
    public TMP_Text clutchText;
    public TMP_Text handbrakeText;
    public TMP_Text gearUpText;
    public TMP_Text gearDownText;
    public TMP_Text lightsText;
    public TMP_Text changeCameraText;
    public TMP_Text cameraLeftText;
    public TMP_Text cameraRightText;
    public TMP_Text cameraBehindText;
    public TMP_Text pauseText;
    public TMP_Text pitlaneText;
    public TMP_Text lcdModeText;
    public TMP_Text lcdUpText;
    public TMP_Text lcdDownText;
    public TMP_Text lcdIncreaseText;
    public TMP_Text lcdDecreaseText;
    [Header("Buttons")]
    public string steeringAxis;
    public KeyCode throttleKey;
    public KeyCode brakeKey;
    public KeyCode clutchKey;
    public KeyCode handbrakeKey;
    public KeyCode gearUpKey;
    public KeyCode gearDownKey;
    public KeyCode lightsKey;
    public KeyCode changeCameraKey;
    public KeyCode cameraLeftKey;
    public KeyCode cameraRightKey;
    public KeyCode cameraBehindKey;
    public KeyCode pauseKey;
    public KeyCode pitlaneKey;
    private KeyCode lcdModeKey;
    private KeyCode lcdUpKey;
    private KeyCode lcdDownKey;
    private KeyCode lcdIncreaseKey;
    private KeyCode lcdDecreaseKey;
    // assignedKeys
    private HashSet<KeyCode> assignedKeys = new HashSet<KeyCode>();
    void Start()
    {
        //Input
        steeringAxis = "Horizontal";
        throttleKey = KeyCode.W;
        brakeKey = KeyCode.S;
        clutchKey = KeyCode.LeftControl;
        handbrakeKey = KeyCode.Space;
        gearUpKey = KeyCode.LeftShift;
        gearDownKey = KeyCode.LeftAlt;
        lightsKey = KeyCode.L;
        changeCameraKey = KeyCode.C;
        cameraLeftKey = KeyCode.Q;
        cameraRightKey = KeyCode.E;
        cameraBehindKey = KeyCode.B;
        pauseKey = KeyCode.P;
        pitlaneKey = KeyCode.P;
        lcdModeKey = KeyCode.M;
        lcdUpKey = KeyCode.UpArrow;
        lcdDownKey = KeyCode.DownArrow;
        lcdIncreaseKey = KeyCode.RightArrow;
        lcdDecreaseKey = KeyCode.LeftArrow;
        //Rates & Sensivity
        sRate.value = 50f;
        sSensitivity.value = 100f;
        tRate.value = 100f;
        tSensitivity.value = 50f;
        bRate.value = 75f;
        bSensitivity.value = 50f;
        cRate.value = 25f;
        cSensitivity.value = 50f;
        hbRate.value = 50f;
        hbSensitivity.value = 50f;

        UpdateActionText();
    }
    void Update()
    {
        throttleSlider.value = Input.GetKey(throttleKey) ? 100f : 0f;
        brakeSlider.value = Input.GetKey(brakeKey) ? 100f : 0f;
        clutchSlider.value = Input.GetKey(clutchKey) ? 100f : 0f;
        handbrakeSlider.value = Input.GetKey(handbrakeKey) ? 100f : 0f;
        //Throttle
        float throttleRate = tRate.value;
        float throttleSensitivity = tSensitivity.value;
        float throttleInput = Input.GetKey(throttleKey) ? 1f : 0f;
        float maxThrottle = 100f * throttleRate;
        float scaledThrottle = throttleInput * maxThrottle * throttleSensitivity;
        // Steering
        float steeringSensitivity = sSensitivity.value;
        float steeringRate = sRate.value;
        float steeringInput = Input.GetAxis(steeringAxis);
        float maxRotation = 120f * steeringRate;
        float scaledRotation = steeringInput * -maxRotation * steeringSensitivity;
        steeringImage.rectTransform.localEulerAngles = new Vector3(0, 0, scaledRotation);
    }
    public void ChangeKey(string action)
    {
        StartCoroutine(WaitForKey(action));
    }
    IEnumerator WaitForKey(string action)
    {
        yield return null;

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        if (Input.GetButton("Horizontal"))
        {
            float value = Input.GetAxis("Horizontal");
            if (Mathf.Abs(value) > 0.1f)
            {
                steeringAxis = "Horizontal";
                UpdateActionText();
            }
        }
        else
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.Escape)
                {
                    if (!assignedKeys.Contains(keyCode))
                    {
                        assignedKeys.Add(keyCode);

                        switch (action)
                        {
                            case "throttle":
                                throttleKey = keyCode;
                                break;
                            case "brake":
                                brakeKey = keyCode;
                                break;
                            case "clutch":
                                clutchKey = keyCode;
                                break;
                            case "handbrake":
                                handbrakeKey = keyCode;
                                break;
                            case "gear up":
                                gearUpKey = keyCode;
                                break;
                            case "gear down":
                                gearDownKey = keyCode;
                                break;
                            case "lights":
                                lightsKey = keyCode;
                                break;
                            case "camera":
                                changeCameraKey = keyCode;
                                break;
                            case "camera left":
                                cameraLeftKey = keyCode;
                                break;
                            case "camera right":
                                cameraRightKey = keyCode;
                                break;
                            case "camera behind":
                                cameraBehindKey = keyCode;
                                break;
                            case "pause":
                                pauseKey = keyCode;
                                break;
                            case "pitlane":
                                pitlaneKey = keyCode;
                                break;
                            case "lcd mode":
                                lcdModeKey = keyCode;
                                break;
                            case "lcd up":
                                lcdUpKey = keyCode;
                                break;
                            case "lcd down":
                                lcdDownKey = keyCode;
                                break;
                            case "lcd increase":
                                lcdIncreaseKey = keyCode;
                                break;
                            case "lcd decrease":
                                lcdDecreaseKey = keyCode;
                                break;
                        }
                    }

                    UpdateActionText();
                    yield break;
                }
            }
        }
    }
    void UpdateActionText()
    {
        steeringText.text = steeringAxis == "Horizontal" ? "Default" : steeringAxis;
        throttleText.text = throttleKey.ToString();
        brakeText.text = brakeKey.ToString();
        clutchText.text = clutchKey.ToString();
        handbrakeText.text = handbrakeKey.ToString();
        gearUpText.text = gearUpKey.ToString();
        gearDownText.text = gearDownKey.ToString();
        lightsText.text = lightsKey.ToString();
        changeCameraText.text = changeCameraKey.ToString();
        cameraLeftText.text = cameraLeftKey.ToString();
        cameraRightText.text = cameraRightKey.ToString();
        cameraBehindText.text =cameraBehindKey.ToString();
        pauseText.text = pauseKey.ToString();
        pitlaneText.text = pitlaneKey.ToString();
        lcdModeText.text = lcdModeKey.ToString();
        lcdUpText.text = lcdUpKey.ToString();
        lcdDownText.text = lcdDownKey.ToString();
        lcdIncreaseText.text = lcdIncreaseKey.ToString();
        lcdDecreaseText.text = lcdDecreaseKey.ToString();
    }
    public void AdjustRateSensitivity(string action)
    {
        switch (action)
        {
            case "steering":
                steeringRate = sRate.value;
                steeringSensitivity = sSensitivity.value;
                break;
            case "throttle":
                throttleRate = tRate.value;
                throttleSensitivity = tSensitivity.value;
                break;
            case "brake":
                brakeRate = bRate.value;
                brakeSensitivity = bSensitivity.value;
                break;
            case "clutch":
                clutchRate = cRate.value;
                clutchSensitivity = cSensitivity.value;
                break;
            case "handbrake":
                handbrakeRate = hbRate.value;
                handbrakeSensitivity = hbSensitivity.value;
                break;
        }
    }
}
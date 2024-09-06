using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.XInput;
public enum Axel
{
    Front,
    Rear
}
public enum Drive
{
    AWD,
    FWD,
    RWD,
}
public enum BrakeType
{
    Regular,
    ABS
}
public enum SteeringType
{
    AckermannPositive,
    AckermannNegative,
    Parallel
}
public enum GearboxType
{
    Automatic,
    Manual,
    ManualClutch
}
public enum GearState
{
    Neutral,
    Running,
    CheckingChange,
    Changing
}
public enum ChargingType
{
    Turbo,
    TwinTurbo,
    SuperCharger
}
public enum EnginePosition
{
    Front,
    Rear
}
public enum SpeedUnit
{
    KPH,
    MPH
}
[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}
public class CarController : MonoBehaviour
{
    [Header("Elements")]
    public new Rigidbody rigidbody;
    public Vector3 CM;
    public Drive drive;
    public Wheel[] wheels;
    public EngineAudio engineAudio;
    public GameObject carLights;
    public KeyCode throttleKey = KeyCode.UpArrow;
    public KeyCode brakeKey = KeyCode.DownArrow;
    public KeyCode clutchKey = KeyCode.RightControl;
    public KeyCode handbrakeKey = KeyCode.Space;
    public KeyCode gearUpKey = KeyCode.E;
    public KeyCode gearDownKey = KeyCode.Q;
    public KeyCode lightsKey = KeyCode.L;
    public Slider throttleSlider;
    public Slider brakeSlider;
    public Slider clutchSlider;
    public Slider handbrakeSlider;
    public Image steeringImage;
    public float maxSteeringWheelRotation = 45f;
    public float steeringWheelReturnSpeed = 5f;
    [Header("UI Elements")]
    public TMP_Text gearText;
    public TMP_Text speedText;
    public TMP_Text rpmText;
    public TMP_Text uphText;
    public Slider rpmSlider;
    [Header("Set Up")]
    public GearboxType gearboxType = GearboxType.Manual;
    public BrakeType brakeMode = BrakeType.Regular;
    public SpeedUnit speedDisplayUnit = SpeedUnit.KPH;
    public bool tcOverride = false;
    public bool hShifter = false;
    public bool handbrakeActive = false;
    [Header("Tacometer Data")]
    public float currentRPM = 0f;
    public float currentSpeed = 0f;
    public float currentTorque = 0f;
    public float currentFuel = 0f;
    [Header("Car Stats")]
    public float currentAceleration = 0f;
    public float currentMaxSpeed = 0f;
    public float currentGForces = 0f;
    [Header("Inputs")]
    public float steering;
    public float throttle;
    public float brake;
    public float clutch;
    public float handbrake;
    public float gearUp;
    public float gearDown;
    public float headLights;
    [Header("Motor")]
    public EnginePosition enginePosition = EnginePosition.Front;
    public float frontEngineCM = -0.5f;
    public float rearEngineCM = 0.5f;
    public float engineMass;
    public float idleRPM;
    public float maxRPM;
    public float maxSpeed;
    public float maxReverseSpeed;
    public float torque;
    public float power; //HP
    public float redLine;
    [Header("Transmission")]
    public int maxGears = 6;
    public float reverseRatio;
    public float[] gearRatios;
    private int currentGear = 0;
    private bool isShifting = false;
    [Header("Braking")]
    public float brakePressure;
    public float handbrakeTorque;
    public float frontBrakeForce;
    public float rearBrakeForce;
    public float absSlipThreshold = 0.2f;
    public float absBrakeMultiplier = 0.7f;
    [Header("Direction")]
    public SteeringType steeringType = SteeringType.Parallel;
    public float maxSteerAngle;
    public float minTurnRadius = 30;
    public float maxTurnRadius = 45;
    public float wheelbase = 1;
    [Header("Suspension")]
    public float frontSuspensionHeight;
    public float rearSuspensionHeight;
    public float suspensionHardness;
    public float CurrentSpeed => currentSpeed;
    void Start()
    {
        rigidbody.centerOfMass = CM;
        UpdateCenterOfMass();
        InitializeSuspension();
    }
    void InitializeSuspension()
    {
        foreach (var wheel in wheels)
        {
            WheelCollider collider = wheel.collider;

            if (wheel.axel == Axel.Front)
            {
                collider.suspensionDistance = frontSuspensionHeight;
            }
            else if (wheel.axel == Axel.Rear)
            {
                collider.suspensionDistance = rearSuspensionHeight;
            }

            JointSpring suspensionSpring = collider.suspensionSpring;
            suspensionSpring.spring = suspensionHardness;
            collider.suspensionSpring = suspensionSpring;
        }
    }
    void UpdateCenterOfMass()
    {
        Vector3 centerOfMass = rigidbody.centerOfMass;
        if (enginePosition == EnginePosition.Front)
        {
            centerOfMass.z = frontEngineCM;
        }
        else
        {
            centerOfMass.z = rearEngineCM;
        }
        rigidbody.centerOfMass = centerOfMass;
        rigidbody.mass += engineMass;
    }
    void Update()
    {
        if (throttle == 0 && brake == 0)
        {
            currentRPM = Mathf.Lerp(currentRPM, idleRPM, Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        AnimateWheels();
        ApplyBrake();
        Engine();
        SetInput();
        Steering();
        Tacometer();
        Transmission();
        TurnLights();
    }
    public void SetInput()
    {
        throttleSlider.value = Input.GetKey(throttleKey) ? 100f : 0f;
        brakeSlider.value = Input.GetKey(brakeKey) ? 100f : 0f;
        clutchSlider.value = Input.GetKey(clutchKey) ? 100f : 0f;
        handbrakeSlider.value = Input.GetKey(handbrakeKey) ? 100f : 0f;

        steering = Input.GetAxis("Horizontal");
        throttle = Input.GetKey(throttleKey) ? 1f : 0f;
        brake = Input.GetKey(brakeKey) ? 1f : 0f;
        clutch = Input.GetKey(clutchKey) ? 1f : 0f;
        handbrake = Input.GetKey(handbrakeKey) ? 1f : 0f;
        headLights = Input.GetKey(lightsKey) ? 1f : 0f;
    }
    public void TurnLights()
    {
        if (Input.GetKeyDown(lightsKey))
        {
            carLights.SetActive(!carLights.activeSelf);
        }
    }
    public void Steering()
    {
        if (steeringType == SteeringType.Parallel)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    float steerAngle = steering * maxSteerAngle;
                    wheel.collider.steerAngle = steerAngle;
                }
            }
        }
        else if (steeringType == SteeringType.AckermannPositive)
        {
            //Ackermann logic
        }
        else if (steeringType == SteeringType.AckermannNegative)
        {
            //Anti-Ackermann
        }
    }
    void ApplyBrake()
    {
        float brakeForce = brake * brakePressure;
        if (brakeMode == BrakeType.Regular)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    wheel.collider.brakeTorque = brakeForce * frontBrakeForce;
                }
                else if (wheel.axel == Axel.Rear)
                {
                    wheel.collider.brakeTorque = brakeForce * rearBrakeForce;
                }
            }
        }
        else if (brakeMode == BrakeType.ABS)
        {
            foreach (var wheel in wheels)
            {
                float wheelAngularSpeed = wheel.collider.rpm * Mathf.Deg2Rad;
                float slip = Mathf.Abs(wheelAngularSpeed - currentSpeed) / Mathf.Max(Mathf.Abs(currentSpeed), 1.0f);

                if (slip > absSlipThreshold)
                {
                    float absBrakeForce = brakeForce * absBrakeMultiplier;
                    wheel.collider.brakeTorque = absBrakeForce;
                }
                else
                {
                    wheel.collider.brakeTorque = brakeForce;
                }
            }
        }
        if (handbrakeActive)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Rear)
                    wheel.collider.brakeTorque += handbrake * handbrakeTorque;
            }
        }
    }
    void Engine()
    {
        foreach (var wheel in wheels)
        {
            float torque = CalculateTorque(currentGear, currentRPM);

            if (drive == Drive.AWD)
                wheel.collider.motorTorque = torque * throttle;
            else if (drive == Drive.FWD && wheel.axel == Axel.Front)
                wheel.collider.motorTorque = torque * throttle;
            else if (drive == Drive.RWD && wheel.axel == Axel.Rear)
                wheel.collider.motorTorque = torque * throttle;
        }
        if (currentGear == 0)
        {
            currentRPM += 50f * Time.deltaTime;
            currentRPM = Mathf.Clamp(currentRPM, idleRPM, maxRPM);
        }
        else
        {
            float targetRPM = maxRPM * throttle;
            float clampedRPM = Mathf.Clamp(targetRPM, idleRPM, maxRPM);
            currentRPM = Mathf.Lerp(currentRPM, clampedRPM, Time.deltaTime * 5f);
        }
        float maxSpeedLimit = (currentGear == -1) ? GetMaxReverseSpeed() : maxSpeed;
        currentSpeed = Mathf.Clamp(rigidbody.velocity.magnitude * 3.6f, 0f, maxSpeedLimit);
    }
    float CalculateTorque(int gear, float currentRPM)
    {
        if (gear == 0)
        {
            return 0f;
        }
        else if (gear == -1)
        {
            return -torque * reverseRatio;
        }
        else
        {
            float ratio = gearRatios[gear - 1];
            float normalizedRPM = Mathf.Clamp01((currentRPM - idleRPM) / (maxRPM - idleRPM));
            float gearTorque = Mathf.Lerp(1f, ratio, normalizedRPM);
            float speedFactor = 1f - Mathf.Clamp01(currentSpeed / GetMaxSpeedForGear(gear));
            gearTorque *= speedFactor;
            return torque * throttle * gearTorque;
        }
    }
    float GetMaxSpeedForGear(int gear)
    {
        switch (gear)
        {
            case 1:
                return maxSpeed * 0.225f;
            case 2:
                return maxSpeed * 0.415f;
            case 3:
                return maxSpeed * 0.625f;
            case 4:
                return maxSpeed * 0.815f;
            case 5:
                return maxSpeed * 1.025f;
            case 6:
                return maxSpeed * 1.215f;
            default:
                return maxSpeed;
        }
    }
    float GetMaxReverseSpeed()
    {
        return maxSpeed * 0.27f;
    }
    float CalculateRPM()
    {
        float targetRPM = maxRPM * throttle;

        if (currentGear == 0)
        {
            return Mathf.Lerp(currentRPM, idleRPM, Time.deltaTime * 5f);
        }
        else if (currentGear == -1)
        {
            float maxReverseRPM = maxRPM;
            return Mathf.Lerp(currentRPM, maxReverseRPM * throttle, Time.deltaTime * 5f);
        }
        else
        {
            float gearRatio = gearRatios[currentGear - 1];
            float normalizedSpeed = Mathf.Clamp01(currentSpeed / GetMaxSpeedForGear(currentGear));
            float maxEngineRPM = gearRatio * maxRPM;
            float clampedRPM = Mathf.Lerp(currentRPM, maxEngineRPM * throttle, Time.deltaTime * 5f);
            float finalRPM = Mathf.Clamp(clampedRPM, idleRPM, maxRPM);
            if (currentGear == -1)
            {
                float speedFactor = Mathf.Clamp01(currentSpeed / GetMaxSpeedForGear(currentGear));
                finalRPM = Mathf.Lerp(idleRPM, finalRPM, speedFactor);
            }
            return finalRPM;
        }
    }
    void Transmission()
    {
        if (gearboxType == GearboxType.Manual)
        {
            if (!isShifting)
            {
                ShiftGear();
            }
        }
        else if (gearboxType == GearboxType.Automatic)
        {
            AutoShiftGear();
        }
    }
    void ShiftGear()
    {
        int direction = 0;

        if (Input.GetKeyDown(gearUpKey))
        {
            direction = 1;
        }
        else if (Input.GetKeyDown(gearDownKey))
        {
            direction = -1;
        }

        if (direction != 0)
        {
            StartCoroutine(ShiftCoroutine(direction));
        }
    }
    IEnumerator ShiftCoroutine(int direction)
    {
        isShifting = true;

        currentGear = Mathf.Clamp(currentGear + direction, -1, maxGears);

        if (currentGear == 0)
        {
            clutch = 0;
        }

        yield return new WaitForSeconds(0.1f);
        isShifting = false;
    }
    void AutoShiftGear()
    {
        int direction = 0;
        if (currentRPM > redLine * 0.9 && currentGear < maxGears)
        {
            direction |= 1;
        }
        else if (maxRPM < redLine * 0.2 && currentGear > 1)
        {
            direction = -1;
        }
    }
    void Tacometer()
    {
        float engineSpeed = currentSpeed * gearRatios[Mathf.Clamp(currentGear - 1, 0, gearRatios.Length - 1)];
        float normalizedEngineSpeed = engineSpeed / maxSpeed;
        // UI
        currentRPM = Mathf.Lerp(currentRPM, normalizedEngineSpeed * maxRPM, Time.deltaTime);
        // Speed
        float speedFactor = Mathf.Clamp01(currentSpeed / maxSpeed);
        float speedInGear = maxSpeed * speedFactor;
        currentSpeed = speedInGear;
        speedText.text = Mathf.Round(currentSpeed).ToString() + " ";
        uphText.text = (speedDisplayUnit == SpeedUnit.KPH ? "KPH" : "MPH");
        // Gears
        if (currentGear == 0)
        {
            gearText.text = "N";
        }
        else if (currentGear == -1)
        {
            gearText.text = "R";
        }
        else
        {
            gearText.text = currentGear.ToString();
        }
        // RPMS
        currentRPM = normalizedEngineSpeed * maxRPM;
        rpmText.text = Mathf.Round(currentRPM).ToString();
        rpmSlider.maxValue = maxRPM;
        rpmSlider.value = currentRPM;
        // Color
        Color redColor = (currentRPM > redLine) ? Color.red : Color.white;
        gearText.color = redColor;
        rpmText.color = redColor;
        rpmSlider.fillRect.GetComponent<Image>().color = redColor;
        speedText.color = redColor;
    }
    public void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot = new Quaternion();
            Vector3 pos = new Vector3();
            wheel.collider.GetWorldPose(out pos, out rot);
            wheel.model.transform.position = pos;
            wheel.model.transform.rotation = rot;
        }
    }
}
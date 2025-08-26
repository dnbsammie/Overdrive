using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCarInput : MonoBehaviour
{
    [Header("Inputs")]
    public PlayerActions input;
    public float steeringInput;
    public float throttleInput;
    public float brakeInput;
    void Awake()
    {
        input = new PlayerActions();
    }
    void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
        input.Car.Steering.performed += ApplySteering;
        input.Car.Steering.canceled += ReleaseSteering;
        input.Car.Throttle.performed += ApplyThrottle;
        input.Car.Throttle.canceled += ReleaseThrottle;
    }
    private void ApplySteering(InputAction.CallbackContext value)
    {
        steeringInput = value.ReadValue<float>();
    }
    private void ReleaseSteering(InputAction.CallbackContext value)
    {
        steeringInput = 0;
    }
    private void ApplyThrottle(InputAction.CallbackContext value)
    {
        throttleInput = value.ReadValue<float>();
    }
    private void ReleaseThrottle(InputAction.CallbackContext value)
    {
        throttleInput = 0;
    }
    private void ApplyBrake(InputAction.CallbackContext value)
    {
        brakeInput = value.ReadValue<float>();
    }
    private void ReleaseBrake(InputAction.CallbackContext value)
    {
        brakeInput = 0;
    }
}
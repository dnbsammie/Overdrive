using UnityEngine;

namespace Overdrive.Vehicles
{
    public class BrakeSystem : MonoBehaviour
    {
        [Header("Brake Configuration")]
        public BrakeType brakeMode = BrakeType.Regular;
        public float brakePressure;
        public float handbrakeTorque;
        public float frontBrakeForce;
        public float rearBrakeForce;
        public float absSlipThreshold = 0.2f;
        public float absBrakeMultiplier = 0.7f;

        public void ApplyBrake(Wheel[] wheels, float brake, float handbrake, float currentSpeed, bool handbrakeActive)
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
    }
}

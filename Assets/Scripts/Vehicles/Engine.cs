using UnityEngine;

namespace Overdrive.Vehicles
{
    public class Engine : MonoBehaviour
    {
        [Header("Engine Configuration")]
        public EnginePosition enginePosition = EnginePosition.Front;
        public float engineMass;
        public float idleRPM;
        public float maxRPM;
        public float maxSpeed;
        public float maxReverseSpeed;
        public float torque;
        public float power; //HP
        public float redLine;
        public float currentRPM;

        public float CalculateTorque(int gear, float currentRPM, float currentSpeed, float throttle, float[] gearRatios, float reverseRatio)
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

        public float GetMaxSpeedForGear(int gear)
        {
            switch (gear)
            {
                case 1: return maxSpeed * 0.225f;
                case 2: return maxSpeed * 0.415f;
                case 3: return maxSpeed * 0.625f;
                case 4: return maxSpeed * 0.815f;
                case 5: return maxSpeed * 1.025f;
                case 6: return maxSpeed * 1.215f;
                default: return maxSpeed;
            }
        }

        public float GetMaxReverseSpeed()
        {
            return maxSpeed * 0.27f;
        }

        public float CalculateRPM(float currentRPM, int currentGear, float throttle, float currentSpeed, float[] gearRatios)
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
    }
}

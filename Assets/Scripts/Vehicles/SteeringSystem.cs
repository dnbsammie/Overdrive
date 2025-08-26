using UnityEngine;

namespace Overdrive.Vehicles
{
    public class SteeringSystem : MonoBehaviour
    {
        [Header("Steering Configuration")]
        public SteeringType steeringType = SteeringType.Parallel;
        public float maxSteerAngle;
        public float minTurnRadius = 30;
        public float maxTurnRadius = 45;
        public float wheelbase = 1;

        public void ApplySteering(Wheel[] wheels, float steering)
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
                // TODO: Implementar lógica Ackermann positiva
            }
            else if (steeringType == SteeringType.AckermannNegative)
            {
                // TODO: Implementar lógica Ackermann negativa
            }
        }
    }
}

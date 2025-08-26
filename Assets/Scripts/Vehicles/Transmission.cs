using UnityEngine;
using System.Collections;

namespace Overdrive.Vehicles
{
    public class Transmission : MonoBehaviour
    {
        [Header("Transmission Configuration")]
        public GearboxType gearboxType = GearboxType.Manual;
        public int maxGears = 6;
        public float reverseRatio;
        public float[] gearRatios;

        private int currentGear = 0;
        private bool isShifting = false;

        public int CurrentGear => currentGear;
        public bool IsShifting => isShifting;

        public void HandleTransmission(float currentRPM, float redLine, float maxRPM)
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
                AutoShiftGear(currentRPM, redLine, maxRPM);
            }
        }

        private void ShiftGear()
        {
            int direction = 0;

            if (Input.GetKeyDown(KeyCode.E)) // gearUpKey
            {
                direction = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // gearDownKey
            {
                direction = -1;
            }

            if (direction != 0)
            {
                StartCoroutine(ShiftCoroutine(direction));
            }
        }

        private IEnumerator ShiftCoroutine(int direction)
        {
            isShifting = true;
            currentGear = Mathf.Clamp(currentGear + direction, -1, maxGears);
            yield return new WaitForSeconds(0.1f);
            isShifting = false;
        }

        private void AutoShiftGear(float currentRPM, float redLine, float maxRPM)
        {
            if (currentRPM > redLine * 0.9f && currentGear < maxGears)
            {
                StartCoroutine(ShiftCoroutine(1));
            }
            else if (maxRPM < redLine * 0.2f && currentGear > 1)
            {
                StartCoroutine(ShiftCoroutine(-1));
            }
        }
    }
}

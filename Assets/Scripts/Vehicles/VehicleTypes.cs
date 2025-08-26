using UnityEngine;
using System;

namespace Overdrive.Vehicles
{
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
}

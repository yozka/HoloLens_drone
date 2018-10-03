using System;
using Urho;

namespace HolographicsDrone.Drone.Hardware
{
    ///-------------------------------------------------------------------
    using Utils;
    ///-------------------------------------------------------------------




    ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Описание моторов
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public enum EMotor
    {
        none,
        frontLeft,
        frontRight,
        rearLeft,
        rearRight
    }
    ///--------------------------------------------------------------------



    ///-------------------------------------------------------------------






    ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Необходимо применить к классу BasicControl. 
    /// Мотор только вычисляет свою силу индивидуально.
    /// Применение силы должно выполняться классом Rigidbody.
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AMotor
    {

        public double upForce        = 0.0f; // Общее усилие, применяемое этим двигателем. Это может быть передано родительскому RigidBody
        public double sideForce      = 0.0f; // Крутящий момент или боковое усилие, прикладываемые этим двигателем. Это может быть передано родительскому RigidBody и рассчитано с другими двигателями
        public double power          = 0.0f;    // Множитель мощности. Легкий способ создания более мощных двигателей
        public double exceedForce    = 0.0f; // Значение отрицательной силы, когда Upforce становится ниже 0

        public double yawFactor      = 0.0f; // Фактор, применяемый к боковой силе. Более высокие значения ускоряют движение Yaw
        public bool  invertDirection= false; // Независимо от направления вращения счетчика или против часовой стрелки
        public double pitchFactor    = 0.0f; // A factor to be applied to the pitch correction
        public double rollFactor     = 0.0f; // A factor to be applied to the roll correction

        public double mass = 0.0f;

        public float  speedPropeller = 0.0f;


        public EMotor typeMotor     = EMotor.none;//тип мотора
        ///-------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Вычесление силы мотора
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void updateForceValues(AComputerModule computer, float timeFrame)
        {
            double upForceLast = upForce;

            double upForceThrottle = MathHelperExt.Clamp(computer.throttle, 0, 1) * power;
            double upForceTotal = upForceThrottle;

            upForceTotal -= computer.pitchCorrection * pitchFactor;
            upForceTotal -= computer.rollCorrection * rollFactor;

            upForce = upForceTotal;

            if (computer.isStopMotor())
            {
                upForce = 0;
            }

           // upForce = MathHelper.Lerp(upForceLast, upForceTotal, 0.5f);

            sideForce = preNormalize(computer.yawCorrection, yawFactor);

            float speed = (float)upForce * 2500.0f * (invertDirection ? 1.0f : -1.0f);
            speedPropeller = MathHelper.Lerp(speedPropeller, speed, timeFrame);

        }
        ///-------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Способ применения коэффициента и зажима крутящего момента до его предела
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private double preNormalize(double input, double factor)
        {
            double finalValue = input;

            if (invertDirection)
                finalValue = MathHelperExt.Clamp(finalValue, -1, 0);
            else
                finalValue = MathHelperExt.Clamp(finalValue, 0, 1);

            return finalValue * factor;
        }
    }
}

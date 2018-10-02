using System;
using Urho;

namespace HolographicsDrone.Drone.Hardware
{
    ///-------------------------------------------------------------------

    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Система создания мотора
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ACreatorMotor
    {
        ///--------------------------------------------------------------------



         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Создание общего мотора
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AMotor createMotor()
        {
            var motor = new AMotor();

            //motor.power         = 0.040f;
            motor.power = 0.0500f;
            motor.yawFactor     = 0.01f;

            return motor;
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Создание мотора
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AMotor frontLeft()
        {
            var motor = createMotor();
            motor.typeMotor = EMotor.frontLeft;

 
            motor.pitchFactor   =  0.005f;
            motor.rollFactor    = -0.005f;
            motor.invertDirection = false;

            return motor;
        }
        ///--------------------------------------------------------------------



         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Создание мотора
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AMotor frontRight()
        {
            var motor = createMotor();
            motor.typeMotor = EMotor.frontRight;

            motor.pitchFactor       =  0.005f;
            motor.rollFactor        =  0.005f;
            motor.invertDirection   = true;

            return motor;
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Создание мотора
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AMotor rearLeft()
        {
            var motor = createMotor();
            motor.typeMotor = EMotor.rearLeft;

            motor.pitchFactor   = -0.005f;
            motor.rollFactor    = -0.005f;
            motor.invertDirection = true;

            return motor;
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Создание мотора
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AMotor rearRight()
        {
            var motor = createMotor();
            motor.typeMotor = EMotor.rearRight;

            motor.pitchFactor       = -0.005f;
            motor.rollFactor        =  0.005f;
            motor.invertDirection = false;

            return motor;
        }
        ///--------------------------------------------------------------------

    }
}

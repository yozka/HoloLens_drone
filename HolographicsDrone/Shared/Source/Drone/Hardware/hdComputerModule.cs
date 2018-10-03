using System;
using System.Collections.Generic;
using System.Text;
using Urho;



namespace HolographicsDrone.Drone.Hardware
{
    ///-------------------------------------------------------------------

    ///-------------------------------------------------------------------





    ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Полетный контроллер
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AComputerModule
    {
        ///--------------------------------------------------------------------
        private const float pFactor = 0.5f;     //0.5f
        private const float iFactor = 5.0f;     //5.0f
        private const float dFactor = 0.05f;    //0.05f

        private const float         pitchLimit          = 30; //0..90
        private const float         rollLimit           = 30; //0..90
        private const float         yawLimit            = 30; //0..90
        private const float         throttleIncrease    = 2.0f; //увелечение газульки




        public readonly APID        pidThrottle     = new APID(1.00f, 0.1f, 0.05f);
        public readonly APID        pidPitch        = new APID(pFactor, iFactor, dFactor);
        public readonly APID        pidRoll         = new APID(pFactor, iFactor, dFactor);
        public readonly APID        pidYaw          = new APID(0.5f, 0.01f, 0.05f);



        public readonly ABasicGyro  gyro            = new ABasicGyro();



        public double pitchCorrection;
        public double rollCorrection;
        public double heightCorrection;

        public double yawCorrection = 0.0f;


        private bool mStopMotor = false;


        public void updateGyro(ADrone drone)
        {
            gyro.update(drone);
        }

        public void updateComputer( float controlPitch, 
                                    float controlRoll, 
                                    float controlThrottle, 
                                    float controlYaw, 
                                    float timeFrame)
        {
            
            pitchCorrection     = pidPitch      .update(controlPitch * pitchLimit, gyro.pitch, timeFrame);
            rollCorrection      = pidRoll       .update(gyro.roll, controlRoll * rollLimit, timeFrame);
            heightCorrection    = pidThrottle   .update(controlThrottle * throttleIncrease, gyro.velocityVector.Y, timeFrame);

            yawCorrection       = pidYaw        .update(controlYaw * yawLimit, gyro.yaw, timeFrame);




            //yawCorrection += controlYaw * yawLimit * timeFrame;


            //
            //yawCorrection = 0;

            if (controlThrottle < -0.7f)
            {
                mStopMotor = true;
            }
            else
            if (controlThrottle > 0.1f)
            {
                mStopMotor = false;
            }


            if (mStopMotor)
            {
                reset();
            }
        }



        public double throttle
        {
            get
            {
                return heightCorrection;
            }
        }


        public void reset()
        {
            pidPitch.reset();
            pidRoll.reset();
            pidThrottle.reset();

            pitchCorrection = 0;
            rollCorrection = 0;
            heightCorrection = 0;
            mStopMotor = true;
        }


        public bool isStopMotor()
        {
            return mStopMotor;
        }
    }
}
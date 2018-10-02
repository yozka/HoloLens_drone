﻿using System;
using System.Collections.Generic;
using System.Text;
using Urho;



namespace HolographicsDrone.Drone.Hardware
{
    ///-------------------------------------------------------------------
    //using PidController;
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

        private const float         pitchLimit  = 30; //0..90
        private const float         rollLimit   = 30; //0..90
        private const float         yawLimit    = 30; //0..90


        private const float pidMax = 90;
        private const float pidMin = -90;


       
        public readonly APID        pidThrottle     = new APID(1.00f, 0.1f, 0.05f);
        public readonly APID        pidPitch        = new APID(pFactor, iFactor, dFactor);
        public readonly APID        pidRoll         = new APID(pFactor, iFactor, dFactor);
        public readonly APID        pidYaw          = new APID(1.00f, 0.1f, 0.05f);

        /*
    public readonly PidController pidThrottle   = new PidController(1.0f, 0.1f, 0.05f,         10, -10);
    public readonly PidController pidPitch      = new PidController(pFactor, iFactor, dFactor,  pidMax, pidMin);
    public readonly PidController pidRoll       = new PidController(pFactor, iFactor, dFactor,  pidMax, pidMin);
    */

        public readonly ABasicGyro  gyro            = new ABasicGyro();



        public double pitchCorrection;
        public double rollCorrection;
        public double heightCorrection;

        public double yawCorrection;

        public void updateGyro(ADrone drone)
        {
            gyro.update(drone);
        }

        public void updateComputer( float controlPitch, 
                                    float controlRoll, 
                                    float controlHeight, 
                                    float controlYaw, 
                                    float timeFrame)
        {
            
            pitchCorrection     = pidPitch      .update(controlPitch * pitchLimit, gyro.pitch, timeFrame);
            rollCorrection      = pidRoll       .update(gyro.roll, controlRoll * rollLimit, timeFrame);
            heightCorrection    = pidThrottle   .update(controlHeight, gyro.velocityVector.Y, timeFrame);

            yawCorrection       = pidYaw        .update(controlYaw * yawLimit, gyro.yaw, timeFrame);


            /*
            TimeSpan time = TimeSpan.FromSeconds(timeFrame);

            pitchCorrection     = pidPitch.update(controlPitch * pitchLimit, gyro.pitch, time);
            rollCorrection      = pidRoll.update(gyro.roll, controlRoll * rollLimit, time);
            heightCorrection    = pidThrottle.update(controlHeight, gyro.velocityVector.Y, time);
            */

            //yawCorrection = controlYaw * timeFrame;


            //
            

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
        }

    }
}
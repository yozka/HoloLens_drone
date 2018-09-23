using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace HolographicsDrone.Drone.Hardware
{
    ///-------------------------------------------------------------------




     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Полетный контроллер
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AFlightSystem
    {

        public bool armed;
        private RX receiver;

        //set points
        public float setX;

        public float setY;
        public float setZ;
        public float throttle;
        public float angSetX;
        public float angSetZ;
        private IMU getIMU;

        //Motor outputs
        private Motor motorControl;

        public float[] motors;
        public float minThrottle;

        [Header("ACRO PIDS")]
        public float[] rollPID = new float[3];

        public float[] pitchPID = new float[3];
        public float[] yawPID = new float[3];

        private float rollEr;
        private float pitchEr;
        private float yawEr;

        private float rollPidOutput;
        private float pitchPidOutput;
        private float yawPidOutput;

        [Header("Angle mode PIDS")]
        public bool angleMode; //true if angle mode is selected

        public float maxAngle; //The maxium angle the craft with rotate to
        public float[] rollAnglePID = new float[3];
        public float[] pitchAnglePID = new float[3];

        private float pitchAngErr;
        private float rollAngErr;

        private float rollAnglePidOutput;
        private float pitchAnglePidOutput;

        public float pidErrorMax;

        private float loopTime = 0.0005f; //Freqency that the PID loop is ran at. Set at 2khz to emulate a stm32f3 processor

        #region acro pid varibles

        private float rollErrSum;
        private float lastRollErr;
        private float rollDErr;

        private float pitchErrSum;
        private float lastPitchErr;
        private float pitchDErr;

        private float yawErrSum;
        private float lastYawErr;
        private float yawDErr;

        #endregion acro pid varibles

        #region angle pid varibles

        private float rollAngErrSum;
        private float lastAngRollErr;
        private float rollAngDErr;

        private float pitchAngErrSum;
        private float lastAngPitchErr;
        private float pitchAngDErr;

        #endregion angle pid varibles

        [Header("Rates and Expo")]
        public float rollAndPitchRate;

        public float rollAndPitchExpo;
        public float yawRate;
        public float yawExpo;
        public float maxDegrees; //max command

        //floats to save input values from reciever
        private float pitDeg;

        private float rollDeg;
        private float yawDeg;

        // Use this for initialization
        private void Start()
        {
            //runs the pidLoop at 2khz
            InvokeRepeating("pidLoop", 0, loopTime);

            //gets access to all other scripts attached to this game obejct
            receiver = gameObject.GetComponent<RX>();
            getIMU = gameObject.GetComponent<IMU>();
            motorControl = gameObject.GetComponent<Motor>();
        }

        // Update is called once per frame
        private void Update()
        {
            #region Armed Check

            if (receiver.outputSwitch == 2000)
            {
                armed = true;
            }
            else
            {
                armed = false;
            }

            #endregion Armed Check

            #region Angle mode check

            if (receiver.outputModeSwitch == 2000)
            {
                angleMode = true;
            }
            else
            {
                angleMode = false;
            }

            #endregion Angle mode check
        }

        //running at 2khz
        private void pidLoop()
        {
            //run rate and expo conversion;
            applyRatesAndExpo();

            //read gyro
            float curRoll = getIMU.gyroRoll;
            float curPitch = getIMU.gyroPitch;
            float curYaw = getIMU.gyroYaw;

            if (armed)
            {
                #region Angle mode PID loop

                if (angleMode)
                {
                    //update acceloromter values
                    getIMU.accUpdate();

                    //determin errors
                    rollAngErr = setZ - getIMU.accRoll;
                    rollAngErrSum += (rollAngErr * loopTime);

                    pitchAngErr = setX - getIMU.accPitch;
                    pitchAngErrSum += (pitchAngErr * loopTime);

                    //determin d error
                    rollAngDErr = (rollAngErr - lastAngRollErr) / loopTime;
                    pitchAngDErr = (pitchAngErr - lastAngPitchErr) / loopTime;

                    // calculate rate pids outputoutput
                    rollAnglePidOutput = rollAnglePID[0] * rollAngErr + rollAnglePID[1] * rollAngErrSum + rollAnglePID[2] * rollAngDErr;
                    lastAngRollErr = rollAngErr;

                    pitchAnglePidOutput = pitchAnglePID[0] * pitchAngErr + pitchAnglePID[1] * pitchAngErrSum + pitchAnglePID[2] * pitchAngDErr;
                    lastAngPitchErr = pitchAngErr;

                    angSetZ = rollAnglePidOutput;
                    angSetX = pitchAnglePidOutput;
                }

                #endregion Angle mode PID loop

                #region Rate PID Loop

                //determin errors;
                if (angleMode)
                {
                    rollEr = angSetZ - curRoll;
                }
                else if (!angleMode)
                {
                    rollEr = setZ - curRoll;
                }

                rollErrSum += (rollEr * loopTime);

                if (angleMode)
                {
                    pitchEr = angSetX - curPitch;
                }
                else if (!angleMode)
                {
                    pitchEr = setX - curPitch;
                }

                pitchErrSum += (pitchEr * loopTime);

                yawEr = setY - curYaw;
                yawErrSum += (yawEr * loopTime);

                //determin d error
                rollDErr = (rollEr - lastRollErr) / loopTime;
                pitchDErr = (pitchEr - lastPitchErr) / loopTime;
                yawDErr = (yawEr - lastYawErr) / loopTime;

                // calculate rate pids outputoutput
                rollPidOutput = rollPID[0] * rollEr + rollPID[1] * rollErrSum + rollPID[2] * rollDErr;
                lastRollErr = rollEr;

                pitchPidOutput = pitchPID[0] * pitchEr + pitchPID[1] * pitchErrSum + pitchPID[2] * pitchDErr;
                lastPitchErr = pitchEr;

                yawPidOutput = yawPID[0] * yawEr + yawPID[1] * yawErrSum + yawPID[2] * yawDErr;
                lastYawErr = yawEr;

                #endregion Rate PID Loop

                //calculate motor output
                motors[0] = (throttle + rollPidOutput + pitchPidOutput - yawPidOutput);
                motors[1] = (throttle + rollPidOutput - pitchPidOutput + yawPidOutput);
                motors[2] = (throttle - rollPidOutput + pitchPidOutput + yawPidOutput);
                motors[3] = (throttle - rollPidOutput - pitchPidOutput - yawPidOutput);

                for (int i = 0; i < motors.Length; i++)
                {
                    if (motors[i] < minThrottle)
                    {
                        motors[i] = minThrottle;
                    }
                }
            }
            else
            {
                for (int i = 0; i < motors.Length; i++) //If pid loop not running set motor outputs to zero
                {
                    motors[i] = 999f;
                }

                //reset Pid errors when disarmed
                rollDErr = 0f;
                pitchDErr = 0f;
                yawDErr = 0f;
                rollAngDErr = 0f;
                pitchAngDErr = 0f;

                rollAnglePidOutput = 0f;
                pitchAnglePidOutput = 0f;
            }

            //Send outputs to motors
            motorControl.convertToForce(motors[0], 1);
            motorControl.convertToForce(motors[1], 2);
            motorControl.convertToForce(motors[2], 3);
            motorControl.convertToForce(motors[3], 4);
        }

        //Gets outputs from reciever and applies rate and expo to them to create the setpoints for thePID loop
        private void applyRatesAndExpo()
        {
            pitDeg = convertPWMToDegree(receiver.outputRightY);
            rollDeg = convertPWMToDegree(receiver.outputRightX);
            yawDeg = convertPWMToDegree(receiver.outputLeftX);

            throttle = receiver.outputLeftY;

            if (angleMode)
            {
                setY = yawDeg;
                setX = pitDeg;

                if (rollDeg >= 0)
                {
                    setZ = rollDeg;
                }
                else if (rollDeg < 0)
                {
                    setZ = rollDeg;
                }
            }
            else if (!angleMode)
            {
                pitDeg = pitDeg / rollAndPitchRate;
                rollDeg = rollDeg / rollAndPitchRate;
                yawDeg = yawDeg / yawRate;

                setY = ((1 - yawExpo) * Mathf.Pow(yawDeg, 3));
                setX = ((1 - rollAndPitchExpo) * Mathf.Pow(pitDeg, 3));
                setZ = ((1 - rollAndPitchExpo) * Mathf.Pow(rollDeg, 3));
            }
        }

        private float convertPWMToDegree(float inputPwm)
        {
            float outputDeg;
            outputDeg = inputPwm - 1000;
            outputDeg = outputDeg / 1000f;
            outputDeg = outputDeg - 0.5f;
            if (!angleMode)
            {
                outputDeg = outputDeg * maxDegrees;
            }
            if (angleMode)
            {
                outputDeg = outputDeg * (maxAngle * 2);
            }
            return outputDeg;
        }

        #region getVaribles

        public float RollP
        {
            get { return rollPID[0]; }
            set { rollPID[0] = value; }
        }

        public float RollI
        {
            get { return rollPID[1]; }
            set { rollPID[1] = value; }
        }

        public float RollD
        {
            get { return rollPID[2]; }
            set { rollPID[2] = value; }
        }

        public float pitchP
        {
            get { return pitchPID[0]; }
            set { pitchPID[0] = value; }
        }

        public float pitchI
        {
            get { return pitchPID[1]; }
            set { pitchPID[1] = value; }
        }

        public float pitchD
        {
            get { return pitchPID[2]; }
            set { pitchPID[2] = value; }
        }

        public float yawP
        {
            get { return yawPID[0]; }
            set { yawPID[0] = value; }
        }

        public float yawI
        {
            get { return yawPID[1]; }
            set { yawPID[1] = value; }
        }

        public float yawD
        {
            get { return yawPID[2]; }
            set { yawPID[2] = value; }
        }

        public float PRRate
        {
            get { return rollAndPitchRate; }
            set { rollAndPitchRate = value; }
        }

        public float PRExpo
        {
            get { return rollAndPitchExpo; }
            set { rollAndPitchExpo = value; }
        }

        public float YRate
        {
            get { return yawRate; }
            set { yawRate = value; }
        }

        public float YExpo
        {
            get { return yawExpo; }
            set { yawExpo = value; }
        }

        #endregion getVaribles
    }
}
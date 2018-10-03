using System;
using Urho;

namespace HolographicsDrone.Drone.Hardware
{
    ///-------------------------------------------------------------------

    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// система с гироскопом
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ABasicGyro
    {
        ///--------------------------------------------------------------------
        private float   mGyroRoll       = 0;
        private float   mGyroPitch      = 0;
        private float   mGyroYaw        = 0;
        private float   mAltitude       = 0; // The current altitude from the zero position
        private Vector3 mVelocityVector = Vector3.Zero; // Velocity vector
        private float   mVelocityScalar = 0; // Velocity scalar value


        private bool mActive = true;
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ABasicGyro()
        {

        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// выставление активности гироскопа
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void setActive(bool active)
        {
            mActive = active;
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обновление системы
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void update(ADrone drone)
        {
            if (!mActive)
            {
                mGyroPitch = 0;
                mGyroRoll = 0;
                mGyroYaw = 0;
                mVelocityVector = Vector3.Zero;
                mAltitude = 0;
                return;
            }

            var mat = drone.model.rigidBody.Rotation;
            //var mat = drone.Node.WorldRotation;

            Vector3 angles = mat.ToEulerAngles();


            mGyroPitch = angles.X;
            mGyroRoll = angles.Z;
            mGyroYaw = angles.Y;

            mGyroPitch = normal(mGyroPitch);
            mGyroRoll = normalRoll(mGyroRoll);
            mGyroYaw = normalYaw(mGyroYaw);


            /*
                        mGyroPitch = (mGyroPitch > 180) ? mGyroPitch - 360 : mGyroPitch;
                        mGyroRoll = (mGyroRoll > 180) ? mGyroRoll - 360 : mGyroRoll;
                        mGyroYaw = (mGyroYaw > 180) ? mGyroYaw - 360 : mGyroYaw;
             */

            mAltitude = drone.model.rigidBody.Position.Y;


            mVelocityVector = drone.model.rigidBody.LinearVelocity;
            mVelocityScalar = mVelocityVector.LengthSquared;

           

        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public float normal(float val)
        {
            /*if (val > 90 && val < 270)
            {
                val -= 180;
            }

            if (val > 270 && val < 360)
            {
                val -= 360;
            }
            */

            /*
            if (val > 180)
            {
                val -= 360;
            }
            */

            return val;
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public float normalRoll(float val)
        {
            if (val > 90 && val < 270)
            {
                val -= 180;
            }

            if (val > 270 && val < 360)
            {
                val -= 360;
            }
  

            return val;
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public float normalYaw(float val)
        {
   
            if (val > 180)
            {
                val -= 360;
            }
   
            return val;
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public float roll
        {
            get
            {
                return mGyroRoll;
            }
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public float pitch
        {
            get
            {
                return mGyroPitch;
            }
        }
        ///--------------------------------------------------------------------



         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public float yaw
        {
            get
            {
                return mGyroYaw;
            }
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public Vector3 velocityVector
        {
            get
            {
                return mVelocityVector;
            }
        }
        ///--------------------------------------------------------------------

    }
}
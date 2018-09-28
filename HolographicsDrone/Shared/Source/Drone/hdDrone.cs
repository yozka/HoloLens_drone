using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace HolographicsDrone.Drone
{
    ///-------------------------------------------------------------------
    using Hardware;
    ///-------------------------------------------------------------------





    ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Компанент, который создает дрона
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ADrone
            :
                Component
    {
        ///-------------------------------------------------------------------
        private readonly AControlSignal mControlSignal = new AControlSignal();
        private readonly AComputerModule mComputer = new AComputerModule();
        private readonly List<AMotor> mMotors = new List<AMotor>();

        private ADroneModel mModel = null;


        private float mThrottleIncrease = 10.0f;
        ///-------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ADrone()
        {
            ReceiveSceneUpdates = true;


            //создание моторов
            var creator = new ACreatorMotor();
            mMotors.Add(creator.frontLeft());
            mMotors.Add(creator.frontRight());
            mMotors.Add(creator.rearLeft());
            mMotors.Add(creator.rearRight());
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// сигнал управления
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AControlSignal controlSignal
        {
            get
            {
                return mControlSignal;
            }
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// гироскоп
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ABasicGyro gyro
        {
            get
            {
                return mComputer.gyro;
            }
        }
        ///--------------------------------------------------------------------



        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// гироскоп
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AComputerModule computer
        {
            get
            {
                return mComputer;
            }
        }
        ///--------------------------------------------------------------------




        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возвратим модель квадрика
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ADroneModel model
        {
            get
            {
                return mModel;
            }
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Инциализация
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void start()
        {
            var save = mModel;
            mModel = Node.GetComponent<ADroneModel>();
            if (save == mModel)
            {
                return;
            }
            ///
            /// первый старт
            //model.rigidBody.Mass = mass;
        }
        ///--------------------------------------------------------------------

        float mass = 0.5f;

        float time = 0;

        bool ok = true;

        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// 
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
            if (mModel == null)
            {
                start();
                return;
            }

            Scene.UpdateEnabled = !Application.Input.GetKeyDown(Key.LeftCtrl);


            if (Application.Input.GetKeyDown(Key.Space))
            {

                Node.Position = new Vector3(0, 5, -6.0f);
                Node.Rotation = new Quaternion(x: 0, y: 0, z: 10);

                var rb = mModel.rigidBody;

                rb.SetLinearVelocity(Vector3.Zero);
                rb.SetAngularVelocity(Vector3.Zero);

                //rb.ResetToDefault();
                // rb.ResetForces();

                //rb.Mass = mass;

                mComputer.reset();
            }


            if (Application.Input.GetKeyDown(Key.Z))
            {
                ok = true;
            }

            if (Application.Input.GetKeyDown(Key.X))
            {
                ok = false;
            }


            if (Application.Input.GetKeyDown(Key.Q))
            {
                mComputer.gyro.setActive(true);
            }

            if (Application.Input.GetKeyDown(Key.W))
            {
                mComputer.gyro.setActive(false);
            }


            float time = timeStep;//MathHelper.Clamp(timeStep, 0.001f, 0.01f);
                                  //Console.WriteLine(time);

            mComputer.updateGyro(this);
            mComputer.updateComputer(mControlSignal.pitch,
                                        mControlSignal.roll,
                                        mControlSignal.throttle * mThrottleIncrease,
                                        mControlSignal.yaw,
                                        time);

            if (ok)
            {
                computeMotors(time);
                //test(time);
            }



        }
        ///--------------------------------------------------------------------





        private void test(float timeStep)
        {
       
            //mModel.rigidBody.ResetForces();

            var power = mComputer.throttle;
            var roll = mComputer.rollCorrection;
            double total = power * 0.005f;

            double drool = 0.001f;
            double kf = 1.00f;

            double powerLeft = total; 
            double powerRight = total;
          
            powerLeft  += roll * drool * kf;
            powerRight += roll * drool * kf;
         
            /*
            if (roll > 0)
            {
                powerLeft += drool * roll * kf;
            }

            if (roll < 0)
            {
               powerRight -= drool * roll * kf;

            }*/
       
        
            motorForce(EMotor.frontLeft, powerLeft);
            motorForce(EMotor.rearLeft, powerLeft );

            motorForce(EMotor.frontRight, powerRight);
            motorForce(EMotor.rearRight, powerRight);
     

     
            bool r = Application.Input.GetKeyDown(Key.N1);
            bool ri = Application.Input.GetKeyDown(Key.N2);
            double powMax = 0.1f;
         
            if (Application.Input.GetKeyDown(Key.I) || r)
            {
                motorForce(EMotor.frontLeft, powMax);
            }
            if (Application.Input.GetKeyDown(Key.O) || ri)
            {
                motorForce(EMotor.frontRight, powMax);
            }

            if (Application.Input.GetKeyDown(Key.K) || r)
            {
                motorForce(EMotor.rearLeft, powMax);
            }
            if (Application.Input.GetKeyDown(Key.L) || ri)
            {
                motorForce(EMotor.rearRight, powMax);
            }


            if (Application.Input.GetKeyDown(Key.U))
            {
                motorForce(EMotor.frontLeft, -powMax);
            }

        }

        private void motorForce(EMotor tp, double power)
        {
            var rb = mModel.rigidBody;
            var motor = mModel.motor(tp);
            var thrust = motor.WorldUp;
            //thrust = Vector3.Up;
            var posA = motor.WorldPosition;
            var posB = Node.WorldPosition;
            var pos = posA - posB;
            pos.Y = 0;
            rb.ApplyImpulse(thrust * (float)power, pos);
        }



         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// просчет нагрузки на моторы
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void computeMotors(float timeStep)
        {
            double yaw = 0.0f;
            var rb = mModel.rigidBody;
            rb.ResetForces();

            //Vector3 thrust = Node.Up;
            //thrust.Normalize();
            //thrust = Vector3.Up;

            //Console.WriteLine(thrust);
            foreach (var motor in mMotors)
            {
                motor.updateForceValues(mComputer, timeStep);
                yaw += motor.sideForce;

                motorForce(motor.typeMotor, motor.upForce);

                //возьмем мотор
                /*
                var nodeMotor = mModel.motor(motor.typeMotor);
         
                var thrust = nodeMotor.Up;

                var impuls = thrust * (float)motor.upForce;
                if (impuls.Length > 0.001f)
                {
                    var pos = nodeMotor.Position;
                    //pos = pos * 2.5f;
                    //pos = pos * 0.5f;

                    rb.ApplyForce(impuls, pos);
  
                }
                */
            }

            const float zeroYaw = 0.001f;
            if (Math.Abs(yaw - zeroYaw) > zeroYaw)
            {
                //rb.ApplyTorque(Vector3.Up * (float)yaw);
            }


            return;
  
       
            //максимальный разворот
            float max_x = 20;
            float max_y = 20;
            float max_z = 20;


            var mat = Node.Rotation;
            var vector = mat.ToEulerAngles();


            //Console.WriteLine();
           // Node.Rotate(new Quaternion(0.0f, 0.0f, -vector.Z));

            var v1 = rb.AngularVelocity;
            if ((vector.Z > max_z && v1.Z > 0.0f) ||
                (vector.Z < -max_z && v1.Z < 0.0f))
            {
                v1.Z = 0;
                vector.Z = MathHelper.Clamp(vector.Z, -max_z, max_z);
                mComputer.reset();
            }

            if ((vector.X > max_x && v1.X > 0.0f) ||
                (vector.X < -max_x && v1.X < 0.0f))
            {
                v1.X = 0;
                vector.X = MathHelper.Clamp(vector.X, -max_x, max_x);
                mComputer.reset();
            }

            rb.SetAngularVelocity(v1);
            Node.Rotation = new Quaternion(vector.X, vector.Y, vector.Z);


            //var mat = rb.;
            /*
            var angles = mat.ToEulerAngles();
            angles.X = MathHelper.Clamp(angles.X, -max_x, max_x);
            angles.Y = MathHelper.Clamp(angles.Y, -max_y, max_y);
            angles.Z = MathHelper.Clamp(angles.Z, -max_z, max_z);
            Console.WriteLine(angles);
            */


            //mat.x
            //mat = Quaternion.(angles);
            /*
            mat.X = MathHelper.Clamp(mat.X, -max_x, max_x);
            mat.Y = MathHelper.Clamp(mat.Y, -max_y, max_y);
            mat.Z = MathHelper.Clamp(mat.Z, -max_z, max_z);


            Node.SetWorldRotation(mat);

            Console.WriteLine(mat.Z);*/
            //Node.Rotation = mat;




        }
        ///--------------------------------------------------------------------





    }
}

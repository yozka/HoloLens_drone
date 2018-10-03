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


        private float mThrottleIncrease = 2.0f;
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




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// сброс всего
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void reset()
        {
            Node.Position = Vector3.Zero;
            Node.Rotation = new Quaternion(x: 0, y: 0, z: 0);

            var rb = mModel.rigidBody;
            var mass = rb.Mass;
            rb.SetLinearVelocity(Vector3.Zero);
            rb.SetAngularVelocity(Vector3.Zero);

            
            
            rb.ResetToDefault();
            rb.ResetForces();
            rb.Mass = mass;

            mComputer.reset();
        }
        ///--------------------------------------------------------------------




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

          



            float time = timeStep;//MathHelper.Clamp(timeStep, 0.001f, 0.01f);
                                  //Console.WriteLine(time);

            mComputer.updateGyro(this);
            mComputer.updateComputer(mControlSignal.pitch,
                                        mControlSignal.roll,
                                        mControlSignal.throttle * mThrottleIncrease,
                                        mControlSignal.yaw,
                                        time);
            computeMotors(time);




        }
        ///--------------------------------------------------------------------





        private void motorForce(EMotor tp, double power)
        {
            var rb = mModel.rigidBody;
            var motor = mModel.motor(tp);
            var thrust = motor.WorldUp;
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

            foreach (var motor in mMotors)
            {
                motor.updateForceValues(mComputer, timeStep);
                yaw += motor.sideForce;

                motorForce(motor.typeMotor, motor.upForce);
            }

            const float zeroYaw = 0.001f;
            if (Math.Abs(yaw - zeroYaw) > zeroYaw)
            {
                rb.ApplyTorqueImpulse(Node.WorldUp * (float)yaw);
            }


        }
        ///--------------------------------------------------------------------





    }
}

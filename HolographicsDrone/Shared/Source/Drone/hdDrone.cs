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
        private AControlSignal  mControlSignal = new AControlSignal();
        private ADroneModel     mModel = null;
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
            set
            {
                mControlSignal = value;
            }

            get
            {
                return mControlSignal;
            }
        }
        ///--------------------------------------------------------------------







        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// система создание модели
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void createModel()
        {
            /*
            var cache = Application.ResourceCache;
            var model = Cre CreateComponent<StaticModel>();
            model.Model = cache.GetModel("robot.mdl");
            model.SetMaterial(cache.GetMaterial("robot.xml"));*/
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
            if (mModel == null)
            {
                mModel = Node.GetComponent<ADroneModel>();
                return;
            }

            var rigid = mModel.rigidBody;

            var force = new Vector3(0.0f, 2.9f, 0.1f);

            rigid.ApplyForce(force, mModel.motor(EMotor.frontLeft).Position);
            rigid.ApplyForce(force, mModel.motor(EMotor.frontRight).Position);
            rigid.ApplyForce(force, mModel.motor(EMotor.rearLeft).Position);
            rigid.ApplyForce(force, mModel.motor(EMotor.rearRight).Position);

            
            
            //test.ApplyImpulse(new Vector3(0.1f * timeStep, 1.4f * timeStep, 0));
            //test.ApplyTorqueImpulse(new Vector3(0, 100.6f * timeStep,0));

            //test.ApplyForce(new Vector3(0.0f, 12.0f, 0.1f), new Vector3(0.0f, 1.0f, 0.0f));


        }
        ///--------------------------------------------------------------------








    }
}

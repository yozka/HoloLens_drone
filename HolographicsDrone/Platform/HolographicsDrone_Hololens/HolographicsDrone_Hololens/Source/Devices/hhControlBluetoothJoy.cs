using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace HolographicsDrone.Drone
{
    ///-------------------------------------------------------------------
    using Hardware;
    using HolographicsDrone.Device;
    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Управлением дроном через блютузный джойстик
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AControlBluetoothJoy
            :
                Component
    {
        ///-------------------------------------------------------------------
        private ABluetoothJoy mJoy = null;
        ///-------------------------------------------------------------------




        ///-------------------------------------------------------------------
        private ADrone mDrone = null; //дрон которым будем управлять
        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AControlBluetoothJoy()
        {
            ReceiveSceneUpdates = true;


            mJoy = new ABluetoothJoy();
        }
        ///--------------------------------------------------------------------






        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обработка кнопок
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected override void OnUpdate(float timeStep)
        {
            if (!mJoy.isConnected())
            {
                return;
            }


            if (mDrone == null)
            {
                attachDrone();
                return;
            }

            var joy = mJoy.joy(EType.futaba);
            if (joy == null)
            {
                joy = mJoy.joy(0);
            }

            float elevator  = joy.getAxisPosition(3);
            float aileron   = joy.getAxisPosition(2);
            float rudder    = joy.getAxisPosition(0);
            float throttle  = joy.getAxisPosition(1);



            var signal          = mDrone.controlSignal;
            signal.throttle     = zero(throttle);
            signal.rudder       = zero(rudder);
            signal.aileron      = zero(aileron);
            signal.elevator     = zero(elevator);


            /*
            signal.throttle     = zero(0);
            signal.rudder       = zero(0);
            signal.aileron      = zero(0);
            signal.elevator     = zero(0);
            */
        }
        ///--------------------------------------------------------------------




        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// присоеденям дрона к управлению
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private float zero(float val)
        {
            const float range = 0.04f;
            if (Math.Abs(Math.Abs(val) - range) <= range)
            {
                return 0.0f;
            }
            return val;
            /*
            var dt = Math.Exp(Math.Abs(val));

            var data = (1.0f / Math.E) * dt;
            if (val < 0)
            {
                data = data * -1.0f;
            }

            if (Math.Abs(Math.Abs(data) - range) <= range)
            {
                return 0.0f;
            }


            return (float)data;*/
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// присоеденям дрона к управлению
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void attachDrone()
        {
            if (mDrone != null || Node == null)
            {
                return;
            }
            mDrone = Node.GetComponent<ADrone>();
        }
        ///--------------------------------------------------------------------




    }
}

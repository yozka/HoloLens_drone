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



            float elevator  = mJoy.getAxisPosition(3) * -1.0f;
            float aileron   = mJoy.getAxisPosition(2);
            float rudder    = mJoy.getAxisPosition(0);
            float throttle  = mJoy.getAxisPosition(1);



            var signal          = mDrone.controlSignal;
            signal.throttle     = zero(throttle);
            signal.rudder       = zero(rudder);
            signal.aileron      = zero(aileron);
            signal.elevator     = zero(elevator);


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
            const float range = 0.07f;
            if (Math.Abs(Math.Abs(val) - range) <= range)
            {
                return 0.0f;
            }
            return val;
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

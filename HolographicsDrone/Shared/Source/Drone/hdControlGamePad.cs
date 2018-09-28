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
    /// Управлением дроном через клавиатуру
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AControlGamePad
            :
                Component
    {
        ///-------------------------------------------------------------------
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
        public AControlGamePad()
        {
            ReceiveSceneUpdates = true;
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
            var input = Application.Input;
            if (input.NumJoysticks <= 0 )
            {
                return;
            }

            JoystickState joy;
            if (!input.TryGetJoystickState(0, out joy))
            {
                return;
            }

            if (mDrone == null)
            {
                attachDrone();
                return;
            }




            float elevator  = joy.GetAxisPosition(3) * -1.0f;
            float aileron   = joy.GetAxisPosition(2) * -1.0f;
            float rudder    = joy.GetAxisPosition(0);
            float throttle  = joy.GetAxisPosition(1) * -1.0f;



            var signal = mDrone.controlSignal;
            signal.throttle = zero(throttle);
            signal.rudder   = zero(rudder);
            signal.aileron  = zero(aileron);
            signal.elevator = zero(elevator);

           
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
            if (Math.Abs(val - range) <= range)
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

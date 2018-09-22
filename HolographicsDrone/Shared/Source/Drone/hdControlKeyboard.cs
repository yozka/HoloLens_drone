using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace HolographicsDrone.Drone
{
    ///-------------------------------------------------------------------




     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Управлением дроном через клавиатуру
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AControlKeyboard
            :
                Component
    {
        ///-------------------------------------------------------------------
        private const float cSpeedUp    = 2.0f;
        private const float cSpeedDown  = 2.0f;
        ///-------------------------------------------------------------------




        ///-------------------------------------------------------------------
        private readonly Dictionary<Urho.Key, float> mImpulse = new Dictionary<Key, float>();
        private ADrone mDrone = null; //дрон которым будем управлять
        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AControlKeyboard()
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
            impulsKey(Key.W, timeStep);
            impulsKey(Key.S, timeStep);
            impulsKey(Key.A, timeStep);
            impulsKey(Key.D, timeStep);

            if (mDrone == null)
            {
                attachDrone();
                return;
            }


        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обработка кнопок
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void impulsKey(Key key, float timeStep)
        {
            float imp = 0.0f;
            if (Application.Input.GetKeyDown(key))
            {
                //клавишу нажали, идет нарастание импульса
                imp = cSpeedUp * timeStep;
            }
            else
            {
                //клавишу отпустили идет спад импульса
                imp = cSpeedDown * timeStep;
            }

            
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

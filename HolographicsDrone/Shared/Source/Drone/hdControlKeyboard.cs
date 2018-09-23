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
    public class AControlKeyboard
            :
                Component
    {
        ///-------------------------------------------------------------------
        private const float cSpeedUp    = 500.0f;
        private const float cSpeedDown  = 800.0f;
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
            impulsKey(Key.Up,       timeStep);
            impulsKey(Key.Down,     timeStep);
            impulsKey(Key.Left,     timeStep);
            impulsKey(Key.Right,    timeStep);

            impulsKey(Key.A,        timeStep);
            impulsKey(Key.D,        timeStep);

            impulsKeyFixed(Key.W, Key.S,  timeStep);

            if (mDrone == null)
            {
                attachDrone();
                return;
            }

            float elevator  = -mImpulse[Key.Down]   + mImpulse[Key.Up];
            float aileron   = -mImpulse[Key.Left]   + mImpulse[Key.Right];
            float rudder    = -mImpulse[Key.A]      + mImpulse[Key.D];
            float throttle  = -mImpulse[Key.S]      + mImpulse[Key.W];

            var signal = new AControlSignal();
            signal.throttle = throttle;
            signal.rudder   = rudder;
            signal.aileron  = aileron;
            signal.elevator = elevator;

            mDrone.controlSignal = signal;
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
                imp = (1.0f / cSpeedUp) * timeStep * 1000.0f;
            }
            else
            {
                //клавишу отпустили идет спад импульса
                imp = -(1.0f / cSpeedDown) * timeStep * 1000.0f;
            }

            float data = mImpulse.ContainsKey(key) ? mImpulse[key] : 0;
            data += imp;
            data = MathHelper.Clamp(data, 0, 1);
            mImpulse[key] = data;
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обработка кнопок без возврата в позицию
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void impulsKeyFixed(Key keyA, Key keyB, float timeStep)
        {
            const float speed = 400;
            float imp = 0.0f;
            if (Application.Input.GetKeyDown(keyA))
            {
                //клавишу нажали, идет нарастание импульса
                imp = (1.0f / cSpeedUp) * timeStep * speed;
            }
            if (Application.Input.GetKeyDown(keyB))
            {
                //клавишу отпустили идет спад импульса
                imp = -(1.0f / cSpeedUp) * timeStep * speed;
            }

            float data = mImpulse.ContainsKey(keyA) ? mImpulse[keyA] : 0;
            data += imp;
            data = MathHelper.Clamp(data, 0, 1);
            mImpulse[keyA] = data;


            mImpulse[keyB] = 0;
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

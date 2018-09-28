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
    /// Сигнал для передачи управления
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AControlSignal

    {
        ///-------------------------------------------------------------------
        public float throttle       { get; set; }   //газ
        public float rudder         { get; set; }   //рысканье YAW
        public float elevator       { get; set; }   //Тангаж Pitch
        public float aileron        { get; set; }   //крен roll
        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AControlSignal()
        {
            throttle    = 0f;
            rudder      = 0f;
            elevator    = 0f;
            aileron     = 0f;
        }
        ///--------------------------------------------------------------------


        public float pitch //тангаж
        {
            get
            {
                return elevator;
            }
            set
            {
                elevator = value;
            }
        }



        public float roll //крен
        {
            get
            {
                return aileron;
            }
            set
            {
                aileron = value;
            }
        }

        public float yaw //крен
        {
            get
            {
                return rudder;
            }
            set
            {
                rudder = value;
            }
        }

    }
}

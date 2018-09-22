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
        public float throttle       { get; set; }
        public float rudder         { get; set; }
        public float elevator       { get; set; }
        public float aileron        { get; set; }
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








    }
}

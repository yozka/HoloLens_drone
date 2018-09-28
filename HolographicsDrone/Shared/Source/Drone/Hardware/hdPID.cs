using System;
using Urho;

namespace HolographicsDrone.Drone.Hardware
{
    ///-------------------------------------------------------------------

    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// ПИД регулятор
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class APID
    {
        ///--------------------------------------------------------------------
        private readonly double pFactor = 0;
        private readonly double iFactor = 0;
        private readonly double dFactor = 0;

        private double integral     = 0;
        private double lastError    = 0;


        public APID(double pFactor, double iFactor, double dFactor)
        {
            this.pFactor = pFactor;
            this.iFactor = iFactor;
            this.dFactor = dFactor;
        }


        public double update(float setpoint, float actual, float timeFrame)
        {
            double present = setpoint - actual;
            if (Math.Abs(present) < 0.1f)
            {
                integral = 0;
            }
            else
            {
                integral += present * timeFrame;
            }

            
           

            double deriv = (present - lastError) / timeFrame;
            lastError = present;
            double finalPID = present * pFactor + integral * iFactor + deriv * dFactor;

         
            if ((finalPID > -0.1) && (finalPID < 0.1))
            {
                finalPID = 0;
                integral = 0;
            }

            return finalPID;
        }


        public void reset()
        {
            integral = 0;
            lastError = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Urho.Gui;
using Urho;

namespace HolographicsDrone.GUI
{
    ///-------------------------------------------------------------------
    using HolographicsDrone.Drone;
    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Информация по дрону
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ADebugInformation
            :
                UIElement
    {
        ///-------------------------------------------------------------------
        private readonly Node mDrone = null;

        private readonly Text mLabelSignal = null; //джойстик

        private readonly Timer mTimerUpdate = null;
        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ADebugInformation(Node drone)
        {
            mDrone = drone;

            var cache = Application.Current.ResourceCache;

            var color = new Color(0.9f, 0.9f, 0f);
            var text = new Text();
            text.Value = "Control signal";
            text.SetColor(color);
            text.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 20);
            text.SetPosition(0, 0);
            AddChild(text);

            mLabelSignal = new Text();
            mLabelSignal.Value = "[error]";
            mLabelSignal.SetColor(color);
            mLabelSignal.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 20);
            mLabelSignal.SetPosition(0, 40);
            AddChild(mLabelSignal);


            mTimerUpdate = new Timer(100);
            mTimerUpdate.Elapsed += onTimedUpdate;
            mTimerUpdate.AutoReset = true;
            mTimerUpdate.Enabled = true;
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// обновление информации
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void onTimedUpdate(Object source, System.Timers.ElapsedEventArgs e)
        {
            string sv = "Not drone";
            var drone = mDrone.GetComponent<ADrone>();
            if (drone != null)
            {
                var signal = drone.controlSignal;
                if (signal != null)
                {
                    sv =  "Throttle: " + signal.throttle + "\n";
                    sv += "  Rudder: " + signal.rudder + "\n";
                    sv += "Elevator: " + signal.elevator + "\n";
                    sv += " Aileron: " + signal.aileron;
                }


                /*
                rollP.text = string.Format("{0}", drone.GetComponent<FC>().rollPID[0].ToString("0.00"));
                rollI.text = string.Format("{0}", drone.GetComponent<FC>().rollPID[1].ToString("0.00"));
                rollD.text = string.Format("{0}", drone.GetComponent<FC>().rollPID[2].ToString("0.00"));

                pitchP.text = string.Format("{0}", drone.GetComponent<FC>().pitchPID[0].ToString("0.00"));
                pitchI.text = string.Format("{0}", drone.GetComponent<FC>().pitchPID[1].ToString("0.00"));
                pitchD.text = string.Format("{0}", drone.GetComponent<FC>().pitchPID[2].ToString("0.00"));

                yawP.text = string.Format("{0}", drone.GetComponent<FC>().yawPID[0].ToString("0.00"));
                yawI.text = string.Format("{0}", drone.GetComponent<FC>().yawPID[1].ToString("0.00"));
                yawD.text = string.Format("{0}", drone.GetComponent<FC>().yawPID[2].ToString("0.00"));
                */
            }
            mLabelSignal.Value = sv;

        }
        ///--------------------------------------------------------------------









        }
}

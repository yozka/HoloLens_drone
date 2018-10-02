using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly Text mLabelGyro = null; //гироскоп квадрика
        private readonly Text mLabelPID = null; //гироскоп квадрика

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


            mLabelGyro = new Text();
            mLabelGyro.Value = "[error]";
            mLabelGyro.SetColor(Color.Black);
            mLabelGyro.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 20);
            mLabelGyro.SetPosition(0, 170 );
            AddChild(mLabelGyro);


            mLabelPID = new Text();
            mLabelPID.Value = "[error]";
            mLabelPID.SetColor(Color.Red);
            mLabelPID.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 20);
            mLabelPID.SetPosition(0, 320);
            AddChild(mLabelPID);


            //Application.Current.Engine.SubscribeToPostUpdate(args=> { onUpdate(args.TimeStep); });
            Application.Current.Engine.PostUpdate += (args => { onUpdate(args.TimeStep); });

        }
        ///--------------------------------------------------------------------


        



         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// обновление информации
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void onUpdate(float TimeStep)
        {
            string sv = "Not drone";
            string sg = "Not gyro";
            string sp = "Not pid";
            var drone = mDrone.GetComponent<ADrone>();
            if (drone != null)
            {
                var signal = drone.controlSignal;
                if (signal != null)
                {
                    sv =  "        Throttle: " + signal.throttle.ToString("0.00") + "\n";
                    sv += "  Aileron / Roll: " + signal.aileron.ToString("0.00") + "\n";
                    sv += "Elevator / Pitch: " + signal.elevator.ToString("0.00") + "\n";
                    sv += "    Rudder / Yaw: " + signal.rudder.ToString("0.00");
                }

                var gyro = drone.gyro;
                if (gyro != null)
                {
                    sg = "---Gyro-----\n";
                    sg += " Roll: " + gyro.roll.ToString("0.00") + "\n";
                    sg += "Pitch: " + gyro.pitch.ToString("0.00") + "\n";
                    sg += "  Yaw: " + gyro.yaw.ToString("0.00") + "\n";
                    sg += "Velty: " + gyro.velocityVector.Y.ToString("0.0000");
                }

                var comp = drone.computer;
                if (comp != null)
                {
                    sp = "---PID-----\n";
                    sp += " Roll: " + comp.rollCorrection.ToString("0.00") + "\n";
                    sp += "Pitch: " + comp.pitchCorrection.ToString("0.00") + "\n";
                    sp += "Throt: " + comp.throttle.ToString("0.00");
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
            mLabelGyro.Value = sg;
            mLabelPID.Value = sp;
        }
        ///--------------------------------------------------------------------



 





        }
}

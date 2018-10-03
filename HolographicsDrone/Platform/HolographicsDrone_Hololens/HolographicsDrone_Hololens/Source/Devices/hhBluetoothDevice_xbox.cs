using System;
using System.Collections.Generic;
using System.Text;
using Urho;

using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Foundation;

using System.Diagnostics;

namespace HolographicsDrone.Device
{
    ///-------------------------------------------------------------------
    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// базовый парсер данных
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ABluetoothDevice_xbox
                    :
                        ABluetoothDevice
    {
        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Тип устройства
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public override EType typeDevice { get { return EType.xbox; } }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ABluetoothDevice_xbox(DeviceInformation deviceinfo, HidDevice device)
            :
                base(deviceinfo, device)
        {

        }
        ///--------------------------------------------------------------------








         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Данные пришли
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected override void onUpdateData()
        {
            /*
            0x20: Button data
            Sent everytime controller input values change. Following code is from taken from Chrome web browser.

            struct XboxOneButtonData
            {
                uint8_t type;
                uint8_t const_0;
                uint16_t id;

                bool sync : 1;
                bool dummy1 : 1;  // Always 0.
                bool start : 1;
                bool back : 1;

                bool a : 1;
                bool b : 1;
                bool x : 1;
                bool y : 1;

                bool dpad_up : 1;
                bool dpad_down : 1;
                bool dpad_left : 1;
                bool dpad_right : 1;

                bool bumper_left : 1;
                bool bumper_right : 1;
                bool stick_left_click : 1;
                bool stick_right_click : 1;

                uint16_t trigger_left;
                uint16_t trigger_right;

                int16_t stick_left_x;
                int16_t stick_left_y;
                int16_t stick_right_x;
                int16_t stick_right_y;
            };
            */

            if (mData.Length != 15)
            {
                return;
            }

            //mData[11] = кнопки

            /*if (mData[0] != 0x20)
            {
                return;
            }*/

            mAxis[0] = parseData(1, 2);
            mAxis[1] = parseData(3, 4) * -1.0f;
            mAxis[2] = parseData(5, 6) * -1.0f;
            mAxis[3] = parseData(7, 8) * -1.0f;
            mAxis[4] = parseData(9, 10);

            //Debug.WriteLine(mData.ToString());
            //rawDebugFF();
            //rawDebugAxis();
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// парсер данных
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private float parseData(int LO, int HI)
        {
            var val = axisShort(LO, HI);
            float dif = 2.0f / 65535.0f;
            val = dif * val - 1.0f;
            return val;
        }
        ///--------------------------------------------------------------------










    }
}

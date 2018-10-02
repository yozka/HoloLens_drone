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
    public class ABluetoothDevice_futaba
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
        public override EType typeDevice { get { return EType.futaba; } }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ABluetoothDevice_futaba(DeviceInformation deviceinfo, HidDevice device)
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
            if (mData.Length != 11)
            {
                return;
            }

            mAxis[0] = parseData(5, 6);
            mAxis[1] = parseData(7, 8);
            mAxis[2] = parseData(1, 2) * -1.0f;
            mAxis[3] = parseData(3, 4) * -1.0f;
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
            float dif = 2.0f / 1000.0f;
            val = dif * val - 1.0f;
            return val;
        }
        ///--------------------------------------------------------------------









        



    }
}

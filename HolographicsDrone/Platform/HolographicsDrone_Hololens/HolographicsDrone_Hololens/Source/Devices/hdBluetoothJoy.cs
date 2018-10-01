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
    /// Джойстик
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ABluetoothJoy
    {
        ///-------------------------------------------------------------------



        ///-------------------------------------------------------------------
        ///-------------------------------------------------------------------
        //Data for BluetoothLE conection.
        public const ushort usagePage = 0x0001; //Generic
        public const ushort usageId = 0x0005; //GamePads

        private HidDevice mDevice = null; //сам девайс


        //Data
        private byte[] mData;

        //Buttons
        private float[] mAxis = new float[10];
        ///--------------------------------------------------------------------




        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ABluetoothJoy()
        {
            for (int i = 0; i < mAxis.Count(); i++)
            {
                mAxis[i] = 0.0f;
            }
            connect();
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Коннектор
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public async void connect()
        {
            if (mDevice != null)
            {
                return;
            }

            string Selector = HidDevice.GetDeviceSelector(usagePage, usageId);
            var Devices = await DeviceInformation.FindAllAsync(Selector);
            if (Devices.Count > 0)
            {
                var dev = Devices.ElementAt(0);

                mDevice = await HidDevice.FromIdAsync(dev.Id, Windows.Storage.FileAccessMode.Read);
                TypedEventHandler<HidDevice, HidInputReportReceivedEventArgs> input = new TypedEventHandler<HidDevice, HidInputReportReceivedEventArgs>(this.onInputReportEvent);
                mDevice.InputReportReceived += input;
            }
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Данные пришли
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void updateData(Windows.Storage.Streams.IBuffer buffer)
        {
            this.mData = buffer.ToArray();
            parseData();
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// парсер данных
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private float axis(int LO, int HI)
        {
            int vlo = mData[LO];
            int vhi = mData[HI];

            int value = vlo | (vhi << 8);

            return (float)value;
        }
        ///--------------------------------------------------------------------



        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// парсер данных
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void parseData()
        {
            //Patern for this GamePad: 8 Bytes Length
            //Byte 0: |12345678| - Unknown, seens like a header
            //Byte 1: |12345678| - Stick 1 x axis. 0=Left 255=Right
            //Byte 2: |12345678| - Stick 1 y axis. 0=Up 255=Down
            //Byte 3: |12345678| - Stick 2 x axis. 0=Left 255=Right
            //Byte 4: |12345678| - Stick 2 y axis. 0=Up 255=Down
            //Byte 5: |12345678| - 1234-> Unknown | 5678 -> Directional, 0 to 8, Defalt 8, Clockwise, top = 0, top/rigth = 1, rigth = 2...top/left=7
            //Byte 6: |12345678| - 1->Right Button, 2->Left Button, 3->Unknown, 4->Button 4, 5->Button 3, 6->Unknown, 7->Button 2, 8->Button 1
            //Byte 7: |12345678| - 1->Play Button, 234->Unknown, 5->Start Button, 6->Select Button


            mAxis[0] = axis(1, 2);
            mAxis[1] = axis(3, 4);
            mAxis[2] = axis(5, 6);
            mAxis[3] = axis(7, 8);
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// подпистчик обновление данных
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void onInputReportEvent(HidDevice sender, HidInputReportReceivedEventArgs e)
        {
            this.updateData(((HidInputReport)e.Report).Data);
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// проверка, есть коннект или нет
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public bool isConnected()
        {
            if (mDevice == null)
            {
                return false;
            }

            return true;
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат данных
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public float getAxisPosition(int index)
        {
            int id = index;
            switch (index)
            {
                case 3 :    { id = 1; break; }
                case 2:     { id = 0; break; }
                case 1:     { id = 3; break; }
                case 0:     { id = 2; break; }
            }

            float val = mAxis[id];
            float dif = 2.0f / 1000.0f;
            val = dif * val - 1.0f;

            return val;
        }
        ///--------------------------------------------------------------------

        



    }
}

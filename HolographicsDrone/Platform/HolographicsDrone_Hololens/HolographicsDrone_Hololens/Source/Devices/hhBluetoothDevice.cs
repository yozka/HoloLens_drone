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


    public enum EButton
    {
        A,
        B,
        X,
        Y,
        start
    };


    public enum EType
    {
        none,
        xbox,
        futaba
    };



     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// базовый парсер данных
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ABluetoothDevice
    {
        ///-------------------------------------------------------------------
        private readonly DeviceInformation  mInfo   = null;
        private readonly HidDevice          mDevice = null;


        private bool                        mConnected = true;


        protected byte[]                    mData;
        protected float[]                   mAxis       = new float[10];
        protected Dictionary<EButton, bool> mButtons    = new Dictionary<EButton, bool>();
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Тип устройства
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public virtual EType typeDevice { get { return EType.none; } }
        ///--------------------------------------------------------------------






        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ABluetoothDevice(DeviceInformation deviceinfo, HidDevice device)
        {
            mInfo = deviceinfo;
            mDevice = device;

            for (int i = 0; i < mAxis.Count(); i++)
            {
                mAxis[i] = 0.0f;
            }



            TypedEventHandler<HidDevice, HidInputReportReceivedEventArgs> input = new TypedEventHandler<HidDevice, HidInputReportReceivedEventArgs>(onInputReportEvent);
            mDevice.InputReportReceived += input;
        }
        ///--------------------------------------------------------------------









         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// индификатор девайса
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public string id
        {
            get
            {
                return mInfo.Id;
            }
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// проверка, есть соеденение или нет
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public bool isConnected()
        {
            return mConnected;
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Данные пришли
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected virtual void onUpdateData()
        {
          
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// парсер данных
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected float axisShort(int LO, int HI)
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
        /// подпистчик обновление данных
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void onInputReportEvent(HidDevice sender, HidInputReportReceivedEventArgs e)
        {
            mData =((HidInputReport)e.Report).Data.ToArray();
            onUpdateData();
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
            return mAxis[index];
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// отладочная информация
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void rawDebug()
        {
            Debug.Write("Data:");
            for (int i = 0; i < mData.Length; i++)
            {
                Debug.Write(Convert.ToString(mData[i], 2).PadLeft(8, '0') + "|");
            }
            Debug.WriteLine("");
        }

        public void rawDebugFF()
        {
            Debug.Write("Data:");
            for (int i = 0; i < mData.Length; i++)
            {
                Debug.Write(Convert.ToString(mData[i], 16).PadLeft(2, '0') + "|");
            }
            Debug.WriteLine("");
        }

        public void rawDebugAxis()
        {
            Debug.Write("Axis: ");
            for (int i = 0; i < mAxis.Length; i++)
            {
                Debug.Write(mAxis[i].ToString("0.00").PadLeft(6, '0') + "|");
            }
            Debug.WriteLine("");
        }

    }
}

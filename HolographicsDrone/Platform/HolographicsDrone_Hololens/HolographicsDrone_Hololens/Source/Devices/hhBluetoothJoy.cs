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
        //Data for BluetoothLE conection.
        public const ushort usagePage = 0x0001; //Generic
        public const ushort usageId = 0x0005; //GamePads


        private List<ABluetoothDevice> mDevices = new List<ABluetoothDevice>(); //сам девайс
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

            connect();
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// создание девайса конкретного
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private ABluetoothDevice create(DeviceInformation deviceinfo, HidDevice device)
        {
            ABluetoothDevice dev = null;
            if (device.ProductId    == 0xF000 &&
                device.VendorId     == 0x0047)
            {
                dev = new ABluetoothDevice_futaba(deviceinfo, device);
            }

            if (device.ProductId    == 0x02e0 &&
                device.VendorId     == 0x045e)
            {
                dev = new ABluetoothDevice_xbox(deviceinfo, device);
            }


            return dev;
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
            string selector = HidDevice.GetDeviceSelector(usagePage, usageId);
            var devices = await DeviceInformation.FindAllAsync(selector);
            for (int i = 0; i < devices.Count; i++)
            {
                //поиск уже созданного девайса
                var devInfo = devices.ElementAt(i);
                bool isOld = false;
                foreach(var device in mDevices)
                {
                    if (device.id == devInfo.Id)
                    {
                        isOld = true;
                        break;
                    }
                }
                //

                if (isOld)
                {
                    continue;
                }


                var devNativ = await HidDevice.FromIdAsync(devInfo.Id, Windows.Storage.FileAccessMode.Read);
                if (devNativ == null)
                {
                    continue;
                }

                var deviceMain = create(devInfo, devNativ);
                if (deviceMain == null)
                {
                    continue;
                }

                mDevices.Add(deviceMain);
            }
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
            foreach (var device in mDevices)
            {
                if (device.isConnected())
                {
                    return true;
                }
            }

            return false;
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// количество джойстиков
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public int count()
        {
            return mDevices.Count;
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат джойстика
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ABluetoothDevice joy(int index)
        {
            return mDevices[index];
        }
        ///--------------------------------------------------------------------



         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возврат по типу
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ABluetoothDevice joy(EType type)
        {
            foreach (var device in mDevices)
            {
                if (device.typeDevice == type)
                {
                    return device;
                }
            }

            return null;
        }
        ///--------------------------------------------------------------------



    }
}

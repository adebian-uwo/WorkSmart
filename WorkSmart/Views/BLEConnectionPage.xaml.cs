using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.BLE.Abstractions.EventArgs;

namespace WorkSmart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BLEConnectionPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        //HashSet<IDevice> deviceList;// = new HashSet<IDevice>();
        IDevice device;

        public BLEConnectionPage()
        {
            
            InitializeComponent();Title = "BLE Connection";
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
            lv.ItemsSource = deviceList;
        }

        /// <summary>
        /// Select Items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (lv.SelectedItem == null)
            {
                return;
            }
            device = lv.SelectedItem as IDevice;
        }

        private void StatusClicked(object sender, EventArgs e)
        {
            var state = ble.State;

            DisplayAlert("Notice", state.ToString(), "OK !");
            if (state == BluetoothState.Off)
            {
                DisplayAlert("Notice", state.ToString(), "OK !");
            }
        }

        private async void ScanClicked(object sender, EventArgs e)
        {

            try
            {
                //We have to test if the device is scanning 
                if (!ble.Adapter.IsScanning)
                {
                    deviceList.Clear();
                    adapter.DeviceDiscovered += (s, a) =>
                    {
                        if (!String.IsNullOrWhiteSpace(a.Device.ToString()) && !deviceList.Contains(a.Device))
                            deviceList.Add(a.Device);

                    };
                    await adapter.StartScanningForDevicesAsync();

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", ex.Message.ToString(), "Error !");
            }
        }

        private async void ConnectClicked(object sender, EventArgs e)
        {

            try
            {
                await adapter.ConnectToDeviceAsync(device);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "You must select a device before clicking connect!", "OK");
            }
        }

        private async void DisconnectClicked(object sender, EventArgs e)
        {
            try
            {
                await adapter.DisconnectDeviceAsync(device);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "You must be connected to a device to disconnect!", "OK");
            }
        }

        IList<IService> Services;
        IService Service;

        IList<ICharacteristic> Characteristics;
        ICharacteristic Characteristic;

        IDescriptor descriptor;
        IList<IDescriptor> descriptors;
        /// <summary>
        /// Get list of services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TestBLE(object sender, EventArgs e)
        {
            //tested code that sends the arduino to start
            var Service = await device.GetServiceAsync(Guid.Parse("9A48ECBA-2E92-082F-C079-9E75AAE428B1"));
            var Characteristic = await Service.GetCharacteristicAsync(Guid.Parse("FE4E19FF-B132-0099-5E94-3FFB2CF07940"));
            byte[] start = new byte[1];
            start[0] = Convert.ToByte(true);
            await Characteristic.WriteAsync(start);
        }
        private async void TestBLE0(object sender, EventArgs e)
        {
            //tested code that sends the arduino to stop
            var Service = await device.GetServiceAsync(Guid.Parse("9A48ECBA-2E92-082F-C079-9E75AAE428B1"));
            var Characteristic = await Service.GetCharacteristicAsync(Guid.Parse("FE4E19FF-B132-0099-5E94-3FFB2CF07940"));
            byte[] start = new byte[1];
            start[0] = Convert.ToByte(false);
            await Characteristic.WriteAsync(start);
        }
    }
}
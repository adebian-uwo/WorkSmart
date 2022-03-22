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
    public partial class AboutPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        IDevice device;

        public AboutPage()
        {
            InitializeComponent();
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
                deviceList.Clear();
                adapter.DeviceDiscovered += (s, a) =>
                {
                    // repeated values in the device list with multiple searches
                    //must find a way to eliminate dupes
                    if(!String.IsNullOrWhiteSpace(a.Device.ToString()))
                        deviceList.Add(a.Device);
                };

                //We have to test if the device is scanning 
                if (!ble.Adapter.IsScanning)
                {
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
            catch (DeviceConnectionException ex)
            {
                //Could not connect to the device
                await DisplayAlert("Notice", ex.Message.ToString(), "OK");
            }
        }
    }
}
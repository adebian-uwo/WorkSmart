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
        IDevice device;

        public BLEConnectionPage()
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
            //Services = (IList<IService>)await device.GetServicesAsync();
            // Service = await device.GetServiceAsync(Guid.Parse("guid")); 
            //or we call the Guid of selected Device
            //foreach (IService s in Services)
            //{
            //    Console.WriteLine(s);
            //}
            var Service = await device.GetServiceAsync(Guid.Parse("9A48ECBA-2E92-082F-C079-9E75AAE428B1"));
            Characteristics = (IList<ICharacteristic>)await Service.GetCharacteristicsAsync();

            for (int i = 0; i<4; i++)
            {
                byte[] bytes = await Characteristics[i].ReadAsync();
                float myFloat = BitConverter.ToSingle(bytes, 0);
                Console.WriteLine("---------------------------------");
                Console.WriteLine(myFloat);
                //foreach (Byte b in bytes)
                //{
                   
                //    Console.WriteLine(b);
                    
                //}
               Console.WriteLine("---------------------------------");
                //Console.WriteLine("---------------------------------");
                //string test = Convert.ToBase64String(bytes);
                //Console.WriteLine(test);
                //Console.WriteLine("---------------------------------");
            }
            //descriptors = (IList<IDescriptor>)await Characteristics[0].GetDescriptorsAsync();

            //foreach (IDescriptor d in descriptors)
            //{
            //    byte[] bytes = await d.ReadAsync();
            //    //Console.WriteLine(bytes);
            //    Console.WriteLine("---------------------------------");
            //    string test = Convert.ToBase64String(bytes);
            //    Console.WriteLine(test);
            //    Console.WriteLine("---------------------------------");
            //    //var bytes = await c.ReadAsync();
            //    //Console.WriteLine(bytes);
            //}


            //Service = await device.GetServiceAsync(device.Id);
        }
    }
}
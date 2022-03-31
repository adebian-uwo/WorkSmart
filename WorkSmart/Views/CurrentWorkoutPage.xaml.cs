using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkSmart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWorkoutPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        //HashSet<IDevice> deviceList;// = new HashSet<IDevice>();
        IDevice device;
        public CurrentWorkoutPage()
        {
            InitializeComponent();
            
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            foreach(IDevice d in adapter.ConnectedDevices)
            {
                //Console.WriteLine(d.Name);
                if (d.Name == "Arduino Nano 33 BLE")
                    device = d;
            }
        }
        private void StatusClicked(object sender, EventArgs e)
        {
            var state = ble.State;
            //DisplayAlert("Notice", adapter.ConnectedDevices.ToString(), "OK!");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine(device.Name);
            Console.WriteLine("-------------------------------------------------------");

        }
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
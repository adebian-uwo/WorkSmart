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
        bool workingOut = false;

        public CurrentWorkoutPage()
        {
            InitializeComponent();
            ConnectDevice();
            Title = "Current Workout";
        }
        private void ConnectDevice()
        {
            //ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            foreach (IDevice d in adapter.ConnectedDevices)
            {
                //Console.WriteLine(d.Name);
                if (d.Name == "Arduino Nano 33 BLE")
                {
                    device = d;
                }
            }
        }
        private bool CheckConnection()
        {
            //ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            bool connected = false;
            foreach (IDevice d in adapter.ConnectedDevices)
            {
                //Console.WriteLine(d.Name);
                if (d.Name == "Arduino Nano 33 BLE")
                {
                    device = d;
                    connected = true;
                }
            }
            return connected;
        }

        int reps = 0;
        private void StatusClicked(object sender, EventArgs e)
        {
            //RepCount.BindingContext = slider;
           
              
            RepCount.Text = "Rep Count: " + reps.ToString();
            reps++;
            //var state = ble.State;
            //DisplayAlert("Notice", adapter.ConnectedDevices.ToString(), "OK!");
            //Console.WriteLine("-------------------------------------------------------");
            //Console.WriteLine(device.Name);
            //Console.WriteLine("-------------------------------------------------------");

        }
        private async void TestBLE(object sender, EventArgs e)//starts workout
        {
            reps = 0;
            RepCount.Text = "Rep Count: " + reps.ToString();
            if (CheckConnection())
            {
                //tested code that sends the arduino to start
                var Service = await device.GetServiceAsync(Guid.Parse("9A48ECBA-2E92-082F-C079-9E75AAE428B1"));
                var Characteristic = await Service.GetCharacteristicAsync(Guid.Parse("FE4E19FF-B132-0099-5E94-3FFB2CF07940"));
                byte[] start = new byte[1];
                start[0] = Convert.ToByte(true);
                await Characteristic.WriteAsync(start);
                workingOut = true;
                CountReps();
            }
            else 
            {
                await DisplayAlert("Notice", "You are not connected to a device!", "OK!");
            }
           
        }
        private async void CountReps()//make it an async void when i have data 
        {
            //tested code that sends the arduino to stop
            var Service = await device.GetServiceAsync(Guid.Parse("9A48ECBA-2E92-082F-C079-9E75AAE428B1"));
            var Characteristic = await Service.GetCharacteristicAsync(Guid.Parse("00002ACC-0000-1000-8000-00805F9B34FB"));

            while (workingOut)
            {

                try
                {
                    if (workingOut)
                    {
                        byte[] recieved = await Characteristic.ReadAsync();
                        Console.WriteLine("-----------------------------------------");
                        Console.WriteLine(recieved);
                        RepCount.Text = "Rep Count: " + BitConverter.ToInt32(recieved, 0).ToString();
                        Console.WriteLine(BitConverter.ToInt32(recieved,0));
                        Console.WriteLine("-----------------------------------------");
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                    //Console.WriteLine("-----------------------------------------");
                    //Console.WriteLine(e.ToString());
                    //Console.WriteLine("-----------------------------------------");
                }
            }




            //byte[] end = new byte[1];
            //end[0] = Convert.ToByte(false);
            //await Characteristic.WriteAsync(end);
            //RepCount.Text = "Rep Count: " + test.ToString();
            //test++;
        }
        private async void TestBLE0(object sender, EventArgs e)//ends workout
        {
            if (CheckConnection())
            {
                //tested code that sends the arduino to stop
                var Service = await device.GetServiceAsync(Guid.Parse("9A48ECBA-2E92-082F-C079-9E75AAE428B1"));
                var Characteristic = await Service.GetCharacteristicAsync(Guid.Parse("FE4E19FF-B132-0099-5E94-3FFB2CF07940"));
                byte[] end = new byte[1];
                end[0] = Convert.ToByte(false);
                workingOut = false;
                System.Threading.Thread.Sleep(1000);
                await Characteristic.WriteAsync(end);
            }
            else
            {
                await DisplayAlert("Notice", "You are not connected to a device!", "OK!");
            }
            
        }

    }
}
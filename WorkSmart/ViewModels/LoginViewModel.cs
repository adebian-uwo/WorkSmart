using System;
using System.Collections.Generic;
using System.Text;
using WorkSmart.Views;
using Xamarin.Forms;

namespace WorkSmart.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(BLEConnectionPage)}");
        }
    }
}

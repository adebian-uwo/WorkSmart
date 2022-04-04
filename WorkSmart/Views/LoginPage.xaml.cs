using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSmart.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkSmart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        private SQLiteAsyncConnection _connection;
        private ObservableCollection<User> _users;
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<User>();

            base.OnAppearing();
        }

        private async void Login_Button_Clicked(object sender, EventArgs e)
        {
            //Get email and password from page
            var myEmail = Email.Text;
            var myPassword = Password.Text;

            //Query database for a user with this username
            try
            {
                User hopefulReturn = await _connection.GetAsync<User>(myEmail);
                if (hopefulReturn == null)
                {
                    await DisplayAlert("Not Found", "Email does not exist. Please register or try again.", "OK");
                }
                else
                {
                    if (hopefulReturn.Password != myPassword)
                    {
                        await DisplayAlert("Incorrect Password", "Incorrect Password: Please register or try again.", "OK");
                    }
                    if (hopefulReturn.Password == myPassword)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(BLEConnectionPage)}");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Not Found", "Email does not exist. Please register or try again.", "OK");
            }

        }

        private async void Register_Button_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new RegistrationPage());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkSmart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {

        private SQLiteAsyncConnection _connection;
        private ObservableCollection<User> _users;

        public RegistrationPage()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();

        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<User>();

            base.OnAppearing();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //Get user information from form
            string firstName = fName.Text;
            string lastName = lName.Text;
            string uMail = regEmail.Text;
            string pWord = regPwd.Text;
            string conWord = regPwdCon.Text;

            //Make sure passwords match
            if (pWord != conWord)
            {
                await DisplayAlert("Password Problem", "Passwords do not match: please re-enter", "OK");
            }
            else if (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName) || String.IsNullOrEmpty(uMail) || String.IsNullOrEmpty(pWord))
            {
                //Make sure form is filled
                await DisplayAlert("Form Not Filled", "Please fill entire form", "OK");
            }
            else
            {
                //If Passwords match and form is filled, then create the user
                var user = new User { FirstName = firstName, LastName = lastName, Email = uMail, Password = pWord };

                //Enter user into database
                await _connection.InsertAsync(user);

                //Route back to login page
                await Navigation.PopModalAsync();
            }
        }
    }
}
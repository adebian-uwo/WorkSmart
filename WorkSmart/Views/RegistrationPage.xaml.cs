using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkSmart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();

            var connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            connection.CreateTableAsync<User>();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
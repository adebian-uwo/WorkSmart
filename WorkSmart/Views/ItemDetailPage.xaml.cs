using System.ComponentModel;
using WorkSmart.ViewModels;
using Xamarin.Forms;

namespace WorkSmart.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
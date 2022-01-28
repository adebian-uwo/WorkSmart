using System;
using System.Collections.Generic;
using System.ComponentModel;
using WorkSmart.Models;
using WorkSmart.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkSmart.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
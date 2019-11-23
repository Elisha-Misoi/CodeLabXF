using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeLabXF.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        ViewModels.MainPageViewModel viewModel = new ViewModels.MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.GetUsers();
        }

    }
}
using LSGS.Services;
using LSGS.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            string trial = "deneme";
            DependencyService.Register<MockDataStore>(); 
            MainPage = new AppShell();
            loginn();
        }

        protected override void OnStart()
        {
        }
        public async void loginn()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

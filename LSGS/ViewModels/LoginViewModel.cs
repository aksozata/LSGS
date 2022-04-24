using LSGS.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LSGS.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command CreateAccountCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            CreateAccountCommand = new Command(OnCreateAccountClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
        private async void OnCreateAccountClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(CreateAccountPage)}");
        }
    }
}

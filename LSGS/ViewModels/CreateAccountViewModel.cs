using LSGS.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LSGS.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel
    {
        public Command CancelCommand { get; }

        public CreateAccountViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
        private async void OnCancelClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

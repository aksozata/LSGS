using LSGS.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using LSGS.Models;

namespace LSGS.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel
    {
        public Command CancelCommand { get; }
        public Command RegisterCommand { get; }
        public Profile profile { get; set; }
        public string passwordConfirm { get; set; }
        public CreateAccountViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            profile = new Profile();
        }

        private async void OnRegisterClicked(object obj)
        {

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one

            if (passwordConfirm != profile.Password)
            {
                App.Current.MainPage.DisplayAlert("Error", "Passwords don't match", "OK");
                return;
            }
            //PASSWORD UPPER-CASE, SPECIAL CHARACTER CHECK WILL BE ADDED

            if (Globals.connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    Globals.connection.Close();
                    Globals.connection.Open();
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Error", "Database Connection Failure", "OK");
                    return;
                }
            }
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $@"INSERT INTO User (`METU_ID`, `Personal_Description`, `First_Name`, `Surname`, `Password`, `Email`) 
                  VALUES('{profile.METU_ID}', '{profile.Description}', '{profile.Name}', '{profile.Surname}', '{profile.Password}', '{profile.Email}');";

            try
            {
                var insert = await command.ExecuteNonQueryAsync();
                App.Current.MainPage.DisplayAlert("Success", "Account created!", "OK");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Check Values!", "OK");
            }
            finally
            {
                Globals.connection.Close();
            }        
        }
        private async void OnCancelClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

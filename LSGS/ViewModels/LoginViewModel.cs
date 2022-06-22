using LSGS.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using LSGS.Models;
using MySqlConnector;

namespace LSGS.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command CreateAccountCommand { get; }
        public Profile profile { get; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            CreateAccountCommand = new Command(OnCreateAccountClicked);
            profile = new Profile();

            try
            {
                if (Globals.connection == null || Globals.connection.State != System.Data.ConnectionState.Open)
                {
                    Globals.connection = new MySqlConnection(Globals.builder.ConnectionString);
                    Globals.connection.Open();

                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Database Connection Failure", "OK");
            }
        }

        private async void OnLoginClicked(object obj)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    Globals.connection.Close();
                    Globals.connection.Open();
                } catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Error", "Database Connection Failure", "OK");
                    return;
                }
            }
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM User WHERE METU_ID = '{profile.METU_ID}' AND Password = '{profile.Password}';";
            try
            {
                // execute the command and read the results
                var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    var Name = reader.GetString("First_Name");
                    var Surname = reader.GetString("Surname");
                    var Email = reader.GetString("Email");
                    var Password = reader.GetString("Password");
                    var Description = reader.GetString("Personal_Description");
                    var METU_ID = reader.GetInt32("METU_ID");
                    Globals.profile = new Profile(Name, Surname, METU_ID, Password, Email, Description);
                    Globals.connection.Close();

                    Globals.connection.Open();
                    // Get Reserved book list
                    command.CommandText = $"SELECT * FROM Reserve WHERE User_ID = '{profile.METU_ID}';";
                    var reader_ = await command.ExecuteReaderAsync();
                    if (reader_.Read())
                    {
                        Globals.profile.ReservedBookList.Add(reader_.GetInt32("Book_ID").ToString());
                    }
                    Globals.connection.Close();

                    // Get Reserved book list
                    Globals.connection.Open();
                    command.CommandText = $"SELECT * FROM Lend WHERE METU_ID = '{profile.METU_ID}';";
                    var reader__ = await command.ExecuteReaderAsync();
                    if (reader__.Read())
                    {
                        Globals.profile.LentBookList.Add(reader__.GetInt32("Book_ID").ToString());
                    }
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Error", "No such user exists!", "OK");
                }
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Login failed!", "OK");
            }
            finally
            {
                Globals.connection.Close();
            }

            
        }
        private async void OnCreateAccountClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(CreateAccountPage)}");
        }
    }
}

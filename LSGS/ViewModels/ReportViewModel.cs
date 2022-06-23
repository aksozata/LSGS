using System;
using System.Collections.Generic;
using System.Text;
using LSGS.Views;
using Xamarin.Forms;
using LSGS.Models;
using MySqlConnector;

namespace LSGS.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        public Command ReportCommand { get; }
        public string description { get; set; }
        ReportPage ReportPage;
        public ReportViewModel(ReportPage reportPage)
        {
            ReportCommand = new Command(OnReportClicked);
            ReportPage = reportPage;
        }
        private async void OnReportClicked(object obj)
        {
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
                $@"INSERT INTO Report (`Error_ID`, `Owner_ID`, `Description`, `Status`) 
                  VALUES(null, '{Globals.profile.METU_ID}', '{description}', 'Open');";

            try
            {
                var insert = await command.ExecuteNonQueryAsync();
                App.Current.MainPage.DisplayAlert("Success", "Error reported!", "OK");
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Error occured. Try again!", "OK");
            }
            finally
            {
                ReportPage.GoBack();
                Globals.connection.Close();
            }
        }
    }
}

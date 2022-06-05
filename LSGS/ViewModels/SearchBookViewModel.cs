using LSGS.Views;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LSGS.ViewModels
{
    public class SearchBookViewModel : BaseViewModel
    {
        public Command SearchBookCommand { get; }

        public SearchBookViewModel()
        {
            SearchBookCommand = new Command(OnSearchClicked);
        }

        private async void OnSearchClicked(object obj)
        {
            // set these values correctly for your database server
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "db4free.net",
                UserID = "is502grp3",
                Password = "metu2022is502",
                Database = "lsgsg3",
            };

            // open a connection asynchronously
            var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();

            // create a DB command and set the SQL statement with parameters
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Book;";

            // execute the command and read the results
            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var Serial_no = reader.GetInt32("Serial_no");
            }



            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(CreateAccountPage)}");
        }
    }
}

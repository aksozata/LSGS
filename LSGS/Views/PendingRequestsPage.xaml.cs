using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.ViewModels;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingRequestsPage : ContentPage
    {
        public static int METU_ID;
        public Command AddCommand { get; }
        public Command DeclineCommand { get; }
        public IList<Profile> finalList { get; set; }
        List<Profile> pendingRequestsList;
        public PendingRequestsPage()
        {
            InitializeComponent();
            
            AddCommand = new Command(AddClicked);
            DeclineCommand = new Command(DeclineClicked);
            BindingContext = this;
        }

        // rate recommend comment reserve

        public async void getRequests()
        {
            pendingRequestsList = new List<Profile>();
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText = $@"SELECT * FROM Friends WHERE 'Friend' = '{Globals.profile.METU_ID}' AND 'isAccepted' = 1";           

            // execute the command and read the results
            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var Name = reader.GetString("First_Name");
                var Surname = reader.GetString("Surname");
                var METU_ID = reader.GetInt32("METU_ID");
                var Email = reader.GetString("Email");
                var Description = reader.GetString("Personal_Description");
                var each_request = new Profile(Name, Surname, METU_ID, null, Email, Description);
                pendingRequestsList.Add(each_request);
            }
            finalList = pendingRequestsList;
            Globals.connection.Close();

        }
        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Profile selectedItem = e.CurrentSelection[0] as Profile;
            METU_ID = selectedItem.METU_ID;
        }

        private async void AddClicked(object obj)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();            
            command.CommandText =
                    $@"UPDATE Friends
                    SET 'isAccepted' = 1 
                    WHERE 'User' = '{METU_ID}' AND 'Friend' = '{Globals.profile.METU_ID}';";
                var insert = await command.ExecuteNonQueryAsync();
                Globals.connection.Close();
                App.Current.MainPage.DisplayAlert("Success", "Friend request accepted!", "OK");
        }
        private async void DeclineClicked(object obj)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();         
                command.CommandText =
                    $@"DELETE FROM Friend (`User`, `Friend`) 
                  VALUES('{METU_ID}', '{Globals.profile.METU_ID}');";
                var insert = await command.ExecuteNonQueryAsync();
                Globals.connection.Close();
        }
    }
}
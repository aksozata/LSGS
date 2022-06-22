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
        public IList<Profile> FinalList { get; set; }
        public static List<Profile> pendingRequestsList = new List<Profile>();
        public PendingRequestsPage()
        {
            InitializeComponent();
            
            AddCommand = new Command(AddClicked);
            DeclineCommand = new Command(DeclineClicked);           
            getRequests();
            BindingContext = this;
        }

        // rate recommend comment reserve

        public async void getRequests()
        {
            List<Profile> tempList = new List<Profile>();
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText = $@"SELECT * FROM Friends INNER JOIN User ON Friends.User = User.METU_ID WHERE Friends.Friend = '{Globals.profile.METU_ID}' AND Friends.isAccepted = 0;";           

            // execute the command and read the results
            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var Name = reader.GetString("First_Name");
                var Surname = reader.GetString("Surname");
                var METU_ID = reader.GetInt32("METU_ID");
                var Email = reader.GetString("Email");
                var Description = reader.GetString("Personal_Description");
                var each_request = new Profile(Name, Surname, METU_ID, "-1", Email, Description);
                tempList.Add(each_request);
            }
            pendingRequestsList = tempList;
            FinalList = pendingRequestsList;
            friendCollection.ItemsSource = FinalList;
            Globals.connection.Close();

        }
        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Profile selectedItem = e.CurrentSelection[0] as Profile;
            METU_ID = (int)selectedItem.METU_ID;
        }

        private async void AddClicked(object obj)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();            
            command.CommandText =
                    $@"UPDATE Friends
                    SET isAccepted = '1' 
                    WHERE User = '{METU_ID}' AND Friend = '{Globals.profile.METU_ID}';";
                var insert = await command.ExecuteNonQueryAsync();
                Globals.connection.Close();
                App.Current.MainPage.DisplayAlert("Success", "Friend request accepted!", "OK");
            await Shell.Current.GoToAsync($"//{nameof(ProfilePage)}");
        }
        private async void DeclineClicked(object obj)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();         
                command.CommandText =
                    $@"DELETE FROM Friends WHERE User = '{METU_ID}' AND Friend = '{Globals.profile.METU_ID}';";
                var insert = await command.ExecuteNonQueryAsync();
                Globals.connection.Close();
            App.Current.MainPage.DisplayAlert("Success", "Friend request declined!", "OK");
            await Shell.Current.GoToAsync("ProfilePage");
        }
    }
}
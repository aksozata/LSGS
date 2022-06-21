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
    public partial class FriendSearchResultsPage : ContentPage
    {
        public static int METU_ID;
        public Command AddCommand { get; }
        public Command RemoveCommand { get; }
        public IList<Profile> SearchList { get; set; }
        public CollectionView lastItem = null;
        public Color color = Color.Default;
        public FriendSearchResultsPage()
        {
            InitializeComponent();
            SearchList = SearchFriendViewModel.FriendSearchResultsList;
            AddCommand = new Command(AddClicked);
            RemoveCommand = new Command(RemoveClicked);
            BindingContext = this;
        }

        // rate recommend comment reserve


        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Profile selectedItem = e.CurrentSelection[0] as Profile;
            METU_ID = (int)selectedItem.METU_ID;
        }

        private async void AddClicked(object obj)
        {
            if (METU_ID == Globals.profile.METU_ID)
            {
                App.Current.MainPage.DisplayAlert("Error", "You can't add yourself as friend!", "OK");
                return;
            }
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $@"SELECT * FROM Friends WHERE Friends.User = '{Globals.profile.METU_ID}' AND Friends.Friend = '{METU_ID}'";
            var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                App.Current.MainPage.DisplayAlert("Friend", "This user is already added as friend!", "OK");
                Globals.connection.Close();
            }
            else
            {
                Globals.connection.Close();
                Globals.connection.Open();
                command.CommandText =
                    $@"INSERT INTO Friends (`User`, `Friend`) 
                  VALUES('{Globals.profile.METU_ID}', '{METU_ID}');";
                var insert = await command.ExecuteNonQueryAsync();
                Globals.connection.Close();
                App.Current.MainPage.DisplayAlert("Success", "Friend request is sent!", "OK");
            }
        }
        private async void RemoveClicked(object obj)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $@"SELECT * FROM Friend WHERE User = '{Globals.profile.METU_ID}' AND Friend = '{METU_ID}";
            var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                command.CommandText =
                    $@"DELETE FROM Friend (`User`, `Friend`) 
                  VALUES('{Globals.profile.METU_ID}', '{METU_ID}');";
                var insert = await command.ExecuteNonQueryAsync();
                Globals.connection.Close();
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Friend", "This user is not currently a friend", "OK");
                Globals.connection.Close();

            }
        }
    }
}
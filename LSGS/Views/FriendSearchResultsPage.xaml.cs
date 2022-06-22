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
            AddButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;
        }

        // rate recommend comment reserve


        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Profile selectedItem = e.CurrentSelection[0] as Profile;
            METU_ID = (int)selectedItem.METU_ID;

            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $@"SELECT * FROM Friends WHERE ((User = '{Globals.profile.METU_ID}' and Friend = '{METU_ID}') OR (Friend = '{Globals.profile.METU_ID}' and User = '{METU_ID}'))";
            try
            {
                var reader = await command.ExecuteReaderAsync();
                if(reader.Read())
                {
                    AddButton.IsEnabled = false;
                    RemoveButton.IsEnabled = true;
                }
                else
                {
                    AddButton.IsEnabled = true;
                    RemoveButton.IsEnabled = false;
                }
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Operation failed.", "OK");
            }
            finally
            {
                Globals.connection.Close();
            }
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
            command.CommandText = $@"INSERT INTO Friends (`User`, `Friend`) 
                  VALUES('{Globals.profile.METU_ID}', '{METU_ID}');";
            try
            {
                var insert = await command.ExecuteNonQueryAsync();
                App.Current.MainPage.DisplayAlert("Success", "Friend request is sent!", "OK");
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Friend request is not sent! ", "OK");
            }
            finally { Globals.connection.Close(); }
        }
        private async void RemoveClicked(object obj)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText = $@"DELETE FROM Friends Where (User ='{Globals.profile.METU_ID}' and Friend = '{METU_ID}' ) OR (Friend = '{Globals.profile.METU_ID}' and User = '{METU_ID}');";
            try
            {
                var insert = await command.ExecuteNonQueryAsync();
                App.Current.MainPage.DisplayAlert("Success", "Deletion is successful!", "OK");

            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Operation failed!", "OK");
            }
            finally
            {
                Globals.connection.Close();
            }
        }
    }
}
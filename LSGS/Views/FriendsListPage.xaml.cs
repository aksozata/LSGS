using LSGS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsListPage : ContentPage
    {
        public ObservableCollection<Profile> friendsList { get; set; }
        Profile selectedProfile = new Profile();
        public Command DeclineCommand { get; }
        public static bool IsRecommended = false;
        public FriendsListPage()
        {
            InitializeComponent();
            friendsList = new ObservableCollection<Profile>();
            CreateFriendsList();
            DeclineCommand = new Command(RemoveClicked);
            if(IsRecommended)
            {
                RemoveButton.Text = "Recommend";
                RemoveButton.BackgroundColor = Color.CadetBlue;
            }
            BindingContext = this;
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProfile = e.CurrentSelection[0] as Profile;            
        }

        private async void RemoveClicked(object obj)
        {
            if (selectedProfile.METU_ID == 0)
                return;
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
         
            if(IsRecommended)
            {
                command.CommandText = $@"INSERT INTO Recommend (`Recommender`, `Recommendee`, `Book_ID`) 
                  VALUES('{Globals.profile.METU_ID}', '{selectedProfile.METU_ID}', '{BookSearchResultsPage.BookSerialNo}');";
                try
                {
                    var insert = await command.ExecuteNonQueryAsync();
                    App.Current.MainPage.DisplayAlert("Success", $"Book is recommended to {selectedProfile.Name}!", "OK");
                }
                catch(Exception ex) 
                { 
                    if(ex.Message.Contains("Duplicate"))
                        App.Current.MainPage.DisplayAlert("Error", $"Book is already recommended to {selectedProfile.Name}!", "OK");

                }
                finally { Globals.connection.Close(); }
            }
            else
            {
                command.CommandText = $@"DELETE FROM Friends where (isAccepted=1 and User = {Globals.profile.METU_ID} and Friend = {selectedProfile.METU_ID});";
                try
                {
                    var insert = await command.ExecuteNonQueryAsync();
                    App.Current.MainPage.DisplayAlert("Success", "Friend is removed!", "OK");
                    friendsList.Remove(selectedProfile);
                }
                catch(Exception ex) 
                {
                    App.Current.MainPage.DisplayAlert("Error", "Friend cannot be removed.", "OK");
                }
                finally { Globals.connection.Close(); }
            }
            //await Shell.Current.GoToAsync("ProfilePage");
        }

        private async void CreateFriendsList()
        {
            friendsList.Clear();
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText = $"SELECT * FROM User Where METU_ID in (SELECT Friend From Friends Where (User = {Globals.profile.METU_ID} and isAccepted=1)) " +
                $"UNION SELECT * FROM User Where METU_ID in (SELECT User From Friends Where(Friend = {Globals.profile.METU_ID} and isAccepted = 1)); ";
            try
            {
                var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var Name = reader.GetString("First_Name");
                    var Surname = reader.GetString("Surname");
                    var Email = reader.GetString("Email");
                    var Password = reader.GetString("Password");
                    var Description = reader.GetString("Personal_Description");
                    var METU_ID = reader.GetInt32("METU_ID");
                    friendsList.Add(new Profile(Name, Surname, METU_ID, Password, Email, Description));
                }
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Friends list cannot be shown right now!", "OK");
                await Navigation.PushAsync(new ProfilePage());
            }
            finally
            {
                Globals.connection.Close();
            }
        }
    }
}
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
    public partial class StudyGroupPage : ContentPage
    {
        public Group GroupInformation = new Group();
        public ObservableCollection<Profile> ParticipantList = new ObservableCollection<Profile>();
        public StudyGroupBindingClass bindingInstance;
        public StudyGroupPage()
        {
            InitializeComponent();
            GroupInformation = MyStudyGroups.SelectedGroup;
            GetParticipantList();
            string ownerName = GetOwnerName();
            bindingInstance = new StudyGroupBindingClass(GroupInformation.Name, GroupInformation.Category, GroupInformation.Description, ownerName, ParticipantList);
            BindingContext = bindingInstance;
        }

        private string GetOwnerName()
        {
            string ownerName = "";
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            var command = Globals.connection.CreateCommand();
            command.CommandText = $@"SELECT * FROM User WHERE METU_ID in (SELECT OwnerID from StudyGroup WHERE ID = '{GroupInformation.GroupId}')";
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var firstName = reader.GetString("First_Name");
                    var surname = reader.GetString("Surname");
                    var email = reader.GetString("Email");
                    int metuId = reader.GetInt32("METU_ID");
                    if(metuId == Globals.profile.METU_ID)
                    {
                        JoinLeaveButton.Text = "Delete";
                        JoinLeaveButton.BackgroundColor = Color.Red;
                    }
                    ownerName = $"{reader.GetString("First_Name")} {reader.GetString("Surname")}";
                    ParticipantList.Add(new Profile(firstName, surname, email));
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                Globals.connection.Close();
            }
            return ownerName;
        }

        private void GetParticipantList()
        {
            ObservableCollection<Profile> participantList = new ObservableCollection<Profile>();
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            var command = Globals.connection.CreateCommand();
            command.CommandText = $@"SELECT * FROM User WHERE METU_ID in (SELECT METU_ID from ParticipateIn WHERE Group_ID = '{GroupInformation.GroupId}')";
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var email = reader.GetString("Email");
                    var firstName = reader.GetString("First_Name");
                    var surname = reader.GetString("Surname");
                    int metuId = reader.GetInt32("METU_ID");
                    if (metuId == Globals.profile.METU_ID)
                    {
                        JoinLeaveButton.Text = "Leave";
                        JoinLeaveButton.BackgroundColor = Color.Firebrick;
                    }
                    participantList.Add(new Profile(firstName, surname, email));
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                Globals.connection.Close();
            }
            ParticipantList = participantList;
        }

        public class StudyGroupBindingClass
        {
            public StudyGroupBindingClass(string name, string category, string description, string ownerName, ObservableCollection<Profile> participantsList)
            {
                Name = name;
                Category = category;
                Description = description;
                OwnerName = ownerName;
                ParticipantList = participantsList;
            }

            public string Name { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public string OwnerName { get; set; }
            public ObservableCollection<Profile> ParticipantList { get; set; }
        }

        private async void JoinLeaveButton_Clicked(object sender, EventArgs e)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            switch (JoinLeaveButton.Text)
            {
                case "Join":
                    {
                        command.CommandText = $@"INSERT INTO ParticipateIn (METU_ID, Group_ID) VALUES ('{Globals.profile.METU_ID}', '{GroupInformation.GroupId}');";
                    }
                    break;
                case "Leave":
                    {
                        command.CommandText = $@"DELETE FROM ParticipateIn WHERE METU_ID = '{Globals.profile.METU_ID}' AND Group_ID = '{GroupInformation.GroupId}';";
                    }
                    break;
                case "Delete":
                    {
                        command.CommandText = $@"DELETE FROM ParticipateIn WHERE Group_ID = '{GroupInformation.GroupId}';";
                    }
                    break;
            }
            try
            {
                var insert = await command.ExecuteNonQueryAsync();
                App.Current.MainPage.DisplayAlert("Success", "Action completed!", "OK");
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Action cannot be completed!", "OK");
            }
            finally
            {
                Globals.connection.Close();
            }
            if(JoinLeaveButton.Text == "Delete")
            {
                if (Globals.connection.State != System.Data.ConnectionState.Open)
                    Globals.connection.Open();
                command.CommandText = $@"DELETE FROM StudyGroup WHERE ID = '{GroupInformation.GroupId}';";
                try
                {
                    var insert = await command.ExecuteNonQueryAsync();
                    await Navigation.PopAsync();
                }
                catch
                {
                    App.Current.MainPage.DisplayAlert("Error", "Action cannot be completed!", "OK");
                }
                finally
                {
                    Globals.connection.Close();
                }
            }
            else if(JoinLeaveButton.Text == "Join")
            {
                JoinLeaveButton.Text = "Leave";
                JoinLeaveButton.BackgroundColor = Color.Firebrick;
                bindingInstance.ParticipantList.Add(Globals.profile);

            }
            else if(JoinLeaveButton.Text == "Leave")
            {
                JoinLeaveButton.Text = "Join";
                JoinLeaveButton.BackgroundColor = Color.CadetBlue;
                bindingInstance.ParticipantList.Remove(Globals.profile);
            }
        }

        private void participants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
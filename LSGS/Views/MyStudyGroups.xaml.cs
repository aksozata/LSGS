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
    public partial class MyStudyGroups : ContentPage
    {
        public static ObservableCollection<Group> StudyGroupList { get; set; }
        public static Group SelectedGroup { get; set; }
        public MyStudyGroups()
        {
            StudyGroupList = new ObservableCollection<Group>();
            InitializeComponent();
            GetStudyGroupList();
            friendCollection.ItemsSource = StudyGroupList;
        }

        private void GetStudyGroupList()
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $@"select * from StudyGroup where (ID in (select Group_ID from ParticipateIn where METU_ID = '{Globals.profile.METU_ID}')) or OwnerID = '{Globals.profile.METU_ID}';";
            try
            {// execute the command and read the results
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var name = reader.GetString("Name");
                    var category = reader.GetString("Category");
                    var group_ID = reader.GetInt32("ID");
                    var owner_ID = reader.GetInt32("OwnerID");
                    var description = reader.GetString("Description");
                    StudyGroupList.Add(new Group(name, category, description, group_ID, owner_ID));
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                Globals.connection.Close();
            }
        }

        private async void Exit_Clicked(object sender, EventArgs e)
        {
            if(SelectedGroup == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Please select a group!", "OK");
                return;
            }
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            switch (Exit.Text)
            {
                case "Exit":
                    {
                        command.CommandText = $@"DELETE FROM ParticipateIn WHERE Group_ID = '{SelectedGroup.GroupId} AND METU_ID = '{Globals.profile.METU_ID}''";
                    }
                    break;
                case "Delete":
                    {
                        command.CommandText = $@"DELETE FROM StudyGroup WHERE Group_ID = '{SelectedGroup.GroupId}'";
                    }
                    break;
            }
            try
            {
                var insert_ = await command.ExecuteNonQueryAsync();
                App.Current.MainPage.DisplayAlert("Success", "Action completed!", "OK");
                StudyGroupList.Remove(SelectedGroup);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Action cannot be completed.", "OK");
            }
            finally { Globals.connection.Close(); }
        }

        private void friendCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedGroup = e.CurrentSelection[0] as Group;
            if(SelectedGroup.OwnerID == Globals.profile.METU_ID)
            {
                Exit.Text = "Delete";
            }
            else
            {
                Exit.Text = "Exit";
            }
        }

        private async void Open_Clicked(object sender, EventArgs e)
        {
            if(SelectedGroup == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Please select a group!", "OK");
                return;
            }
            await Navigation.PushAsync(new StudyGroupPage());
        }
    }
}
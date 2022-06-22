using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LSGS.ViewModels;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchCreateGroupPage : ContentPage
    {
        public IList pickerList;
        public static IList searchResults;
        public Command SearchGroupCommand { get; }
        public Command CreateGroupCommand { get; }
        public Group searchedGroup { get; set; } = new Group();
        public SearchCreateGroupPage()
        {
            InitializeComponent();
            SearchGroupCommand = new Command(OnSearchClicked);
            CreateGroupCommand = new Command(OnCreateClicked);
            List<string> tempList = new List<string>();
            tempList.Add("Math"); tempList.Add("Programming"); tempList.Add("Physics"); tempList.Add("Chemistry"); tempList.Add("History"); tempList.Add("Biology"); tempList.Add("Law"); tempList.Add("Psychology");
            pickerList = tempList;
            picker1.ItemsSource = pickerList;
            BindingContext = this;
        }

        private async void OnCreateClicked(object obj)
        {
            if(searchedGroup.Name == null || searchedGroup.Description == null || picker1.SelectedIndex < 0 )
            {
                App.Current.MainPage.DisplayAlert("Warning", "Please fill in all areas!", "OK");
                return;
            }
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();        
                command.CommandText =
                    $@"INSERT INTO StudyGroup (`OwnerID`, `Name`, `Description`, `Category`) 
                  VALUES('{Globals.profile.METU_ID}', '{searchedGroup.Name}', '{searchedGroup.Description}', '{picker1.Items[picker1.SelectedIndex]}');";
            try
            {
                var insert = await command.ExecuteNonQueryAsync();
                App.Current.MainPage.DisplayAlert("Success", "Study group is created!", "OK");
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Study group is not created!", "OK");
            }
            finally { Globals.connection.Close(); }      
        }

        private async void OnSearchClicked(object obj)
        {
            List<Group> search_result_list = new List<Group>();
            bool andChecker = false;
            bool shouldThereBeWhere = false;
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $@"SELECT * FROM StudyGroup WHERE ";
            if (searchedGroup.Name != null && searchedGroup.Name != "")
            {
                command.CommandText += $"Name LIKE('{searchedGroup.Name}')";
                andChecker = true;
                shouldThereBeWhere = true;
            }

            if (picker1.SelectedIndex >= 0 && picker1.Items[picker1.SelectedIndex] != null && picker1.Items[picker1.SelectedIndex] != "")
            {
                if (andChecker)
                {
                    command.CommandText += " AND ";
                }
                command.CommandText += $"Category LIKE('{picker1.Items[picker1.SelectedIndex]}')";
                andChecker = true;
                shouldThereBeWhere = true;
            }
            if (searchedGroup.Description != null && searchedGroup.Description != "")
            {
                if (andChecker)
                {
                    command.CommandText += " AND ";
                }
                command.CommandText += $"Description LIKE ('{searchedGroup.Description}')";
                andChecker = true;
                shouldThereBeWhere = true;
            }
            if(!shouldThereBeWhere)
            {
                command.CommandText = $@"SELECT * FROM StudyGroup";
            }
            command.CommandText += ";";

            // execute the command and read the results
            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var Name = reader.GetString("Name");
                var Category = reader.GetString("Category");
                var Description = reader.GetString("Description");
                var Owner = reader.GetInt32("OwnerID");
                var each_group = new Group(Name, Category, Description, Owner);
                search_result_list.Add(each_group);
            }
            Globals.connection.Close();
            searchResults = search_result_list;
            await Shell.Current.GoToAsync("GroupSearchResultsPage");
        }
    }
}
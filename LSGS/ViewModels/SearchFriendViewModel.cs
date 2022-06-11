﻿using System;
using System.Windows.Input;
using Xamarin.Essentials;
using System.Collections.Generic;
using MySqlConnector;
using Xamarin.Forms;
using LSGS.Models;

namespace LSGS.ViewModels
{
    public class SearchFriendViewModel : BaseViewModel
    {
        public static List<Profile> FriendSearchResultsList = new List<Profile>();
        public Command SearchFriendCommand { get; }
        public Profile searchedFriend { get; set; } = new Profile();
        public SearchFriendViewModel()
        {
            SearchFriendCommand = new Command(OnSearchClicked);
        }

        private async void OnSearchClicked(object obj)
        {
            List<Profile> search_result_list = new List<Profile>();
            bool andChecker = false;
            if(Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $@"SELECT * FROM User WHERE ";
            if (searchedFriend.Name != null)
            {
                command.CommandText += $"First_Name LIKE('%{searchedFriend.Name}%')";
                andChecker = true;
            }

            if (searchedFriend.Surname != null)
            {
                if (andChecker)
                {
                    command.CommandText += " AND ";
                }
                command.CommandText += $"Surname LIKE('%{searchedFriend.Surname}%')";
                andChecker = true;
            }
            if(searchedFriend.Surname != null)
            {
                if (andChecker)
                {
                    command.CommandText += " AND ";
                }
                command.CommandText += $"METU_ID LIKE ('%{searchedFriend.METU_ID}%')";
                andChecker = true;
            }
            if (searchedFriend.Surname != null)
            {
                if (andChecker)
                {
                    command.CommandText += " AND ";
                }
                command.CommandText += $"Email LIKE ('%{searchedFriend.Email}%')";
                andChecker = true;
            }
            command.CommandText += ";";

            // execute the command and read the results
            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var Name = reader.GetString("First_Name");
                var Surname = reader.GetString("Surname");
                var METU_ID = reader.GetInt32("METU_ID");
                var Email = reader.GetString("Email");
                var Description = reader.GetString("Personal_Description");
                var each_friend = new Profile(Name, Surname, METU_ID, null, Email, Description);
                search_result_list.Add(each_friend);
            }
            Globals.connection.Close();
            FriendSearchResultsList = search_result_list;
            await Shell.Current.GoToAsync("FriendSearchResultsPage");
        }
    }
}
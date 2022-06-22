using LSGS.Views;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using LSGS.Models;
using System.Collections.ObjectModel;

namespace LSGS.ViewModels
{
    public class SearchBookViewModel : BaseViewModel
    {
        public static ObservableCollection<Book> BookSearchResultsList = new ObservableCollection<Book>();
        public Command SearchBookCommand { get; }
        public Book searchedBook { get; set; } = new Book();
        public INavigation Navigation;
        public SearchBookViewModel(INavigation navigation)
        {
            SearchBookCommand = new Command(OnSearchClicked);
            Navigation = navigation;
        }

        private async void OnSearchClicked(object obj)
        {
            ObservableCollection<Book> search_result_list = new ObservableCollection<Book>();
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
            command.CommandText =
                $@"SELECT *
                  FROM Book
                  WHERE Title LIKE ('%{searchedBook.Name}%')
                    AND
                  Author LIKE ('%{searchedBook.Author}%')
                    AND
                  Year LIKE ('%{searchedBook.PublishYear}%')
                    AND
                  Publisher LIKE ('%{searchedBook.Publisher}%')
                  ;";
            // execute the command and read the results
            try
            {
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var Book_name = reader.GetString("Title");
                    var Publisher = reader.GetString("Publisher");
                    var Author_name = reader.GetString("Author");
                    var Published_year = reader.GetInt32("Year");
                    var Serial_no = reader.GetInt32("Serial_no");
                    var imageUrl = reader.GetString("IMG_URL");
                    var each_book = new Book(Book_name, Author_name, Publisher, (Published_year).ToString(), imageUrl, Serial_no.ToString());
                    search_result_list.Add(each_book);
                    BookSearchResultsList = search_result_list;
                }
            }
            catch
            {
                Application.Current.MainPage.DisplayAlert("Error", "Search failed!", "OK");
            }
            finally
            {
                connection.Close();
            }

            //await Shell.Current.GoToAsync("//BookSearchResultsPage");
            await Navigation.PushAsync(new BookSearchResultsPage());

        }
    }
}

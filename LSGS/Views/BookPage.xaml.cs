using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookPage : ContentPage
    {
        public Book BookInformation = new Book();
        public BookPage()
        {
            BookInformation = ViewModels.SearchBookViewModel.BookSearchResultsList.Find(book => book.SerialNo == BookSearchResultsPage.BookSerialNo);
            InitializeComponent();
            List<Comment> comment_result_list = new List<Comment>();

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
                  FROM Comment
                  LEFT JOIN User ON User.METU_ID = Comment.User_ID
                  WHERE Book_ID = {BookInformation.SerialNo}
                  ;";
            // execute the command and read the results
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var comment = reader.GetString("User_Comment");
                var rating = reader.GetInt32("Rating");
                var first_name = reader.GetString("First_Name");
                var surname = reader.GetString("Surname");
                var each_book = new Comment(BookInformation.SerialNo, Globals.profile.METU_ID.ToString(), first_name+" "+surname , comment, (rating).ToString());
                comment_result_list.Add(each_book);
            }
            connection.Close();
            BindingContext = this.BookInformation;
        }

        private async void Rate_Button_Clicked(object sender, EventArgs e)
        {
            RatingPage.ratedBook = BookInformation;
            await Navigation.PushAsync(new RatingPage());
            //await Shell.Current.GoToAsync("//RatingPage");
        }

        private void Recommend_Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Reserve_Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
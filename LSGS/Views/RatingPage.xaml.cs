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
    public partial class RatingPage : ContentPage
    {
        public static Book ratedBook = new Book();
        public RatingPage()
        {
            InitializeComponent();
            BindingContext = ratedBook;
        }

        async private void GoBack_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async private void Post_Button_Clicked(object sender, EventArgs e)
        {
            if (bookRating.SelectedIndex == -1)
                return;
            string rating = bookRating.Items[bookRating.SelectedIndex];
            string comment = bookComment.Text;
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "db4free.net",
                UserID = "is502grp3",
                Password = "metu2022is502",
                Database = "lsgsg3",
            };

            // open a connection asynchronously
            var connection = new MySqlConnection(builder.ConnectionString);
            try
            {
                connection.Open();
                // create a DB command and set the SQL statement with parameters
                var command = connection.CreateCommand();

                // TO-DO            --- Book_ID	User_ID	User_Comment	Rating
                command.CommandText = "insert into Comment(Book_ID,User_ID,User_Comment,Rating)" +
                    "values('" + ratedBook.SerialNo + "','" + Globals.profile.METU_ID + "','" + comment + "','" + Int32.Parse(rating) + "')" +
                    " " + "ON DUPLICATE KEY UPDATE User_Comment='" + comment + "'," + "Rating=" + Int32.Parse(rating) + ";";
                // execute the command and read the results
                await command.ExecuteReaderAsync();
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Error posting the review!", "OK");
            }
            finally
            {
                connection.Close();            
            }
            App.Current.MainPage.DisplayAlert("Congratulations", "Review is posted!", "OK");
            await Navigation.PushAsync(new BookPage());
        }
    }
}
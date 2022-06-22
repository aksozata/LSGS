using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;
using System.ComponentModel;
using LSGS.ViewModels;
using System.Threading;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookPage : ContentPage
    {
        public Book BookInformation = new Book();
        public List<Comment> CommentList = new List<Comment>();
        public BindingClass bindingModel;

        public BookPage()
        {
            if(SearchBookViewModel.BookSearchResultsList.Count > 0)
                BookInformation = SearchBookViewModel.BookSearchResultsList.Where(book => book.SerialNo == BookSearchResultsPage.BookSerialNo).FirstOrDefault();
            else
            {
                GetBook();
            }
            bool isNull = true;
            while(isNull)
            {
                lock(BookInformation)
                {
                    if (BookInformation.Name != null)
                    {
                        isNull = false;
                        break;
                    }
                }
                Thread.Sleep(200);
            }
            InitializeComponent();
            if(Globals.profile.ReservedBookList.Contains(BookInformation.SerialNo))
            {
                ReserveButton.BackgroundColor = Color.Blue;
                ReserveButton.Text = "Pick up";
            }
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
            try
            {
                // execute the command and read the results
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var comment = reader.GetString("User_Comment");
                    var rating = reader.GetInt32("Rating");
                    var first_name = reader.GetString("First_Name");
                    var surname = reader.GetString("Surname");
                    var each_book = new Comment(BookInformation.SerialNo, Globals.profile.METU_ID.ToString(), first_name + " " + surname, comment, rating.ToString() + "/5");
                    comment_result_list.Add(each_book);
                }
            }
            catch 
            {
                Application.Current.MainPage.DisplayAlert("Error", "Loading book failed", "OK"); 
            }
            finally 
            {
                connection.Close();
            }
            CommentList = comment_result_list;
            bindingModel = new BindingClass(BookInformation.Name, BookInformation.Author, BookInformation.Publisher, BookInformation.PublishYear, BookInformation.ImageUrl, BookInformation.SerialNo, CommentList);
            BindingContext = bindingModel;
            ChangeButton();
        }

        private async void GetBook()
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    Globals.connection.Close();
                    Globals.connection.Open();
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Error", "Database Connection Failure", "OK");
                    return;
                }
            }
            // Get the book
            var newCommand = Globals.connection.CreateCommand();
            newCommand.CommandText = $@"SELECT * FROM Book WHERE Serial_No = '{BookSearchResultsPage.BookSerialNo}';";
            try
            {
                var reader = await newCommand.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var name = reader.GetString("Title");
                    var serialNo = reader.GetUInt32("Serial_No");
                    var author = reader.GetString("Author");
                    var imageUrl = reader.GetString("IMG_URL");
                    var publishYear = reader.GetUInt32("Year");
                    var publisher = reader.GetString("Publisher");
                    lock(BookInformation)
                    {
                        BookInformation = new Book(name, author, publisher, publishYear.ToString(), serialNo.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Database Connection Failure", "OK");
                await Navigation.PopAsync();
            }
            finally
            {
                Globals.connection.Close();
            }
        }

        private async void ChangeButton()
        {
            // Arrange the reserve button
            if (Globals.connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    Globals.connection.Close();
                    Globals.connection.Open();
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Error", "Database Connection Failure", "OK");
                    return;
                }
            }
            var command = Globals.connection.CreateCommand();

            // Check if the book is reserved
            command.CommandText = $"SELECT * FROM Reserve WHERE Book_ID = '{BookInformation.SerialNo}';";
            try
            {
                var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    if (reader.GetInt32("User_ID") == Globals.profile.METU_ID)
                    {
                        ReserveButton.BackgroundColor = Color.Blue;
                        ReserveButton.Text = "Pick up";
                    }
                    else
                    {
                        ReserveButton.BackgroundColor = Color.Gray;
                        ReserveButton.Text = "Unavailable";
                        ReserveButton.IsEnabled = false;
                        ReserveButton.FontSize = 10;
                    }
                    return;
                }
            }
            catch { }
            finally
            {
                Globals.connection.Close();
            }          

            // Check if the book is picked up
            Globals.connection.Open();
            command.CommandText = $"SELECT * FROM Lend WHERE Book_ID = '{BookInformation.SerialNo}';";
            try
            {
                var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    if (reader.GetInt32("METU_ID") == Globals.profile.METU_ID)
                    {
                        ReserveButton.BackgroundColor = Color.Red;
                        ReserveButton.Text = "Return";
                    }
                    else
                    {
                        ReserveButton.BackgroundColor = Color.Gray;
                        ReserveButton.Text = "Unavailable";
                        ReserveButton.IsEnabled = false;
                        ReserveButton.FontSize = 10;
                    }
                }
            }
            catch { }
            finally { Globals.connection.Close(); }            
        }

        private async void Rate_Button_Clicked(object sender, EventArgs e)
        {
            RatingPage.ratedBook = BookInformation;
            await Navigation.PushAsync(new RatingPage());
            //await Shell.Current.GoToAsync("//RatingPage");
        }

        private async void Recommend_Button_Clicked(object sender, EventArgs e)
        {
            FriendsListPage.IsRecommended = true;
            await Navigation.PushAsync(new FriendsListPage());
        }

        private async void Reserve_Button_Clicked(object sender, EventArgs e)
        {
            if (Globals.connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    Globals.connection.Close();
                    Globals.connection.Open();
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Error", "Database Connection Failure", "OK");
                    return;
                }
            }
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            var command = Globals.connection.CreateCommand();
            switch (ReserveButton.Text)
            {
                case "Reserve":
                    {
                        Random rnd = new Random();
                        command.CommandText =
                            $@"INSERT INTO Reserve (`Book_ID`, `User_ID`, `Password`) 
                  VALUES('{BookInformation.SerialNo}', '{Globals.profile.METU_ID}', '{rnd.Next(1, 99999)}');";

                        try
                        {
                            var insert = await command.ExecuteNonQueryAsync();
                            App.Current.MainPage.DisplayAlert("Success", "Book reserved!", "OK");
                            Globals.profile.ReservedBookList.Add(BookInformation.SerialNo);
                            ReserveButton.BackgroundColor = Color.Blue;
                            ReserveButton.Text = "Pick up";
                        }
                        catch (Exception ex)
                        {
                            App.Current.MainPage.DisplayAlert("Error", "Error reserving the book!", "OK");
                        }
                        finally
                        {
                            Globals.connection.Close();
                        }
                    }
                    break;
                case "Pick up":
                    {
                        // Get password from RESERVE table
                        int password = 0;
                        command.CommandText = $"SELECT * FROM Reserve WHERE Book_ID = '{BookInformation.SerialNo}';";
                        try
                        {
                            var reader = await command.ExecuteReaderAsync();
                            if (reader.Read())
                            {
                                password = reader.GetInt32("Password");
                            }
                        }
                        catch { }                        
                        finally { Globals.connection.Close(); }                       

                        // Add to LEND table and user lent book list
                        Globals.connection.Open();
                        var newCommand = Globals.connection.CreateCommand();
                        newCommand.CommandText = "insert into Lend(METU_ID,Book_ID,Password)" +
     " values('" + Globals.profile.METU_ID + "','" + BookInformation.SerialNo + "','" + password + "')" + ";";
                        try
                        {
                            var insert_ = await newCommand.ExecuteReaderAsync();
                            App.Current.MainPage.DisplayAlert("Success", $"Book will be ready to pick up. Password: {password}. Please give this password to the librarian.", "OK");
                            Globals.profile.LentBookList.Add(BookInformation.SerialNo);
                            ReserveButton.BackgroundColor = Color.Firebrick;
                            ReserveButton.Text = "Return";
                        }
                        catch (Exception ex)
                        {
                            App.Current.MainPage.DisplayAlert("Error", "Error picking up the book! Try again later", "OK");
                        }
                        finally
                        {
                            Globals.connection.Close();
                        }

                        // Delete from user reserved book list
                        Globals.profile.ReservedBookList.Remove(BookInformation.SerialNo);
                    }
                    break;
                case "Return":
                    {
                        // Get password from LEND table
                        command.CommandText = $"SELECT * FROM Lend WHERE Book_ID = '{BookInformation.SerialNo}';";
                        int password = 0;
                        try
                        {
                            var reader = await command.ExecuteReaderAsync();
                            if (reader.Read())
                            {
                                password = reader.GetInt32("Password");
                            }
                        }
                        catch { }
                        finally { Globals.connection.Close(); }                      

                        // Delete from LEND table and user lent book list
                        Globals.connection.Open();
                        command.CommandText = $@"DELETE FROM Lend WHERE Book_ID = '{BookInformation.SerialNo}';";

                        try
                        {
                            var insert_ = await command.ExecuteNonQueryAsync();
                            App.Current.MainPage.DisplayAlert("Success", $"You can return the book. Password: {password}. Please give this password to the librarian.", "OK");
                            Globals.profile.LentBookList.Remove(BookInformation.SerialNo);
                            ReserveButton.BackgroundColor = Color.Green;
                            ReserveButton.Text = "Reserve";
                        }
                        catch (Exception ex)
                        {
                            App.Current.MainPage.DisplayAlert("Error", "Error returning the book! Try again later", "OK");
                        }
                        finally
                        {
                            Globals.connection.Close();
                        }
                    }
                    break;
            }
        }
    }

    public class BindingClass
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string PublishYear { get; set; }
        public string ImageUrl { get; set; }
        public string SerialNo { get; set; }
        public List<Comment> CommentList { get; set; }

        public BindingClass(string name, string author, string publisher, string publishYear, string imageUrl, string serialNo, List<Comment> commentList)
        {
            Name = name;
            Author = author;
            Publisher = publisher;
            PublishYear = publishYear;
            ImageUrl = imageUrl;
            SerialNo = serialNo;
            CommentList = commentList;
        }
    }
}
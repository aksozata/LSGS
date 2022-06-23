using LSGS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyRecommendedBooksPage : ContentPage
    {
        public ObservableCollection<RecommendedBookInfo> MyRecommendedBooksList { get; set; }
        public MyRecommendedBooksPage()
        {
            InitializeComponent();
            MyRecommendedBooksList = new ObservableCollection<RecommendedBookInfo>();
            GetRecommendedBookList();
            BindingContext = this;
            //Thread.Sleep(10000);            
        }

        private async void GetRecommendedBookList()
        {
            MyRecommendedBooksList.Clear();
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
            var command = Globals.connection.CreateCommand();
            command.CommandText = $@"SELECT * FROM
(SELECT * FROM User WHERE METU_ID in (SELECT Recommender FROM Recommend WHERE '{Globals.profile.METU_ID}' = Recommendee)) a
CROSS JOIN
(SELECT * FROM Book WHERE Serial_No in (SELECT Book_ID FROM Recommend WHERE '{Globals.profile.METU_ID}' = Recommendee)) b; ";
            try
            {
                var reader = await command.ExecuteReaderAsync();
                while(reader.Read())
                {
                    var name = reader.GetString("Title");
                    var serialNo = reader.GetUInt32("Serial_No");
                    var author = reader.GetString("Author");
                    var imageUrl = reader.GetString("IMG_URL");
                    var firstName = reader.GetString("First_Name");
                    var surname = reader.GetString("Surname");
                    MyRecommendedBooksList.Add(new RecommendedBookInfo(imageUrl, name, author, serialNo.ToString(), $"{firstName} {surname}"));
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

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecommendedBookInfo selectedItem = e.CurrentSelection[0] as RecommendedBookInfo;
            BookSearchResultsPage.BookSerialNo = selectedItem.BookSerialNo;
            //await Navigation.PushAsync(new BookPage());
        }

        public class RecommendedBookInfo
        {
            public RecommendedBookInfo(string imageUrl, string bookName, string author, string bookSerialNo)
            {
                ImageUrl = imageUrl;
                BookName = bookName;
                Author = author;
                BookSerialNo = bookSerialNo;
            }

            public RecommendedBookInfo(string imageUrl, string bookName, string author, string bookSerialNo, string recommender)
            {
                ImageUrl = imageUrl;
                BookName = bookName;
                Author = author;
                BookSerialNo = bookSerialNo;
                Recommender = recommender;
            }

            public string Recommender { get; set; }
            public string ImageUrl { get; set; }
            public string BookName { get; set; }
            public string Author { get; set; }
            public string BookSerialNo { get; set; }
        }
    }
}
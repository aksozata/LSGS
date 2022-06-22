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
            command.CommandText = $@"SELECT * FROM Book WHERE Serial_No in
(SELECT Book_ID FROM Recommend WHERE Recommendee = '{Globals.profile.METU_ID}');";
            try
            {
                var reader = await command.ExecuteReaderAsync();
                if(reader.Read())
                {
                    var name = reader.GetString("Title");
                    var serialNo = reader.GetUInt32("Serial_No");
                    var author = reader.GetString("Author");
                    var imageUrl = reader.GetString("IMG_URL");
                    //MyRecommendedBooksList.Add(new RecommendedBookInfo(imageUrl, name, author, serialNo.ToString()));
                    MyRecommendedBooksList.Add(new RecommendedBookInfo(imageUrl, name, author, serialNo.ToString(), "AAAAAA"));
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                Globals.connection.Close();
            }

            // Get the recommender
            Globals.connection.Open();
            command.CommandText = $@"SELECT * FROM User Where METU_ID in
(SELECT Recommender FROM Recommend WHERE Recommendee = '{Globals.profile.METU_ID}');";
            try
            {
                var reader = await command.ExecuteReaderAsync();
                int ind = 0;
                if (reader.Read())
                {
                    var firstName = reader.GetString("First_Name");
                    var surname = reader.GetString("Surname");
                    //if(ind < MyRecommendedBooksList.Count)
                    //    MyRecommendedBooksList[ind].Recommender = $"{firstName} {surname}";
                    //ind++;
                }
            }
            catch (Exception ex)
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
            await Navigation.PushAsync(new BookPage());
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
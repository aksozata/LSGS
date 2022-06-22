using LSGS.Models;
using LSGS.ViewModels;
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
    public partial class MyBooksPage : ContentPage
    {
        public ObservableCollection<MyBook> MyBookList 
        { get; set; }
        public ObservableCollection<Book> BookList { get; set; }
        public MyBooksPage()
        {
            InitializeComponent();
            MyBookList = new ObservableCollection<MyBook>();
            BookList = new ObservableCollection<Book>();
            AddBooks();
            BindingContext = this;
        }


        public async void AddBooks()
        {
            MyBookList.Clear();
            BookList.Clear();
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            // Add LENT books
            if(Globals.profile.LentBookList.Count > 0)
            {
                var lentBookList = "(" + String.Join(", ", Globals.profile.LentBookList.ToArray()) + ")";
                command.CommandText = $"select * from Book where Serial_No in {lentBookList}; ";
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var Book_name = reader.GetString("Title");
                    var Publisher = reader.GetString("Publisher");
                    var Author_name = reader.GetString("Author");
                    var Published_year = reader.GetInt32("Year");
                    var Serial_no = reader.GetInt32("Serial_no");
                    var imageUrl = reader.GetString("IMG_URL");
                    MyBookList.Add(new MyBook(Book_name, Author_name, Publisher, Published_year.ToString(), imageUrl, Serial_no.ToString(), true, false));
                    BookList.Add(new Book(Book_name, Author_name, Publisher, Published_year.ToString(), imageUrl, Serial_no.ToString()));
                }
            }
            Globals.connection.Close(); 
            Globals.connection.Open();
            if (Globals.profile.ReservedBookList.Count > 0)
            {
                // Add RESERVE books
                var reservedBookList = "(" + String.Join(", ", Globals.profile.ReservedBookList.ToArray()) + ")";
                command.CommandText = $"select * from Book where Serial_No in {reservedBookList}; ";
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var Book_name = reader.GetString("Title");
                    var Publisher = reader.GetString("Publisher");
                    var Author_name = reader.GetString("Author");
                    var Published_year = reader.GetInt32("Year");
                    var Serial_no = reader.GetInt32("Serial_no");
                    var imageUrl = reader.GetString("IMG_URL");
                    MyBookList.Add(new MyBook(Book_name, Author_name, Publisher, Published_year.ToString(), imageUrl, Serial_no.ToString(), false, true));
                    BookList.Add(new Book(Book_name, Author_name, Publisher, Published_year.ToString(), imageUrl, Serial_no.ToString()));
                }
            }
            Globals.connection.Close();
        }

        public class MyBook
        {
            public MyBook()
            {
            }

            public MyBook(string name, string author, string publisher, string publishYear, string imageUrl, string serialNo, bool isLent, bool isReserved)
            {
                Name = name;
                Author = author;
                Publisher = publisher;
                PublishYear = publishYear;
                ImageUrl = imageUrl;
                SerialNo = serialNo;
                IsLent = isLent;
                IsReserved = isReserved;
            }

            public string Name { get; set; }
            public string Author { get; set; }
            public string Publisher { get; set; }
            public string PublishYear { get; set; }
            public string ImageUrl { get; set; }
            public string SerialNo { get; set; }
            public bool IsLent { get; set; }
            public bool IsReserved { get; set; }
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBookViewModel.BookSearchResultsList = BookList;
            MyBook selectedItem = e.CurrentSelection[0] as MyBook;
            BookSearchResultsPage.BookSerialNo = selectedItem.SerialNo;
            //await Shell.Current.GoToAsync("//BookPage");
            await Navigation.PushAsync(new BookPage());
        }
        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
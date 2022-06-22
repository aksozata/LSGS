using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.ViewModels;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookSearchResultsPage : ContentPage
    {
        public static string BookSerialNo;
        public ObservableCollection<Book> BookList { get; set; }
        public BookSearchResultsPage()
        {
            InitializeComponent();
            BookList = SearchBookViewModel.BookSearchResultsList;
            BindingContext = this;
        }
        
        // rate recommend comment reserve


        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book selectedItem = e.CurrentSelection[0] as Book;
            BookSerialNo = selectedItem.SerialNo;
            await Navigation.PushAsync(new BookPage());
        }

    }
}
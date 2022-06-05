using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            BindingContext = this.BookInformation;
        }
    }
}
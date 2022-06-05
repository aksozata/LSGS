using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.ViewModels;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookSearchResultsPage : ContentPage
    {
        public IList<Book> BookList { get; set; }
        public BookSearchResultsPage()
        {
            InitializeComponent();
            BookList = LSGS.ViewModels.SearchBookViewModel.BookSearchResultsList;
            BindingContext = this;
        }
        
        // rate recommend comment reserve


        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book selectedItem = e.CurrentSelection[0] as Book;
        }

    }
}
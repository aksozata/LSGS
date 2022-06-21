using LSGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyBooksPage : ContentPage
    {
        public IList<Book> BookList { get; set; }
        public MyBooksPage()
        {
            InitializeComponent();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LSGS.ViewModels;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchFriendPage : ContentPage
    {
        public SearchFriendPage()
        {
            InitializeComponent();
            this.BindingContext = new SearchFriendViewModel();
        }
    }
}
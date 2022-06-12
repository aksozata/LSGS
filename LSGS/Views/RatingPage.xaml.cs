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

        private void Post_Button_Clicked(object sender, EventArgs e)
        {
            string rating = bookRating.Items[bookRating.SelectedIndex];
            string comment = bookComment.Text;
        }
    }
}
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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = Globals.profile;
        }

        private async void Books_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyBooksPage());
        }

        private async void Friends_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FriendsListPage());
        }

        private void StudyGroups_Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void PendingRequests_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PendingRequestsPage());
        }
    }
}
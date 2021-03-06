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

        public static ProfilePage Current;
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = Globals.profile;
            Current = this;
        }

        private async void Books_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyBooksPage());
        }

        private async void Friends_Button_Clicked(object sender, EventArgs e)
        {
            FriendsListPage.IsRecommended = false;
            await Navigation.PushAsync(new FriendsListPage());
        }

        private async void StudyGroups_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyStudyGroups());
        }
        private async void RecommendedBooks_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyRecommendedBooksPage());
        }

        private async void PendingRequests_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PendingRequestsPage());
        }
        private async void MyReports_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyReportsPage());
        }
        private async void UpdateDesc_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UpdateDescriptionPage());
        }
    }
}
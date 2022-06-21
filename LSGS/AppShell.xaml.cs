using LSGS.ViewModels;
using LSGS.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LSGS
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SearchFriendPage), typeof(SearchFriendPage));
            Routing.RegisterRoute(nameof(SearchBookPage), typeof(SearchBookPage));
            Routing.RegisterRoute(nameof(FriendSearchResultsPage), typeof(FriendSearchResultsPage));
            Routing.RegisterRoute(nameof(ReportPage), typeof(ReportPage));
            Routing.RegisterRoute(nameof(PendingRequestsPage), typeof(PendingRequestsPage));


        }

        public async void OnMenuItemClicked(object sender, EventArgs e)
        {
            // The sender is the menuItem
            MenuItem menuItem = sender as MenuItem;
            switch (menuItem.Text)
            {
                case "About":
                    {
                        await Current.GoToAsync("//AboutPage");
                        Shell.Current.FlyoutIsPresented = false;
                    }
                    break;
                case "Search Book":
                    {
                        await Current.GoToAsync("SearchBookPage");
                        Shell.Current.FlyoutIsPresented = false;
                    }
                    break;
                case "Search Friend":
                    {
                        await Current.GoToAsync("SearchFriendPage");
                        Shell.Current.FlyoutIsPresented = false;
                    }
                    break;
                case "Logout":
                    {
                        await Current.GoToAsync("//LoginPage");
                        Shell.Current.FlyoutIsPresented = false;
                    }
                    break;
                case "Report":
                    {
                        await Current.GoToAsync("ReportPage");
                        Shell.Current.FlyoutIsPresented = false;
                    }
                    break;
                case "Pending Request":
                    {
                        await Current.GoToAsync("PendingRequestsPage");
                        Shell.Current.FlyoutIsPresented = false;
                    }
                    break;
            }
        }
    }
}

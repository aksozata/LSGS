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
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            // The sender is the menuItem
            MenuItem menuItem = sender as MenuItem;
            switch (menuItem.Text)
            {
                case "Search Book":
                    {
                        await Current.GoToAsync("//SearchBookPage");
                    }
                    break;
                case "Logout":
                    {
                        await Current.GoToAsync("//LoginPage");
                    }
                    break;
            }
        }
    }
}

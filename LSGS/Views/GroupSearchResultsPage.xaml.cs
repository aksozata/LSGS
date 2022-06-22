using LSGS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupSearchResultsPage : ContentPage
    {
        public ObservableCollection<Group> StudyGroupList { get; set; }
        public GroupSearchResultsPage()
        {
            InitializeComponent();
            StudyGroupList = SearchCreateGroupPage.searchResults;
            BindingContext = this;
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
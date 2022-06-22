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
    public partial class MyStudyGroups : ContentPage
    {
        public static ObservableCollection<Group> StudyGroupList { get; set; }
        public MyStudyGroups()
        {
            InitializeComponent();
        }



        private void Exit_Clicked(object sender, EventArgs e)
        {

        }

        private void friendCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
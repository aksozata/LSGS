using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccountPage : ContentPage
    {
        public CreateAccountPage()
        {

            InitializeComponent();
            this.BindingContext = new CreateAccountViewModel();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LSGS.ViewModels;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {

        public ReportPage()
        {
            InitializeComponent();
            this.BindingContext = new ReportViewModel();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using LSGS.Views;
using Xamarin.Forms;
using LSGS.Models;
using MySqlConnector;

namespace LSGS.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        public Command ReportCommand { get; }
        public ReportViewModel()
        {

        }
    }
}

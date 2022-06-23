using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.ViewModels;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyReportsPage : ContentPage
    {
        public ObservableCollection<Report> ReportList { get; set; }
        public static ObservableCollection<Report> search_result_list = new ObservableCollection<Report>();

        public MyReportsPage()
        {
            InitializeComponent();
            GetReports();
            reportCollection.ItemsSource = ReportList;
        }

        private async void GetReports()
        {
            ReportList = new ObservableCollection<Report>();
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            var command = Globals.connection.CreateCommand();
            command.CommandText =
                $@"SELECT * FROM Report WHERE Report.Owner_ID = '{Globals.profile.METU_ID}';";
            try
            {
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var Error_ID = reader.GetInt32("Error_ID");
                    var METU_ID = reader.GetInt32("Owner_ID");
                    var Description = reader.GetString("Description");
                    var Status = reader.GetString("Status");
                    var each_report = new Report(Error_ID, METU_ID, Description, Status);
                    ReportList.Add(each_report);
                }
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Loading reports failed! ", "OK");
            }
            finally { Globals.connection.Close(); }
        }       
    }
}
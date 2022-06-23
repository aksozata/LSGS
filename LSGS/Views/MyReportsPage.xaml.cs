using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.ViewModels;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyReportsPage : ContentPage
    {
        public IList<Report> ReportList { get; set; }
        public static List<Report> search_result_list = new List<Report>();

        public MyReportsPage()
        {
            InitializeComponent();
            GetReports();
            BindingContext = ReportList;
        }

        private async void GetReports()
        {
            List<Report> temp_result_list = new List<Report>();
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
                        temp_result_list.Add(each_report);
                    }
                    search_result_list = temp_result_list;
                    ReportList = search_result_list;
                Globals.connection.Close();
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Friend request is not sent! ", "OK");
            }
            finally { Globals.connection.Close(); }
        }       
    }
}
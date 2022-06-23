using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSGS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDescriptionPage : ContentPage { 

        public string description;
        public UpdateDescriptionPage()
        {
            InitializeComponent();
            
        }

        async private void GoBack_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async private void UpdateButtonClicked(object sender, EventArgs e)
        {
            description = entry1.Text;
            if (description == "" || description == null)
                return;
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            // create a DB command and set the SQL statement with parameters
            
            try
            {
                var command = Globals.connection.CreateCommand();

                command.CommandText = $@"UPDATE User SET Personal_Description = '{description}' WHERE METU_ID = '{Globals.profile.METU_ID}'";
                // execute the command and read the results
                var insert = await command.ExecuteNonQueryAsync();
                App.Current.MainPage.DisplayAlert("Congratulations", "Description is updated!", "OK");
                Globals.profile.Description = description;
                ProfilePage.Current.desc.Text = description;
                
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Error updating description!", "OK");
            }
            finally
            {
                Globals.connection.Close();            
            }
        }
    }
}
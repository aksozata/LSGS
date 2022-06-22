using LSGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LSGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudyGroupPage : ContentPage
    {
        public Group GroupInformation = new Group();
        public List<Profile> ParticipantList = new List<Profile>();
        public StudyGroupBindingClass bindingInstance;
        public StudyGroupPage()
        {
            InitializeComponent();
            GroupInformation = MyStudyGroups.SelectedGroup;
            GetParticipantList();
            string ownerName = GetOwnerName();
            bindingInstance = new StudyGroupBindingClass(GroupInformation.Name, GroupInformation.Category, GroupInformation.Description, ownerName, ParticipantList);
            BindingContext = bindingInstance;
        }

        private string GetOwnerName()
        {
            string ownerName = "";
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            var command = Globals.connection.CreateCommand();
            command.CommandText = $@"SELECT * FROM User WHERE METU_ID in (SELECT OwnerID from StudyGroup WHERE ID = '{GroupInformation.GroupId}')";
            try
            {
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var firstName = reader.GetString("First_Name");
                    var surname = reader.GetString("Surname");
                    var email = reader.GetString("Email");
                    ownerName = $"{reader.GetString("First_Name")} {reader.GetString("Surname")}";
                    ParticipantList.Add(new Profile(firstName, surname, email));
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                Globals.connection.Close();
            }
            return ownerName;
        }

        private void GetParticipantList()
        {
            List<Profile> participantList = new List<Profile>();
            if (Globals.connection.State != System.Data.ConnectionState.Open)
                Globals.connection.Open();
            var command = Globals.connection.CreateCommand();
            command.CommandText = $@"SELECT * FROM User WHERE METU_ID in (SELECT METU_ID from ParticipantIn WHERE GROUP_ID = '{GroupInformation.GroupId}')";
            try
            {
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var email = reader.GetString("Email");
                    var firstName = reader.GetString("First_Name");
                    var surname = reader.GetString("Surname");
                    participantList.Add(new Profile(firstName, surname, email));
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                Globals.connection.Close();
            }
            ParticipantList = participantList;
        }

        public class StudyGroupBindingClass
        {
            public StudyGroupBindingClass(string name, string category, string description, string ownerName, List<Profile> participantsList)
            {
                Name = name;
                Category = category;
                Description = description;
                OwnerName = ownerName;
                ParticipantList = participantsList;
            }

            public string Name { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public string OwnerName { get; set; }
            public List<Profile> ParticipantList { get; set; }
        }

        private void JoinLeaveButton_Clicked(object sender, EventArgs e)
        {

        }

        private void participants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
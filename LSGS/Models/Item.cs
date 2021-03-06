using System;
using System.Collections.Generic;

namespace LSGS.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }

    public class Profile
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? METU_ID { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public List<string> LentBookList { get; set; } 
        public List<string> ReservedBookList { get; set; }

        public Profile() {
            this.METU_ID = null;
        }
        public Profile(string Name, string Surname, int METU_ID, string Password, string Email, string Description)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.METU_ID = METU_ID;
            this.Password = Password;
            this.Email = Email;
            this.Description = Description;
            this.LentBookList = new List<string>();
            this.ReservedBookList = new List<string>();
        }

        public Profile(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }
    }
    public class Group
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int OwnerID { get; set; }
        public int GroupId { get; set; }
        public List<Student> ParticipantList { get; set; }
        public Group()
        {
        }
        public Group(string Name, string Category, string Description, int OwnerID = -1)
        {
            this.Name = Name;
            this.Category = Category;
            this.Description = Description;
            this.OwnerID = OwnerID;
            ParticipantList = new List<Student>();
        }
        public Group(string Name, string Category, string Description, int GroupId, int OwnerID = -1)
        {
            this.Name = Name;
            this.Category = Category;
            this.Description = Description;
            this.OwnerID = OwnerID;
            this.GroupId = GroupId;
            ParticipantList = new List<Student>();
        }

    }

    public class Report
    {
        public int Error_ID { get; set; }
        public int Owner_ID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Report()
        {
        }
        public Report(int Error_ID, int Owner_ID, string Description, string Status)
        {
            this.Error_ID = Error_ID;
            this.Owner_ID = Owner_ID;
            this.Description = Description;
            this.Status = Status;
        }

    }
    public class Book
    {
        public Book(string name, string author, string publisher, string publishYear, string serialNo)
        {
            Name = name;
            Author = author;
            Publisher = publisher;
            PublishYear = publishYear;
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg";
            SerialNo = serialNo;
        }

        public Book(string name, string author, string publisher, string publishYear, string imageUrl, string serialNo)
        {
            Name = name;
            Author = author;
            Publisher = publisher;
            PublishYear = publishYear;
            ImageUrl = imageUrl;
            SerialNo = serialNo;
        }


        public Book()
        {
        }

        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string PublishYear { get; set; }
        public string ImageUrl { get; set; }
        public string SerialNo { get; set; }
    }

    public class Comment
    {
        public Comment(string bookID, string userID, string userName, string userComment, string rating)
        {
            BookID = bookID;
            UserID = userID;
            UserName = userName;
            UserComment = userComment;
            Rating = rating;
        }
        public Comment(string bookID, string userID, string userComment, string rating)
        {
            BookID = bookID;
            UserID = userID;
            UserComment = userComment;
            Rating = rating;
        }

        public string BookID{ get; set; }
        public string UserID{ get; set; }
        public string UserName { get; set; }
        public string UserComment { get; set; }
        public string Rating { get; set; }
    }

    public class Student
    {

    }
}
﻿using System;

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
        public int METU_ID { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        public Profile() { }
        public Profile(string Name, string Surname, int METU_ID, string Password, string Email, string Description)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.METU_ID = METU_ID;
            this.Password = Password;
            this.Email = Email;
            this.Description = Description;
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

    public class Student
    {

    }
}
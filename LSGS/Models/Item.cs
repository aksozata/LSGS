﻿using System;

namespace LSGS.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }

    public class Book
    {
        public Book(string name, string author, string publisher, string publishYear)
        {
            Name = name;
            Author = author;
            Publisher = publisher;
            PublishYear = publishYear;
        }

        public Book()
        {
        }

        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string PublishYear { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace EachOther.Api.Models
{
    public class Article
    {
        public Article()
        {
            Comments = new List<Comment>();
        }

        public string Id {get; set;}

        public string Title {get; set;}

        public string Overview {get; set;}

        public string Content {get; set;}

        public string Category {get; set;}

        public DateTime Date {get; set;}

        public List<Comment> Comments {get; set;}
    }
}
using System;
using System.Collections.Generic;

namespace EachOther.Models
{
    public class Article
    {
        public int Id {get; set;}

        public string Title {get; set;}

        public string Overview {get; set;}

        public string Content {get; set;}

        public long Like {get; set;}

        public DateTime Date {get; set;}

        public List<Comment> Comments {get; set;}
    }
}
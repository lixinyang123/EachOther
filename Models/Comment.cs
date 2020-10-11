using System;

namespace EachOther.Models
{
    public class Comment
    {
        public int Id {get; set;}

        public int ArticleId {get; set;}

        public Article Article {get; set;}

        public string User {get; set;}

        public string Content {get; set;}

        public DateTime Date {get; set;}
    }
}
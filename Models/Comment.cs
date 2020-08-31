using System;
using System.Collections.Generic;

namespace EachOther.Models
{
    public class Comment
    {
        public int Id {get; set;}

        public int ArticleId {get; set;}

        public Article Article {get; set;}

        public string Content {get; set;}

        public DateTime Date {get; set;}

        public List<Reply> Replies {get; set;}
    }
}
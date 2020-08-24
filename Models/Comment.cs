using System;
using System.Collections.Generic;

namespace EachOther.Models
{
    public class Comment
    {
        public Comment()
        {
            Comments = new List<Comment>();
        }

        public string Id {get; set;}

        public string Content {get; set;}

        public DateTime Date {get; set;}

        public List<Comment> Comments {get; set;}
    }
}
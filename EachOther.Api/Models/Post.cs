using System;

namespace EachOther.Api.Models
{
    public class Post
    {
        public string Id {get; set;}

        public string Title {get; set;}

        public string Overview {get; set;}

        public string Category {get; set;}

        public DateTime Date {get; set;}
    }
}
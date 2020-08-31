using System;

namespace EachOther.Models
{
    public class Reply
    {
        public int Id {get; set;}

        public int CommitId {get; set;}

        public Comment Comment {get; set;}

        public string ReplayId {get; set;}

        public DateTime Date {get; set;}

        public string Content {get; set;}
    }
}
using System.ComponentModel.DataAnnotations;

namespace EachOther.ViewModels
{
    public class ArticleViewModel
    {
        public string ArticleCode {get; set;}

        [Required]
        public string Title {get; set;}

        [Required]
        public string Overview {get; set;}

        [Required]
        public string Content {get; set;}
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EachOther.ViewModels
{
    public class ArticleViewModel
    {
        public string ArticleCode {get; set;}

        [Required]
        [DisplayName("标题")]
        public string Title {get; set;}

        [Required]
        [DisplayName("封面")]
        public string CoverUrl {get; set;}

        [Required]
        [DisplayName("概述")]
        public string Overview {get; set;}

        [Required]
        [DisplayName("内容")]
        public string Content {get; set;}
    }
}
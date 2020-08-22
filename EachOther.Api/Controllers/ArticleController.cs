using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EachOther.Api.Services;
using EachOther.Api.Models;

namespace EachOther.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService articleService;

        public ArticleController(ArticleService articleService)
        {
            this.articleService = articleService;
        }

        [HttpGet("GetArticles")]
        public IActionResult GetAllArticles()
        {
            List<Article> articles = articleService.GetArticles(0,-1);
            return Content(JsonSerializer.Serialize(articles));
        }

        [HttpGet("AddArticles")]
        public IActionResult AddArticles()
        {
            var flag = articleService.AddArticle(new Article()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "afasdfawef"
            });
            return Content(flag.ToString());
        }

        [HttpGet("RemoveArticle")]
        public IActionResult RemoveArticle(string id)
        {
            var flag = articleService.RemoveArticle(id);
            return RedirectToAction("GetArticles");
        }

    }
}

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
        private readonly int pageSize = 12;

        public ArticleController(ArticleService articleService)
        {
            this.articleService = articleService;
        }

        //矫正页码
        private int CorrectIndex(int index, int pageCount)
        {
            //页码<1时留在第一页
            index = index < 1 ? 1 : index;
            //页码>总页数时留在最后一页
            index = index > pageCount ? pageCount : index;
            //如果没有博客时留在第一页
            index = pageCount == 0 ? 1 : index;
            return index;
        }

        [HttpGet("GetArticles")]
        public IActionResult GetArticles(int index)
        {
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(articleService.GetArticleCount()) / pageSize));
            index = CorrectIndex(index, pageCount);

            List<Article> articles = articleService.GetArticles((index-1)*pageSize, pageSize);
            return Content(JsonSerializer.Serialize(articles));
        }

        [HttpGet("AddArticles")]
        public IActionResult AddArticles(Article article)
        {
            var flag = articleService.AddArticle(article);
            return Content(flag.ToString());
        }

        [HttpGet("RemoveArticle")]
        public IActionResult RemoveArticle(string id)
        {
            var flag = articleService.RemoveArticle(id);
            return RedirectToAction("GetArticles");
        }

        [HttpGet("EditArticles")]
        public IActionResult EditArticles(Article article)
        {
            var flag = articleService.EditArticle(article);
            return Content(flag.ToString());
        }

    }
}

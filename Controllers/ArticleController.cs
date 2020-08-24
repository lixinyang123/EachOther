using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using EachOther.Services;
using EachOther.Models;
using EachOther.Filter;
using EachOther.ViewModels;
using Microsoft.AspNetCore.Http;

namespace EachOther.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ArticleController : Controller
    {
        private readonly ArticleService articleService;
        private readonly int pageSize = 12;

        public ArticleController(ArticleService articleService)
        {
            this.articleService = articleService;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult GetArticles(int index)
        {
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(articleService.GetArticleCount()) / pageSize));
            index = CorrectIndex(index, pageCount);

            List<Article> articles = articleService.GetArticles((index-1)*pageSize, pageSize);
            return Content(JsonSerializer.Serialize(articles));
        }

        public IActionResult AddArticle()
        {
            return View("Editor",new ArticleViewModel());
        }

        [HttpPost]
        public IActionResult AddArticle(Article article)
        {
            var flag = articleService.AddArticle(article);
            return Content(flag.ToString());
        }

        public IActionResult RemoveArticle(string id)
        {
            var flag = articleService.RemoveArticle(id);
            return RedirectToAction("GetArticles");
        }

        public IActionResult EditArticles(Article article)
        {
            var flag = articleService.EditArticle(article);
            return Content(flag.ToString());
        }

        [HttpPost]
        public IActionResult UploadImage([FromForm]IFormFile upload)
        {
            return Content("");
        }

    }
}

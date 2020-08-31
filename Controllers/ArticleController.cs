using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using EachOther.Models;
using EachOther.Filter;
using EachOther.ViewModels;
using Microsoft.AspNetCore.Http;
using EachOther.Data;

namespace EachOther.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ArticleController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly int pageSize = 12;

        public ArticleController(ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
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
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(articleDbContext.Articles.Count()) / pageSize));
            index = CorrectIndex(index, pageCount);

            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i=>i.Id)
                .Skip((index-1)*pageSize)
                .Take(pageSize).ToList();

            return Content(JsonSerializer.Serialize(articles));
        }

        public IActionResult AddArticle()
        {
            return View("Editor",new ArticleViewModel());
        }

        [HttpPost]
        public IActionResult AddArticle(ArticleViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                Article article = new Article()
                {
                    Title = viewModel.Title,
                    Overview = viewModel.Overview,
                    Content = viewModel.Content,
                    Like = 0,
                    Date = DateTime.Now
                };
                articleDbContext.Articles.Add(article);
                articleDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Editor",viewModel);
            }
        }

        public IActionResult RemoveArticle(int id)
        {
            return RedirectToAction("GetArticles");
        }

        public IActionResult EditArticles(Article article)
        {
            return Ok();
        }

        public IActionResult Detail()
        {
            return View();
        }


        // ckeditor4 中 config.js 中添加 config.filebrowserImageUploadUrl= "/Article/UploadImage";
        [HttpPost]
        public IActionResult UploadImage([FromForm]IFormFile upload)
        {
            return Json(
                new
                {
                    uploaded = 1,
                    fileName = Guid.NewGuid().ToString(),
                    url = "https://corehome.oss-accelerate.aliyuncs.com/images/f.jpg"
                }
            );
        }

    }
}

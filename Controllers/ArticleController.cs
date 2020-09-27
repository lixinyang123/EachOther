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
using EachOther.Services;
using Microsoft.Extensions.Configuration;

namespace EachOther.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ArticleController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly OssService ossService;
        private readonly int pageSize;

        public ArticleController(IConfiguration configuration, ArticleDbContext articleDbContext, OssService ossService)
        {
            this.articleDbContext = articleDbContext;
            this.ossService = ossService;
            pageSize = configuration.GetValue<int>("PageSize");
        }

        public IActionResult Index()
        {
            ViewBag.pageCount = Math.Ceiling(articleDbContext.Articles.Count() / Convert.ToDouble(pageSize));
            return View();
        }

        public IActionResult GetArticles(int index)
        {
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(articleDbContext.Articles.Count()) / pageSize));

            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i=>i.Id)
                .Skip((index-1)*pageSize)
                .Take(pageSize).ToList();

            return Content(JsonSerializer.Serialize(articles));
        }

        public IActionResult AddArticle()
        {
            ViewBag.Action = "AddArticle";
            return View("Editor",new ArticleViewModel());
        }

        [HttpPost]
        public IActionResult AddArticle(ArticleViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                Article article = new Article()
                {
                    ArticleCode = Guid.NewGuid().ToString(),
                    Title = viewModel.Title,
                    CoverUrl = viewModel.CoverUrl,
                    Overview = viewModel.Overview,
                    Content = viewModel.Content,
                    Like = 0,
                    Date = DateTime.Now.ToString()
                };
                articleDbContext.Articles.Add(article);
                articleDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Action = "AddArticle";
                return View("Editor",viewModel);
            }
        }

        public IActionResult RemoveArticle(string id)
        {
            articleDbContext.Articles.Remove(articleDbContext.Articles.Single(i=>i.ArticleCode == id));
            return RedirectToAction("Index");
        }

        public IActionResult EditArticles(string id)
        {
            ViewBag.Action = "EditArticles";
            Article article = articleDbContext.Articles.Single(i=>i.ArticleCode == id);
            return View("Editor",article);
        }

        [HttpPost]
        public IActionResult EditArticles(ArticleViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                Article article = articleDbContext.Articles.Single(i=>i.ArticleCode == viewModel.ArticleCode);
                article.Title = viewModel.Title;
                article.CoverUrl = viewModel.CoverUrl;
                article.Overview = viewModel.Overview;
                article.Content = viewModel.Content;
                articleDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Editor",viewModel);
            }
        }

        public IActionResult Detail(string id)
        {
            Article article = articleDbContext.Articles.Single(i=>i.ArticleCode == id);
            return View(article);
        }

         [HttpPost]
        public IActionResult UploadCover([FromForm]IFormFile upload)
        {
            return Content(ossService.UploadCover(upload.OpenReadStream()));
        }

        // ckeditor4 中 config.js 中添加 config.filebrowserImageUploadUrl= "/Article/UploadImage";
        [HttpPost]
        public IActionResult UploadImage([FromForm]IFormFile upload)
        {
            try
            {
                return Json(
                    new
                    {
                        uploaded = 1,
                        fileName = Guid.NewGuid().ToString(),
                        url = ossService.UploadPic(upload.OpenReadStream())
                    }
                );
            }
            catch(Exception)
            {
                return Json(new { uploaded = 0,fileName = "",url = "" });
            }
        }

    }
}

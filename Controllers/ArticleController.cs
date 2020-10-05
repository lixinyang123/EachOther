using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using EachOther.Models;
using EachOther.Filter;
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
        private readonly NotifyService notifyService;
        private readonly IConfiguration configuration;
        private readonly int pageSize;

        public ArticleController(IConfiguration configuration, 
            ArticleDbContext articleDbContext, 
            OssService ossService,
            NotifyService notifyService)
        {
            this.articleDbContext = articleDbContext;
            this.ossService = ossService;
            this.notifyService = notifyService;
            this.configuration = configuration;
            pageSize = configuration.GetValue<int>("PageSize");
        }

        public IActionResult Index()
        {
            ViewBag.pageCount = Math.Ceiling(articleDbContext.Articles.Count() / Convert.ToDouble(pageSize));
            return View();
        }

        public IActionResult GetArticles(int index = 1)
        {
            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i=>i.Id)
                .Skip((index-1)*pageSize)
                .Take(pageSize).ToList();

            return Content(JsonSerializer.Serialize(articles));
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

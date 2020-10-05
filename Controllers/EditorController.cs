using System;
using EachOther.Data;
using EachOther.Filter;
using EachOther.Models;
using EachOther.Services;
using EachOther.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EachOther.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class EditorController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly IConfiguration configuration;
        private readonly NotifyService notifyService;

        public EditorController(ArticleDbContext articleDbContext, 
            IConfiguration configuration,
            NotifyService notifyService)
        {
            this.articleDbContext = articleDbContext;
            this.configuration = configuration;
            this.notifyService = notifyService;
        }

        public IActionResult Index()
        {
            ViewBag.Action = "/Editor";
            return View(new ArticleViewModel());
        }

        [HttpPost]
        public IActionResult Index(ArticleViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                Article article = new Article()
                {
                    ArticleCode = Guid.NewGuid().ToString(),
                    User = Request.Cookies["user"],
                    Title = viewModel.Title,
                    CoverUrl = viewModel.CoverUrl,
                    Overview = viewModel.Overview,
                    Content = viewModel.Content,
                    Like = 0,
                    Date = DateTime.Now.ToString()
                };
                articleDbContext.Articles.Add(article);
                articleDbContext.SaveChanges();

                if(Request.Cookies["user"]=="Female")
                {
                    notifyService.PushNotify(configuration.GetValue<string>("MaleSckey"), "EachOther", "你收到了一条新消息，访问 EachOther 查看");
                }
                if(Request.Cookies["user"]=="Male")
                {
                    notifyService.PushNotify(configuration.GetValue<string>("FemaleSckey"), "EachOther", "你收到了一条新消息，访问 EachOther 查看");
                }
                return RedirectToAction("Index","Article");
            }
            else
            {
                ViewBag.Action = "/Editor";
                return View(viewModel);
            }
        }
    }
}
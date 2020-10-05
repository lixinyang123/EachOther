using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ManagerController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly IConfiguration configuration;
        private readonly NotifyService notifyService;
        private readonly int pageSize;

        public ManagerController(IConfiguration configuration, 
            ArticleDbContext articleDbContext,
            NotifyService notifyService)
        {
            this.articleDbContext = articleDbContext;
            this.configuration = configuration;
            this.notifyService = notifyService;
            pageSize = configuration.GetValue<int>("PageSize");
        }
        
        public IActionResult Index(int index = 1)
        {
            string user = Request.Cookies["user"];
            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i=>i.Id)
                .Where(i => i.User == user)
                .Skip((index-1)*pageSize)
                .Take(pageSize).ToList();

            return View(articles);
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
                ViewBag.Action = "AddArticle";
                return View("Editor",viewModel);
            }
        }

        public IActionResult RemoveArticle(string id)
        {
            articleDbContext.Articles.Remove(articleDbContext.Articles.Single(i=>i.ArticleCode == id));
            articleDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditArticle(string id)
        {
            ViewBag.Action = "EditArticle";
            Article article = articleDbContext.Articles.Single(i=>i.ArticleCode == id);
            ArticleViewModel viewModel = new ArticleViewModel()
            {
                ArticleCode = article.ArticleCode,
                Title = article.Title,
                CoverUrl = article.CoverUrl,
                Overview = article.Overview,
                Content = article.Content
            };
            return View("Editor",viewModel);
        }

        [HttpPost]
        public IActionResult EditArticle(ArticleViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                Article article = articleDbContext.Articles.Single(i=>i.ArticleCode == viewModel.ArticleCode);
                article.Title = viewModel.Title;
                article.CoverUrl = viewModel.CoverUrl;
                article.Overview = viewModel.Overview;
                article.Content = viewModel.Content;
                articleDbContext.SaveChanges();
                return RedirectToAction("Index","Article");
            }
            else
            {
                ViewBag.Action = "EditArticle";
                return View("Editor",viewModel);
            }
        }

    }
}
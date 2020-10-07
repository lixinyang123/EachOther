using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using EachOther.Data;
using EachOther.Filter;
using EachOther.Models;
using EachOther.Services;
using EachOther.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EachOther.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ManagerController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly IConfiguration configuration;
        private readonly NotifyService notifyService;
        private readonly IMemoryCache memoryCache;
        private readonly int pageSize;

        public ManagerController(IConfiguration configuration, 
            ArticleDbContext articleDbContext,
            NotifyService notifyService,
            IMemoryCache memoryCache)
        {
            this.articleDbContext = articleDbContext;
            this.configuration = configuration;
            this.notifyService = notifyService;
            this.memoryCache = memoryCache;
            pageSize = configuration.GetValue<int>("PageSize");
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
        
        public IActionResult Index(int index = 1)
        {
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(articleDbContext.Articles.Count()) / pageSize));
            index = CorrectIndex(index,pageCount);
            ViewBag.CurrentIndex = index;
            ViewBag.PageCount = pageCount;
            
            string user = Request.Cookies["user"];
            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i=>i.Id)
                .Where(i => i.User == user)
                .Skip((index-1)*pageSize)
                .Take(pageSize).ToList();

            return View(articles);
        }

        [HttpPost]
        public IActionResult Save(ArticleViewModel viewModel)
        {
            // 将临时文件存入缓存
            try
            {
                memoryCache.Set<ArticleViewModel>(Request.Cookies["user"],viewModel,DateTimeOffset.Now.AddDays(1));
                return Ok();
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }

        public IActionResult AddArticle()
        {
            ViewBag.Action = "AddArticle";
            
            // 从缓存读取内容
            ArticleViewModel viewModel = memoryCache.Get<ArticleViewModel>(Request.Cookies["user"]);
            if(viewModel == null)
            {
                viewModel = new ArticleViewModel();
            }
            
            return View("Editor",viewModel);
        }

        [HttpPost]
        public IActionResult AddArticle(ArticleViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Action = "AddArticle";
                return View("Editor",viewModel);
            }

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

            // 移除缓存
            memoryCache.Remove(Request.Cookies["user"]);

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
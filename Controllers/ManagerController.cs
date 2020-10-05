using System.Collections.Generic;
using System.Linq;
using EachOther.Data;
using EachOther.Filter;
using EachOther.Models;
using EachOther.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EachOther.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    public class ManagerController : Controller
    {
        private readonly ArticleDbContext articleDbContext;
        private readonly int pageSize;

        public ManagerController(IConfiguration configuration, ArticleDbContext articleDbContext)
        {
            this.articleDbContext = articleDbContext;
            pageSize = configuration.GetValue<int>("PageSize");
        }
        
        public IActionResult Index(int index = 1)
        {
            //管理逻辑
            string user = Request.Cookies["user"];
            List<Article> articles = articleDbContext.Articles
                .OrderByDescending(i=>i.Id)
                .Where(i => i.User == user)
                .Skip((index-1)*pageSize)
                .Take(pageSize).ToList();

            return View(articles);
        }

        public IActionResult RemoveArticle(string id)
        {
            articleDbContext.Articles.Remove(articleDbContext.Articles.Single(i=>i.ArticleCode == id));
            articleDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditArticles(string id)
        {
            ViewBag.Action = "EditArticles";
            Article article = articleDbContext.Articles.Single(i=>i.ArticleCode == id);
            var viewModel = new ArticleViewModel()
            {
                
            };
            return RedirectToAction("Index","Editor",viewModel);
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

    }
}
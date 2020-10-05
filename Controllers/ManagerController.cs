using System.Collections.Generic;
using System.Linq;
using EachOther.Data;
using EachOther.Filter;
using EachOther.Models;
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
    }
}
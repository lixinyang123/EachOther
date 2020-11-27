using System;
using System.Diagnostics;
using EachOther.ViewModels;
using EachOther.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace EachOther.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache cache;
        private readonly IConfiguration configuration;
        private readonly SecurityService securityService;
        private readonly NotifyService notifyService;

        public HomeController(IMemoryCache cache, 
            IConfiguration configuration,
            SecurityService securityService,
            NotifyService notifyService)
        {
            this.cache = cache;
            this.configuration = configuration;
            this.securityService = securityService;
            this.notifyService = notifyService;
        }

        public IActionResult Index()
        {
            //有管理员权限的话直接跳转的Overview验证访问令牌
            if (Request.Cookies.TryGetValue("accessToken", out _))
            {
                return RedirectToAction("Index","Article");
            }
            return View();
        }

        public IActionResult Login(int id)
        {
            string cacheKey = string.Empty;
            
            //随机生成密码
            string password = Guid.NewGuid().ToString().Substring(0, 6);
            
            try
            {
                //发送密码到手机
                if(id == 0)
                {
                    cacheKey = "Female";
                    notifyService.PushNotify(configuration.GetValue<string>("FemaleSckey"), "EachOther Login", password);
                }
                else if(id == 1)
                {
                    cacheKey = "Male";
                    notifyService.PushNotify(configuration.GetValue<string>("MaleSckey"), "EachOther Login", password);
                }
                else
                {
                    throw new Exception();
                }
                
                //记录密码并设置过期时间为一分钟
                cache.Set(cacheKey, password, DateTimeOffset.Now.AddMinutes(1));

                Response.Cookies.Append("user", cacheKey, new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddDays(1),
                    SameSite = SameSiteMode.Lax
                });

                return Content("验证码已经发送");
            }
            catch (Exception)
            {
                return Content("网络错误");
            }
        }

        [HttpPost]
        public IActionResult VerifyPassword([FromForm] string pwd)
        {
            string cacheKey = null, password = null;
            try
            {
                cacheKey = Request.Cookies["user"];
                if(cacheKey == "Male" || cacheKey == "Female")
                {
                    password = cache.Get(cacheKey).ToString();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return View("Index");
            }

            if (pwd == password && pwd != null && password != null)
            {
                //颁发访问令牌
                Response.Cookies.Append("accessToken", securityService.Encrypt(cacheKey), new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });

                //重定向到仪表盘
                return RedirectToAction("Index","Article");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

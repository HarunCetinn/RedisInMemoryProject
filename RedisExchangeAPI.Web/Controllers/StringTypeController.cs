using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
            
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db= _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            
            db.StringSet("name", "Harun");
            db.StringSet("ziyaretci", 100);


            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");
            var len = db.StringLength("name");

            db.StringIncrement("ziyaretci", 2);
            var count=db.StringDecrementAsync("ziyaretci", 1).Result;
            

            if (value.HasValue )
            {
                ViewBag.value = value.ToString();
                
            }

            ViewBag.len = len.ToString();
            return View();
        }
    }
}

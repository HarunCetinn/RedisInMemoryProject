using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            //_distributedCache.SetString("name", "Harun",cacheEntryOptions);
            //await _distributedCache.SetStringAsync("surname", "Çetin");

            Product product = new Product { Id=2,Name="Defter",Price=100};
            string jsonproduct = JsonConvert.SerializeObject(product);
            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);
            _distributedCache.Set("product:1", byteproduct);
            
            //await _distributedCache.SetStringAsync("product:2",jsonproduct,cacheEntryOptions);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonproduct = Encoding.UTF8.GetString(byteProduct);
            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);
            //string name = _distributedCache.GetString("name");
            //string surname = await _distributedCache.GetStringAsync("surname");
            ViewBag.product = p;
            //ViewBag.surname = surname;
            return View();
        }

        public async Task<IActionResult> Remove()
        {
            _distributedCache.Remove("name");
            //await _distributedCache.RemoveAsync("surname");
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] resimByte = _distributedCache.Get("resim");
            return File(resimByte, "image/jpg");

        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/e39d893812408981.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("resim", imageByte);

            return View();
        }
    }
}

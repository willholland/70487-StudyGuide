using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _70_487_Examples.Models;
using Microsoft.Extensions.Caching.Memory;

namespace _70_487_Examples.Controllers
{
    public class MemoryCachingController : Controller
    {
        private IMemoryCache _cache;
        private const string ACKEY = "ABS"; 
        private const string SCKEY = "SLID";

        public MemoryCachingController(IMemoryCache memoryCache){
            _cache = memoryCache;
        }
        
        public IActionResult Index()
        {   
            var a = TryGetValueFromCache(ACKEY);
            ViewData[ACKEY] = TryGetValueFromCache(ACKEY);
            ViewData[SCKEY] = TryGetValueFromCache(SCKEY);
                        
            return View();
        }

        [HttpPost]
        public IActionResult CacheAbsoluteExpiration(string value){
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(new TimeSpan(0, 0, 30));
            var m = new CachingModel(value, false);
            _cache.Set(ACKEY, m, cacheEntryOptions);
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult CacheSlidingExpiration(string value){
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(new TimeSpan(0, 0, 30));
            var m = new CachingModel(value, true);
            _cache.Set(SCKEY, m, cacheEntryOptions);
            return RedirectToAction("Index");
        }
        
                
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private CachingModel TryGetValueFromCache(string key)
        {
            CachingModel cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry))
            {
               return null;
            }
            return cacheEntry;
        }
    }
}

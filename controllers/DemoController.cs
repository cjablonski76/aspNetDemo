using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ConsoleApp.SQLite;

namespace ConsoleApplication
{
    [Route("[controller]")]
    public class DemoController : Controller
    {
        private readonly IMyUsefulInterface _service;

        public DemoController(IMyUsefulInterface service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            _service = service;
        }

        [HttpGet]
        public async Task<string> Index()
        {
            return await Task.FromResult("Hello Asp.net mvc Get");
        }

        [HttpGet("query")]
        public async Task<string> Query([FromQuery] string text)
        {
            return await Task.FromResult($"You entered [{text}]");
        }

        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            return await Task.FromResult($"Item Id: [{id}]");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Blog blog)
        {
            using (var db = new BloggingContext())
            {
                db.Blogs.Add(blog);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var blogEntry in db.Blogs)
                {
                    Console.WriteLine(" - {0}", blogEntry.Url);
                }
            }
            return await Task.FromResult(Created("http://localhost/demo/", blog));
        }
    }
}
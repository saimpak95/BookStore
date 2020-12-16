using BookStore_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILoggerService loggerService;

        public HomeController(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            loggerService.LogInfo("Accessed Home Controller");
            return Ok("Hello World");
        }
    }
}

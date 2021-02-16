using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;

namespace Optsol.Playground.Security.Identity.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public DefaultController(IWebHostEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return new PhysicalFileResult(Path.Combine(this.environment.WebRootPath, "index.html"), new MediaTypeHeaderValue("text/html"));
        }
    }
}

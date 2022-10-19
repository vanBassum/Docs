using Docs.Models;
using Docs.Services;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace Docs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, FileWatcherService s)
        {
            _logger = logger;
        }

        public IActionResult Index(string? id)
        {
            Contents contents = new Contents();
            contents.Root = GetFolder("wwwroot/docs");
            id ??= "wwwroot/docs/index.md";
            id = HttpUtility.UrlDecode(id);
            contents.Name = id;
            return View(contents);
        }

        public IActionResult GetContent(string? id)
        {
            id ??= "wwwroot/docs/index.md";
            id = HttpUtility.UrlDecode(id);
            string content;


            if (System.IO.File.Exists(id))
            {
                string markdown = System.IO.File.ReadAllText(id);
                content = Markdown.ToHtml(markdown);
            }
            else
            {
                content = Markdown.ToHtml("File not found");
            }

            return PartialView("_Content", content);
        }



        public Folder GetFolder(string path)
        {
            Folder folder = new Folder(path);
            foreach (string dir in Directory.GetDirectories(path))
                folder.Folders.Add(GetFolder(dir));

            foreach (string file in Directory.GetFiles(path))
                folder.Files.Add(file);

            return folder;
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
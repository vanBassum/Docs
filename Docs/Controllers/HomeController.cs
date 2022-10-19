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
        private readonly SettingsManager<Settings> _settingsManager;

        public HomeController(ILogger<HomeController> logger, FileWatcherService s, SettingsManager<Settings> settingsManager)
        {
            _logger = logger;
            _settingsManager = settingsManager;
        }

        public IActionResult Index(string? id)
        {
            Contents contents = new Contents();
            contents.Title = _settingsManager.Settings.Title;
            if(id == null)
            {
                if(_settingsManager.Settings.HomePage != null)
                    id = Path.Combine("wwwroot/docs/", _settingsManager.Settings.HomePage);
            }
            id = NormalizePath(id); 
            contents.Name = id;
            contents.Root = GetFolder("wwwroot/docs", Path.GetDirectoryName(id));
            return View(contents);
        }

        public static string? NormalizePath(string? path)
        {
            if (path == null)
                return null;
            path = HttpUtility.UrlDecode(path);
            path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public Folder GetFolder(string path, string? req)
        {
            Folder folder = new Folder(path);
            foreach (string dir in Directory.GetDirectories(path))
            {
                var ddir = NormalizePath(dir);
                Folder nFolder = GetFolder(ddir, req);
                nFolder.Expanded = req == ddir;
                folder.Folders.Add(nFolder);
            }
                
            foreach (string file in Directory.GetFiles(path, "*.md"))
                folder.Files.Add(file);

            return folder;
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
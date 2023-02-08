using Docs.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.IO;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Docs.Services
{
    public class FileWatcherService : IDisposable
    {
        private readonly IHubContext<FileChangeHub> _hub;
        private readonly FileSystemWatcher _watcher;


        public FileWatcherService(IHubContext<FileChangeHub> hub)
        {
            _hub = hub;
            Directory.CreateDirectory(@"wwwroot/docs");
            _watcher = new FileSystemWatcher(@"wwwroot/docs");
            _watcher.NotifyFilter = NotifyFilters.Attributes
                                  | NotifyFilters.CreationTime
                                  | NotifyFilters.DirectoryName
                                  | NotifyFilters.FileName
                                  | NotifyFilters.LastAccess
                                  | NotifyFilters.LastWrite
                                  | NotifyFilters.Security
                                  | NotifyFilters.Size;

            _watcher.Changed += _watcher_Changed;
            //_watcher.Created += OnChanged;
            //_watcher.Deleted += OnChanged;
            //_watcher.Renamed += OnChanged;
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            _hub.Clients.All.SendAsync("FileChanged", e.FullPath.Replace("\\", "/"));
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}

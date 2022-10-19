using Docs.Hubs;
using Microsoft.AspNetCore.SignalR;
using SharpYaml.Serialization;

namespace Docs.Services
{
    public class SettingsManager<T>
    {
        const string settingsFile = "wwwroot/docs/settings.yml";
        public T Settings { get; private set; }


        public SettingsManager()
        {
            var serializer = new Serializer();
            if (File.Exists(settingsFile))
            {
                string text = File.ReadAllText(settingsFile);
                try
                {
                    Settings = serializer.Deserialize<T>(text);
                }
                catch
                {

                }
            }
            else
            {
                Settings ??= Activator.CreateInstance<T>();
                var text = serializer.Serialize(Settings);
                File.WriteAllText(settingsFile, text);
            }
            Settings ??= Activator.CreateInstance<T>();
        }
    }
}

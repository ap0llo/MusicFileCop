using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicFileCop.Core.Configuration
{
    internal class ConfigurationWriter : IConfigurationWriter
    {

        public void WriteConfiguration(IConfigurationNode configuration, string outputPath)
        {
            var json = new JObject();

            foreach (var name in configuration.Names)
            {
                SerializeSetting(name, configuration, json);
            }

            File.WriteAllText(outputPath, json.ToString(Formatting.Indented));
        }


        void SerializeSetting(string name, IConfigurationNode configurationNode, JObject json)
        {
            if (name.Contains(":"))
            {
                var prefix = name.Split(':').First();

                var innerJson = json[prefix] as JObject;
                if (innerJson == null)
                {
                    innerJson = new JObject();
                    json[prefix] = innerJson;
                }


                var newName = name.Remove(0, prefix.Length + 1);
                SerializeSetting(newName, new PrefixConfigurationNode(configurationNode, prefix), innerJson);
            }
            else
            {
                var value = configurationNode.GetValue(name);
                json[name] = value;
            }
        }

    }
}
using System.Collections.Generic;

namespace MusicFileCop.Core.Configuration
{
    public interface IConfigurationNode
    {
        bool TryGetValue(string name, out string value);

        string GetValue(string name);

        bool TryGetValue<T>(string name, out T value);

        T GetValue<T>(string name);

        IEnumerable<string> Names { get; }

    }
}

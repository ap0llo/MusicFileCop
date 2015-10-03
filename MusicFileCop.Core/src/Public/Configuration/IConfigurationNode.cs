namespace MusicFileCop.Core.Configuration
{
    public interface IConfigurationNode
    {
        string GetValue(string name); 

        T GetValue<T>(string name);   
    }
}

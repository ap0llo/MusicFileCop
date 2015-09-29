namespace MusicFileCop.Model.Configuration
{
    public interface IMutableConfigurationNode : IConfigurationNode
    {

        /// <summary>
        /// Adds the specified key value pair to the configuration node.
        /// 
        /// If a setting with the specified name already exists, the exisiting value will be replaced with 
        /// the specifed value
        /// </summary>
        /// <typeparam name="T">The type of the value to add to the node</typeparam>
        /// <param name="name">The name of the value to add</param>
        /// <param name="value">The value to add</param>
        /// <remarks>name is case-insensitive</remarks>
        void AddValue<T>(string name, T value);

    }
}
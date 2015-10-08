namespace MusicFileCop.Core
{
    public interface IDynamicConfigurator
    {
        /// <summary>
        /// Configures the ninject kernel before the program runs
        /// </summary>
        void CreateDynamicBindings();
    }
}
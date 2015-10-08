namespace MusicFileCop.Core
{
    public interface IDynamicConfigurator
    {
        /// <summary>
        /// Configures the Ninject kernel before the program runs
        /// </summary>
        void CreateDynamicBindings();
    }
}
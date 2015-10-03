using Microsoft.Framework.ConfigurationModel;
using Moq;

namespace MusicFileCop.Core.Test.Configuration.Helpers
{
    class ConfigurationMockHelper
    {

        private readonly Mock<IConfiguration> m_Mock = new Mock<IConfiguration>(MockBehavior.Strict);


        public ConfigurationMockHelper()
        {
            string dummy;
            m_Mock.Setup(m => m.TryGet(It.IsAny<string>(), out dummy)).Returns(false);
        }


        public Mock<IConfiguration> Mock => m_Mock;

        public IConfiguration Object => m_Mock.Object;

        public ConfigurationMockHelper AddValue(string key, string value)
        {
            m_Mock.Setup(m => m.Get(key)).Returns(value);
            m_Mock.Setup(m => m.TryGet(key, out value)).Returns(true);
            return this;
        }


        

    }
}

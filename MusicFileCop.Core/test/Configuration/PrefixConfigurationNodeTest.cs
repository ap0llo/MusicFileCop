using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MusicFileCop.Core.Configuration;
using Xunit;

namespace MusicFileCop.Core.Test.Configuration
{
    public class PrefixConfigurationNodeTest
    {


        [Fact]
        public void Test_Constructor_PrefixMustNotBeNull()
        {
            var wrappedConfigurationNode = new Mock<IConfigurationNode>(MockBehavior.Strict).Object;

            var exception = Assert.Throws<ArgumentNullException>(() => new PrefixConfigurationNode(wrappedConfigurationNode, null));
            Assert.Equal("prefix", exception.ParamName);
        }

        [Fact]
        public void Test_Constructor_PrefixMustNotBeEmpty()
        {
            var wrappedConfigurationNode = new Mock<IConfigurationNode>(MockBehavior.Strict).Object;

            var exception = Assert.Throws<ArgumentException>(() => new PrefixConfigurationNode(wrappedConfigurationNode, ""));
            Assert.Equal("prefix", exception.ParamName);
        }


        [Fact]
        public void Test_Constructor_WrappedConfigurationNodeMustNotBeNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new PrefixConfigurationNode(null, "Irrelevant"));
            Assert.Equal("wrappedConfigurationNode", exception.ParamName);
        }



        [Fact]
        public void Test_GetValue()
        {
            const string prefix = "SomePrefix";
            const string key = "key";
            const string value = "value";

            var mock = new Mock<IConfigurationNode>(MockBehavior.Strict);
            mock.Setup(m => m.GetValue($"{prefix}:{key}")).Returns(value);

            var instance = new PrefixConfigurationNode(mock.Object, prefix);

            Assert.Equal(value, instance.GetValue(key));

            mock.Verify(m => m.GetValue(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void Test_GetTypedValue()
        {
            const string prefix = "SomePrefix";
            const string key = "key";
            
            var mock = new Mock<IConfigurationNode>(MockBehavior.Strict);
            mock.Setup(m => m.GetValue<bool>($"{prefix}:{key}")).Returns(true);

            var instance = new PrefixConfigurationNode(mock.Object, prefix);

            Assert.Equal(true, instance.GetValue<bool>(key));

            mock.Verify(m => m.GetValue<bool>(It.IsAny<string>()), Times.Once());
        }
    }
}

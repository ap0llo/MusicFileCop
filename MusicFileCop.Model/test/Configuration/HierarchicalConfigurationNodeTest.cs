using Moq;
using MusicFileCop.Model.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusicFileCop.Model.Test.Configuration
{
    public class HierarchicalConfigurationNodeTest
    {
        [Fact]
        public void Test_Constructor_ParentNodeIsNull()
        {
            var node = new HierarchicalConfigurationNode(null, new Dictionary<string, string>());
        }

        [Fact]
        public void Test_Constructor_ValuesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new HierarchicalConfigurationNode(null, null));
        }


        [Fact]
        public void Test_GetValue_String()
        {
            var node = new HierarchicalConfigurationNode(null, new Dictionary<string, string>()
            {
                { "SomeKey", "SomeValue" }
            });

            Assert.Equal("SomeValue", node.GetValue<string>("SomeKey"));
        }

        [Fact]
        public void Test_GetValue_Bool()
        {
            var node = new HierarchicalConfigurationNode(null, new Dictionary<string, string>()
            {
                { "SomeKey", "true" },
                { "SomeKey2", "trUE" },
                { "SomeKey3", "FALse" }
            });

            Assert.Equal(true, node.GetValue<bool>("SomeKey"));
            Assert.Equal(true, node.GetValue<bool>("SomeKey2"));
            Assert.Equal(false, node.GetValue<bool>("SomeKey3"));
        }

        public void Test_GetValue_Bool_Invalid()
        {
            var node = new HierarchicalConfigurationNode(null, new Dictionary<string, string>()
            {
                { "SomeKey", "abcd" }
            });

            Assert.Throws<ArgumentException>(() => node.GetValue<bool>("SomeKey"));            
        }

        [Fact]
        public void Test_GetValue_Int()
        {
            var node = new HierarchicalConfigurationNode(null, new Dictionary<string, string>()
            {
                { "SomeKey", "1234" },                
            });

            Assert.Equal(1234, node.GetValue<int>("SomeKey"));
        }


        [Fact]
        public void Test_GetValue_Int_Invalid()
        {
            var node = new HierarchicalConfigurationNode(null, new Dictionary<string, string>()
            {
                { "SomeKey", "sdafdv" },
            });

            Assert.Throws<ArgumentException>(() =>node.GetValue<int>("SomeKey"));
        }

        [Fact]
        public void Test_GetValue_NotFound()
        {
            var node = new HierarchicalConfigurationNode(null, new Dictionary<string, string>());

            Assert.Throws<KeyNotFoundException>(() => node.GetValue<string>("SomeKey"));
        }


        [Fact]
        public void Test_GetValue_FromParentNode()
        {
            var expectedValue = Guid.NewGuid().ToString();

            var parentNodeMock = new Mock<IConfigurationNode>(MockBehavior.Strict);
            parentNodeMock.Setup(x => x.GetValue<string>("SomeKey")).Returns(expectedValue);

            var node = new HierarchicalConfigurationNode(parentNodeMock.Object, new Dictionary<string, string>());

            Assert.Equal(expectedValue, node.GetValue<string>("SomeKey"));
            parentNodeMock.Verify(x => x.GetValue<string>("SomeKey"), Times.Once);
        }

        [Fact]
        public void Test_GetValue_NodeValueOverridesParentNodeValue()
        {
            var expectedValue = Guid.NewGuid().ToString();

            var parentNodeMock = new Mock<IConfigurationNode>(MockBehavior.Strict);
            parentNodeMock.Setup(x => x.GetValue<string>("SomeKey")).Returns("");

            var node = new HierarchicalConfigurationNode(parentNodeMock.Object, new Dictionary<string, string>()
            {
                {"SomeKey", expectedValue }
            });

            Assert.Equal(expectedValue, node.GetValue<string>("SomeKey"));
            parentNodeMock.Verify(x => x.GetValue<string>("SomeKey"), Times.Never);
        }


        [Fact]
        public void Test_GetValue_KeyIsCaseInsensitive()
        {
            var value = "SomeValue";
            var node = new HierarchicalConfigurationNode(null, new Dictionary<string, string>()
            {
                { "SomeKey", value },
            });

            Assert.Equal(value, node.GetValue<string>("soMEKey"));

        }
    }
}

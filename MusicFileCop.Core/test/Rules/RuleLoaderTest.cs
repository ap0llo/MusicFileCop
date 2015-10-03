using System;
using System.Linq;
using System.Reflection;
using Moq;
using MusicFileCop.Core;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using Ninject;
using Xunit;

namespace MusicFileCop.Core.Test.Rules
{

    public class RuleLoaderTest : IDisposable
    {
        readonly Mock<IKernel> m_KernelMock = new Mock<IKernel>(MockBehavior.Strict);
        DynamicConfigurator m_Instance;

        public RuleLoaderTest()
        {            
            this.m_Instance = new DynamicConfigurator(m_KernelMock.Object, new MutableConfigurationNode());
        }

        public void Dispose()
        {
            this.m_Instance = null;
        }



        [Fact]
        public void Test_GetCheckableTypes_FromCurrentAssembly()
        {            
            var expected = new[] { typeof(TestCheckable) };
            var actual = m_Instance.GetCheckableTypes(Assembly.GetExecutingAssembly()).ToArray();

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(typeof(IFile))]
        [InlineData(typeof(IDirectory))]
        [InlineData(typeof(IAlbum))]
        [InlineData(typeof(IArtist))]
        [InlineData(typeof(IDisk))]
        [InlineData(typeof(ITrack))]        
        public void Test_GetCheckableTypes_IncludesType(Type type)
        {
            var checkables = m_Instance.GetCheckableTypes();
            Assert.Contains(type, checkables);       
        }
      

    }
}

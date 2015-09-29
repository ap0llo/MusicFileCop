using System;
using System.Linq;
using System.Reflection;
using Moq;
using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;
using Ninject;
using Xunit;

namespace MusicFileCop.Model.Test.Rules
{

    public class RuleLoaderTest : IDisposable
    {
        readonly Mock<IKernel> m_KernelMock = new Mock<IKernel>(MockBehavior.Strict);
        RuleLoader m_Instance;

        public RuleLoaderTest()
        {            
            this.m_Instance = new RuleLoader(m_KernelMock.Object);
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

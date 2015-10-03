using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MusicFileCop.Core.FileSystem;
using Xunit;
using static MusicFileCop.Core.Test.DirectoryMockHelper;

namespace MusicFileCop.Core.Test
{
    
    public class MetadataMapperTest
    {       
        readonly MetadataMapper m_Instance = new MetadataMapper();


        [Fact]
        public void Test_CombineCommonAncestors_1()
        {
            /*
                root
                  |- dir1  
                  |- dir2  
         
                => root is common ancestor of all directories
            */

            var root = CreateDirectoryMock().Named("root")
                .WithSubDirectories("dir1", "dir2");

            var result = m_Instance.CombineCommonAncestors(root.Object.Directories).ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal("root", result.First().Name);
        }

        [Fact]
        public void Test_CombineCommonAncestors_2()
        {
            /*
                root
                  |- dir1  
                  |- dir2  
                  |- foo              
            */

            var root = CreateDirectoryMock().Named("root")
                .WithSubDirectories("dir1", "dir2", "foo1");

            var result = m_Instance.CombineCommonAncestors(root.Object.Directories.Where(x => x.Name.StartsWith("dir"))).ToList();


            Assert.Equal(2, result.Count);
            Assert.True(result.Any(x => x.Name == "dir1"));
            Assert.True(result.Any(x => x.Name == "dir2"));
        }

        [Fact]
        public void Test_CombineCommonAncestors_3()
        {
            /*
                root
                  |- dir1  
                      |-dir1.1
                      |-dir1.2
                  |- dir2                              
            */

            var dir11 = CreateDirectoryMock().Named("dir1.1");
            var dir12 = CreateDirectoryMock().Named("dir1.2");
            var dir1 = CreateDirectoryMock().Named("dir1").WithSubDirectories(dir11, dir12);
            var dir2 = CreateDirectoryMock().Named("dir2");

            var root = CreateDirectoryMock().Named("root").WithSubDirectories(dir1, dir2);

            var result = m_Instance.CombineCommonAncestors(new[] { dir11.Object, dir2.Object }).ToList();

            Assert.Equal(2, result.Count);
            Assert.True(result.Any(x => x.Name == dir11.Object.Name));
            Assert.True(result.Any(x => x.Name == dir2.Object.Name));
        }        

        [Fact]
        public void Test_CombineCommonAncestors_4()
        {
            /*
                root
                  |- dir1  
                      |-dir11
                           |-dir111
                           |-dir112                      
                  |- dir2                              
            */

            var dir111 = CreateDirectoryMock()
                .Named("dir111");

            var dir112 = CreateDirectoryMock()
                .Named("dir112");

            var dir11 = CreateDirectoryMock()
                .Named("dir11")
                .WithSubDirectories(dir111, dir112);
            
            var dir1 = CreateDirectoryMock()
                .Named("dir1")
                .WithSubDirectories(dir11);

            var dir2 = CreateDirectoryMock()
                .Named("dir2");

            var root = CreateDirectoryMock().Named("root").WithSubDirectories(dir1, dir2);

            var result = m_Instance.CombineCommonAncestors(new[] { dir111.Object, dir112.Object, dir2.Object }).ToList();

            Assert.Equal(1, result.Count);
            Assert.True(result.Any(x => x.Name == root.Object.Name));
        }
    }

}

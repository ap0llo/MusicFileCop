using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core.Test
{
    static class DirectoryMockHelper
    {
        public static Mock<IDirectory> CreateDirectoryMock()
        {
            var mock = new Mock<IDirectory>(MockBehavior.Strict);
            mock.Setup(x => x.ParentDirectory).Returns<IDirectory>(null);
            return mock;
        }

        public static Mock<IDirectory> WithSubDirectories(this Mock<IDirectory> parentDirectory, params Mock<IDirectory>[] dirs)
        {
            foreach (var mock in dirs)
            {
                mock.Setup(m => m.ParentDirectory).Returns(parentDirectory.Object);
            }

            parentDirectory.Setup(dir => dir.Directories).Returns(dirs.Select(m => m.Object));
            return parentDirectory;
        }

        public static Mock<IDirectory> WithSubDirectories(this Mock<IDirectory> parentDirectory, params string[] names)
        {
            var subdirectoryMocks = names.Select(name => CreateDirectoryMock().Named(name)).ToArray();
            return parentDirectory.WithSubDirectories(subdirectoryMocks);
        }

        public static Mock<IDirectory> Named(this Mock<IDirectory> directory, string name)
        {
            directory.Setup(mock => mock.Name).Returns(name);
            return directory;
        }
    }
}

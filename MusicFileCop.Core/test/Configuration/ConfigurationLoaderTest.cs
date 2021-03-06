﻿using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Directory = MusicFileCop.Core.FileSystem.Directory;

namespace MusicFileCop.Core.Test.Configuration
{
    public class ConfigurationLoaderTest : IDisposable
    {

        readonly List<string> m_TempFiles = new List<string>();


        [Fact]
        public void Test_LoadConfigurationFile()
        {
            var filePath = CreateConfigFile("config.json",
                new JObject(
                    new JProperty("SomeKey", "SomeValue"),
                    new JProperty("SomeKey1",
                        new JObject(new JProperty("SubKey", "SomeOtherValue")))));

            var fileMock = MockFile("Irrelevant", filePath, null);
            
            var configurationLoader = new ConfigurationLoader(new ConfigurationMapper(), MockDefaultConfigurationNode().Object);
            var configNode = configurationLoader.LoadConfigurationFile(fileMock.Object);

            Assert.Equal("SomeValue", configNode.Get("SomeKey"));
            Assert.Equal("SomeOtherValue", configNode.Get("SomeKey1:SubKey"));

            
        }

        [Fact]
        public void Test_LoadConfigurationForFile()
        {
            var filePath = CreateConfigFile("config.json",
              new JObject(
                  new JProperty("SomeKey", "SomeValue")));

            var directory = new Directory(null, "dir");

            var fileMock = MockFile("file.ext", "", directory);
            directory.AddFile(fileMock.Object);

            var configFileMock = MockFile("file.ext.MusicFileCop.json", filePath, directory);            
            directory.AddFile(configFileMock.Object);
                      
            var mapper = new ConfigurationMapper();

            var configurationLoader = new ConfigurationLoader(mapper, MockDefaultConfigurationNode().Object);
            configurationLoader.LoadConfiguration(MockConfigurationNode().Object, fileMock.Object);

            var config = mapper.GetConfiguration(fileMock.Object);
            Assert.NotNull(config);

            Assert.Equal("SomeValue", config.GetValue("SomeKey"));
            

        }


        [Fact]
        public void Test_LoadConfigurationForDirectory()
        {
            var filePath = CreateConfigFile("config.json",
              new JObject(
                  new JProperty("SomeKey", "SomeValue")));

            var directory = new Directory(null, "dir");

            var configFileMock = MockFile("MusicFileCop.json", filePath, directory);
            directory.AddFile(configFileMock.Object);

            var mapper = new ConfigurationMapper();

            var configurationLoader = new ConfigurationLoader(mapper, MockDefaultConfigurationNode().Object);
            configurationLoader.LoadConfiguration(MockConfigurationNode().Object, directory);

            var config = mapper.GetConfiguration(directory);
            Assert.NotNull(config);
            Assert.Equal("SomeValue", config.GetValue("SomeKey"));
            
        }

        [Fact]
        public void Test_FileInheritsDirectoryConfiguration()
        {
            var directoryConfig = CreateConfigFile("config1.json",
             new JObject(
                 new JProperty("SomeKey", "SomeValue")));
            
            var directory = new Directory(null, "Irrelevant");

            var directoryConfigMock = MockFile("MusicFileCop.json", directoryConfig, directory);
            var fileMock = MockFile("file1.ext", "", directory);

            directory.AddFile(directoryConfigMock.Object);
            directory.AddFile(fileMock.Object);

            
            var mapper = new ConfigurationMapper();
                       
            var configurationLoader = new ConfigurationLoader(mapper, MockDefaultConfigurationNode().Object);
            configurationLoader.LoadConfiguration(MockConfigurationNode().Object, directory);

            var fileConfig = mapper.GetConfiguration(fileMock.Object);

            Assert.NotNull(fileConfig);
            Assert.Equal("SomeValue", fileConfig.GetValue("SomeKey"));            
        }

        string CreateConfigFile(string fileName, JObject config)
        {
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            using (var stream = new StreamWriter(System.IO.File.OpenWrite(filePath)))
            using (var writer = new JsonTextWriter(stream))
            {
                config.WriteTo(writer);
            }

            m_TempFiles.Add(filePath);

            return filePath;
        }


        Mock<IFile> MockFile(string name, string fullPath, IDirectory directory)
        {
            var fileMock = new Mock<IFile>(MockBehavior.Strict);
            fileMock.Setup(x => x.NameWithExtension).Returns(name);
            fileMock.Setup(x => x.FullPath).Returns(fullPath);
            fileMock.Setup(x => x.Directory).Returns(directory);

            return fileMock;
        }

        Mock<IConfigurationNode> MockConfigurationNode() => new Mock<IConfigurationNode>(MockBehavior.Strict);

        Mock<IDefaultConfigurationNode> MockDefaultConfigurationNode() => new Mock<IDefaultConfigurationNode>(MockBehavior.Strict);

        public void Dispose()
        {
            foreach (var file in m_TempFiles)
            {
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
        }
    }
}

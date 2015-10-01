using MusicFileCop.Model;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.Configuration;

namespace MusicFileCop.Rules
{

    class FileNameConstants
    {
        public const string ConfigurationNamespace = "FileName";
        public const string InvalidCharReplacementSettingName = "InvalidCharReplacement";
    }

    public class FileNameDefaultConfiguration : IDefaultConfigurationProvider
    {
        public string ConfigurationNamespace => FileNameConstants.ConfigurationNamespace;

        public void Configure(IMutableConfigurationNode configurationNode)
        {
            configurationNode.AddValue(FileNameConstants.InvalidCharReplacementSettingName, "");
        }
    }


    public abstract class FileNameRule
    {
        readonly IMetadataMapper m_MetadataMapper;
        readonly IConfigurationMapper m_ConfigurationMapper;

        protected FileNameRule(IMetadataMapper metadataMapper, IConfigurationMapper configurationMapper)
        {
            if (configurationMapper == null)
            {
                throw new ArgumentNullException(nameof(configurationMapper));
            }
            if (metadataMapper == null)
            {
                throw new ArgumentNullException(nameof(metadataMapper));
            }

            m_ConfigurationMapper = configurationMapper;
            m_MetadataMapper = metadataMapper;
        }


        protected string TrackNameToFileName(ITrack track)
        {
            var file = m_MetadataMapper.GetFile(track);
            var configuration = m_ConfigurationMapper.GetConfiguration(file);

            var name = track.Name;

            var replacement = configuration.GetValue(FileNameConstants.InvalidCharReplacementSettingName);

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalidChar.ToString(), replacement);
            }

            return name;
        }
    }



    [ConfigurationNamespace(FileNameConstants.ConfigurationNamespace)]
    public class SingleDiskFileNameRule : FileNameRule, IRule<ITrack>
    {
        readonly IMetadataMapper m_MetadataMapper;

        public SingleDiskFileNameRule(IMetadataMapper metadataMapper, IConfigurationMapper configurationMapper) 
            : base(metadataMapper, configurationMapper)
        {
            if (metadataMapper == null)
            {
                throw new ArgumentNullException(nameof(metadataMapper));
            }
            this.m_MetadataMapper = metadataMapper;
        }


        public string Description => @"For albums that only have a single disk, the filename matches the format '{TRACKNUMBER}- {TITLE}'";

        public bool IsApplicable(ITrack track) => track.Album.Disks.Count() == 1;

        public bool IsConsistent(ITrack track)
        {
            var expectedFileName = $"{track.TrackNumber:D2} - {TrackNameToFileName(track)}";
            var actualFileName = m_MetadataMapper.GetFile(track).Name;            

            return expectedFileName == actualFileName;
        }

    }

    [ConfigurationNamespace(FileNameConstants.ConfigurationNamespace)]
    public class MultiDiskFileNameRule : FileNameRule, IRule<ITrack>
    {

        readonly IMetadataMapper m_MetadataMapper;

        public MultiDiskFileNameRule(IMetadataMapper metadataMapper, IConfigurationMapper configurationMapper) 
            : base(metadataMapper, configurationMapper)
        {
            if (metadataMapper == null)
            {
                throw new ArgumentNullException(nameof(metadataMapper));
            }
            this.m_MetadataMapper = metadataMapper;
        }


        public string Description => @"For albums that only have a single disk, the filename matches the format '{DISKNUMBER}-{TRACKNUMBER}- {TITLE}'";

        public bool IsApplicable(ITrack track) => track.Album.Disks.Count() > 1;

        public bool IsConsistent(ITrack track) => m_MetadataMapper.GetFile(track).Name == GetExpectedFileName(track);



        string GetExpectedFileName(ITrack track) => $"{track.Disk.DiskNumber}-{track.TrackNumber} - {TrackNameToFileName(track)}";
    }

}

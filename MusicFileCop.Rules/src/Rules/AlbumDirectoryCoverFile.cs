using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model;
using MusicFileCop.Model.Configuration;

namespace MusicFileCop.Rules
{
    class AlbumDirectoryCoverFileConstants
    {
        public const string ConfigurationNamespace = "AlbumDirectoryCoverFile";
        public const string CoverFileSettingName = "CoverFileName";
    }


    [ConfigurationNamespace(AlbumDirectoryCoverFileConstants.ConfigurationNamespace)]
    public class AlbumDirectoryCoverFileRule : IRule<IAlbum>
    {
        private readonly IMapper m_FileMapper;

        public AlbumDirectoryCoverFileRule(IMapper fileMapper)
        {
            if(fileMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMapper));
            }
            this.m_FileMapper = fileMapper;
        }

        public string Description => "In every directory containig a album, there must be a Cover.jpg file";
        
        public bool IsApplicable(IAlbum album) => true;
        
        public bool IsConsistent(IAlbum album)
        {            
            return m_FileMapper.GetDirectories(album).All(dir =>
            {
                var coverFileName = m_FileMapper.GetConfiguration(dir).GetValue(AlbumDirectoryCoverFileConstants.CoverFileSettingName);
                return dir.FileExists(coverFileName);
            });
        }
    }

    public class AlbumDirectoryCoverFileDefaultConfiguration : IDefaultConfigurationProvider
    {
        public string ConfigurationNamespace => AlbumDirectoryCoverFileConstants.ConfigurationNamespace;

        public void Configure(IMutableConfigurationNode configurationNode)
        {
            configurationNode.AddValue(AlbumDirectoryCoverFileConstants.CoverFileSettingName, "Cover.jpg");
        }
    }

}

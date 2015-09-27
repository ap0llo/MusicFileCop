using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model;

namespace MusicFileCop.Rules
{
    public class AlbumDirectoryMustContainCoverFile : IRule<IAlbum>
    {
        private const string s_CoverFileName = "Cover.jpg";
        private readonly IMapper m_FileMapper;


        public AlbumDirectoryMustContainCoverFile(IMapper fileMapper)
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
            return m_FileMapper.GetDirectories(album).All(dir => dir.FileExists(s_CoverFileName));
        }
    }
}

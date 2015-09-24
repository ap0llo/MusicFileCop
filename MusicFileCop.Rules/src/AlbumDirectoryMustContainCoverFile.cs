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

        public string Description => "In Every directory containign a media file, there must be a Cover.jpg file";
        
        public bool IsApplicable(IFileMapper fileMapper, IAlbum album) => true;
        
        public bool IsConsistent(IFileMapper fileMapper, IAlbum album)
        {
            return fileMapper.GetDirectories(album).All(dir => dir.FileExists(s_CoverFileName));
        }
    }
}

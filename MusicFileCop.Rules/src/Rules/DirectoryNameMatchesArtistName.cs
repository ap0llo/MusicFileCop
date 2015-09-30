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
    public class DirectoryNameMatchesArtistName : IRule<IAlbum>
    {

        readonly IMetadataMapper m_FileMetadataMapper;

        public DirectoryNameMatchesArtistName(IMetadataMapper fileMetadataMapper)
        {
            if(fileMetadataMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMetadataMapper));
            }
            this.m_FileMetadataMapper = fileMetadataMapper;
        }


        public string Description => "All album directories must be contained in directories named the same as the album artist";        

        public bool IsApplicable(IAlbum album) => true;
        
        public bool IsConsistent(IAlbum album) => m_FileMetadataMapper.GetDirectories(album).All(dir => dir.Name == album.Name);
      
    }
}

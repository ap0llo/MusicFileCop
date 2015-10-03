using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Core;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

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

        public bool IsApplicable(IAlbum album) => !String.IsNullOrWhiteSpace(album.Artist?.Name);
        
        public bool IsConsistent(IAlbum album)
        {
            var consistent = m_FileMetadataMapper
                .GetDirectories(album)
                .All(dir => album.Artist.Name.ReplaceInvalidFileNameChars("").Equals(dir.ParentDirectory?.Name, StringComparison.InvariantCultureIgnoreCase));

            if (!consistent)
            {
                
            }

            return consistent;
        }
    }
}

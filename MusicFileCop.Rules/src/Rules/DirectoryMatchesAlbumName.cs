using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model;
using MusicFileCop.Model.Rules;
using MusicFileCop.Model.Metadata;

namespace MusicFileCop.Rules
{
    public class DirectoryNameMatchesAlbumName : IRule<ITrack>
    {
        readonly IMetadataMapper m_FileMetadataMapper;

        public DirectoryNameMatchesAlbumName(IMetadataMapper fileMetadataMapper)
        {
            if(fileMetadataMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMetadataMapper));
            }

            this.m_FileMetadataMapper = fileMetadataMapper;
        }


        public string Description => "The directory a music file is located in must have the same name as the album";

        public bool IsApplicable(ITrack track) => !String.IsNullOrEmpty(track.Album?.Name);

        public bool IsConsistent(ITrack track) => m_FileMetadataMapper.GetFile(track).Directory.Name == track.Album.Name;
        
    }
}

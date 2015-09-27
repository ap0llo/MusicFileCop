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
        readonly IMapper m_FileMapper;

        public DirectoryNameMatchesAlbumName(IMapper fileMapper)
        {
            if(fileMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMapper));
            }

            this.m_FileMapper = fileMapper;
        }


        public string Description => "The directory a music file is located in must have the same name as the album";

        public bool IsApplicable(ITrack _) => true;

        public bool IsConsistent(ITrack track) => m_FileMapper.GetFile(track).Directory.Name == track.Album.Name;
        
    }
}

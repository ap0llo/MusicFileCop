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
    class DirectoryNameMatchesArtistName : IRule<IAlbum>
    {

        readonly IMapper m_FileMapper;

        public DirectoryNameMatchesArtistName(IMapper fileMapper)
        {
            if(fileMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMapper));
            }
            this.m_FileMapper = fileMapper;
        }


        public string Description => "All album directories must be contained in directories named the same as the album artist";        

        public bool IsApplicable(IAlbum album) => true;
        
        public bool IsConsistent(IAlbum album) => m_FileMapper.GetDirectories(album).All(dir => dir.Name == album.Name);
      
    }
}

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
        public string Description => "The directory a music file is located in must have the same name as the album";

        public bool IsApplicable(IFileMapper _, ITrack __) => true;

        public bool IsConsistent(IFileMapper mapper, ITrack track) => mapper.GetFile(track).Directory.Name == track.Album.Name;
        
    }
}

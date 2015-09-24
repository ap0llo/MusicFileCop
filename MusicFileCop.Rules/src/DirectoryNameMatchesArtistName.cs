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
        public string Description => "All album directories must be contained in directories named the same as the album artist";        

        public bool IsApplicable(IFileMapper fileMapper, IAlbum album) => true;
        
        public bool IsConsistent(IFileMapper fileMapper, IAlbum album)
        {
            var albumDirectories = album.Disks.SelectMany(disk => disk.Tracks)
                                              .Select(fileMapper.GetFile)
                                              .Select(file => file.Directory)
                                              .Distinct();

            return albumDirectories.All(dir => dir.Name == album.Name);
        }
    }
}

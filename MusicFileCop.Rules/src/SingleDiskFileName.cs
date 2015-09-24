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
    class SingleDiskFileName : IRule<ITrack>
    {
        public string Description => @"For albums that only have a single disk, the filename matches the format '{TRACKNUMBER}- {TITLE}'";               

        public bool IsApplicable(IFileMapper fileMapper, ITrack track) => track.Album.Disks.Count() == 1;
        
        public bool IsConsistent(IFileMapper fileMapper, ITrack track) => fileMapper.GetFile(track).Name == $"{track.TrackNumber} - {track.Name}";

    }
}

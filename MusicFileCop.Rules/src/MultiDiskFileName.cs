using MusicFileCop.Model;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Rules
{
    public class MultiDiskFileName : IRule<ITrack>
    {
        public string Description => @"For albums that only have a single disk, the filename matches the format '{DISKNUMBER}-{TRACKNUMBER}- {TITLE}'";

        public bool IsApplicable(IFileMapper fileMapper, ITrack track) => track.Album.Disks.Count() > 1;

        public bool IsConsistent(IFileMapper fileMapper, ITrack track) => fileMapper.GetFile(track).Name == GetExpectedFileName(track);



        string GetExpectedFileName(ITrack track) => $"{track.Disk.DiskNumber}-{track.TrackNumber} - {track.Name}";
    }

}

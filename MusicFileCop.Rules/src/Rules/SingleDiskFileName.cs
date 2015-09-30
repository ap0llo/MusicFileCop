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
    public class SingleDiskFileName : IRule<ITrack>
    {
        readonly IMetadataMapper m_FileMetadataMapper;

        public SingleDiskFileName(IMetadataMapper fileMetadataMapper)
        {
            if (fileMetadataMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMetadataMapper));
            }
            this.m_FileMetadataMapper = fileMetadataMapper;
        }


        public string Description => @"For albums that only have a single disk, the filename matches the format '{TRACKNUMBER}- {TITLE}'";               

        public bool IsApplicable(ITrack track) => track.Album.Disks.Count() == 1;

        public bool IsConsistent(ITrack track)
        {
            var expectedFileName = $"{track.TrackNumber:D2} - {track.Name}";
            var actualFileName = m_FileMetadataMapper.GetFile(track).Name;

            if(expectedFileName != actualFileName)
            {
                
            }

            return expectedFileName == actualFileName;
        }

    }
}

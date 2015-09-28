﻿using MusicFileCop.Model.Metadata;
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
        readonly IMapper m_FileMapper;

        public SingleDiskFileName(IMapper fileMapper)
        {
            if (fileMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMapper));
            }
            this.m_FileMapper = fileMapper;
        }


        public string Description => @"For albums that only have a single disk, the filename matches the format '{TRACKNUMBER}- {TITLE}'";               

        public bool IsApplicable(ITrack track) => track.Album.Disks.Count() == 1;

        public bool IsConsistent(ITrack track)
        {
            var expectedFileName = $"{track.TrackNumber:D2} - {track.Name}";
            var actualFileName = m_FileMapper.GetFile(track).Name;

            if(expectedFileName != actualFileName)
            {
                
            }

            return expectedFileName == actualFileName;
        }

    }
}

﻿using System.Linq;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Rules
{
    public class DiskNumberMustBeUniqueRule : IRule<IDisk>

    {
        public string Id => RuleIds.DiskNumberMustBeUnique;

        public string Description => "There must only be a single disk for any number in each album";

        public bool IsApplicable(IDisk item) => true;

        public bool IsConsistent(IDisk item) => item.Album.Disks.Count(d => d.DiskNumber == item.DiskNumber) == 1;

    }
}
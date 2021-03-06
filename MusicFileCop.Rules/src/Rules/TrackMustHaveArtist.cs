﻿using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Rules
{
    public class TrackMustHaveArtistRule : IRule<ITrack>
    {
        public string Id => RuleIds.TrackMustHaveArtist;

        public string Description => "A artist has to be specified for every track";

        public bool IsApplicable(ITrack item) => true;

        public bool IsConsistent(ITrack item) => item.Artist != null;

    }
}
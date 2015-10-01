using System;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Rules
{
    public class TrackNameMustNotBeEmptyRule : IRule<ITrack>
    {
        public string Description => "The name of a album must not be empty";

        public bool IsApplicable(ITrack item) => true;

        public bool IsConsistent(ITrack item) => !String.IsNullOrWhiteSpace(item.Name);
    
    }
}
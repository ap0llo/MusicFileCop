using System;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Rules
{
    public class TrackNameMustNotBeEmptyRule : IRule<ITrack>
    {
        public string Id => RuleIds.TrackNameMustNotBeEmpty;

        public string Description => "The name of a album must not be empty";

        public bool IsApplicable(ITrack item) => true;

        public bool IsConsistent(ITrack item) => !String.IsNullOrWhiteSpace(item.Name);
    
    }
}
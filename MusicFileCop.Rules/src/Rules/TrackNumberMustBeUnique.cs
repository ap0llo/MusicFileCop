using System.Linq;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Rules
{
    public class TrackNumberMustBeUniqueRule : IRule<ITrack>
    {
        public string Id => RuleIds.TrackNumberMustBeUnique;

        public string Description => "There must only be a single track for any number in each disk";

        public bool IsApplicable(ITrack item) => true;

        public bool IsConsistent(ITrack item) => item.Disk.Tracks.Count(t => t.TrackNumber == item.TrackNumber) == 1;

    }
}
using System.Linq;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Rules
{
    public class TrackNumberMustBeUniqueRule : IRule<ITrack>
    {
        public string Description => "There must only be a single track for any number in each disk";

        public bool IsApplicable(ITrack item) => true;

        public bool IsConsistent(ITrack item) => item.Disk.Tracks.Count(t => t.TrackNumber == item.TrackNumber) == 1;

    }
}
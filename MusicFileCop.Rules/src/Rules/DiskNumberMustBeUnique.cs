using System.Linq;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Rules
{
    public class DiskNumberMustBeUniqueRule : IRule<IDisk>

    {
        public string Description => "There must only be a single track for any number in each album";

        public bool IsApplicable(IDisk item) => true;

        public bool IsConsistent(IDisk item) => item.Album.Disks.Count(d => d.DiskNumber == item.DiskNumber) == 1;

    }
}
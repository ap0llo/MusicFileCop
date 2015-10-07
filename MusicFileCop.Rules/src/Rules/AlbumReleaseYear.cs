using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Rules
{
    public class AlbumReleaseYear : IRule<IAlbum>
    {
        public string Id => RuleIds.AlbumReleaseYearMustNotBeZero;

        public string Description => "Checks whether a album has a release year specified";

        public bool IsApplicable(IAlbum item) => true;

        public bool IsConsistent(IAlbum item) => item.ReleaseYear != 0;

    }
}
﻿using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Rules
{
    public class AlbumMustHaveArtistRule : IRule<IAlbum>
    {
        public string Description => "The album artist has to be specified for every album";
        public bool IsApplicable(IAlbum item) => true;

        public bool IsConsistent(IAlbum item) => item.Artist != null;

    }
}
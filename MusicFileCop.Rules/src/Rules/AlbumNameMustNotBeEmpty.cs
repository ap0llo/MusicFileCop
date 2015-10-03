﻿using System;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Rules
{
    public class AlbumNameMustNotBeEmptyRule : IRule<IAlbum>
    {
        public string Description => "The name of an album must not be empty";
        public bool IsApplicable(IAlbum item) => true;

        public bool IsConsistent(IAlbum item) => !String.IsNullOrWhiteSpace(item.Name);

    }
}
﻿using System;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Rules
{
    public class ArtistNameMustNotBeEmptyRule : IRule<IArtist>
    {
        public string Description => "The name of an artist must not be empty";

        public bool IsApplicable(IArtist item) => true;

        public bool IsConsistent(IArtist item) => !String.IsNullOrWhiteSpace(item.Name);    
            
    }
}
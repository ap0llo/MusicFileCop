using System;
using System.Linq;
using MusicFileCop.Core;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Rules
{
    public class DirectoryNameMatchesAlbumNameRule : IRule<IAlbum>
    {
        readonly IMetadataMapper m_FileMetadataMapper;

        public DirectoryNameMatchesAlbumNameRule(IMetadataMapper fileMetadataMapper)
        {
            if(fileMetadataMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMetadataMapper));
            }

            this.m_FileMetadataMapper = fileMetadataMapper;
        }


        public string Id => RuleIds.DirectoryNameMatchesAlbumName;

        public string Description => "The directory a music file is located in must have the same name as the album";

        public bool IsApplicable(IAlbum album) => !String.IsNullOrWhiteSpace(album.Name);

        public bool IsConsistent(IAlbum album)
        {
            return m_FileMetadataMapper
                .GetDirectories(album)
                .All(dir => album.Name.ReplaceInvalidFileNameChars("").Equals(dir.Name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}

using System;
using System.Linq;
using MusicFileCop.Core;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Rules
{
    public class SingleAlbumDirectoryRule : IRule<IAlbum>
    {
        public string Id => RuleIds.SingleAlbumDirectory;

        public string Description => "Checks whether all files that are part of an album are located within a single directory";



        readonly IMetadataMapper m_MetadataMapper;

        public SingleAlbumDirectoryRule(IMetadataMapper metadataMapper)
        {
            if (metadataMapper == null)
            {
                throw new ArgumentNullException(nameof(metadataMapper));
            }
            m_MetadataMapper = metadataMapper;
        }


        public bool IsApplicable(IAlbum item) => true;

        public bool IsConsistent(IAlbum item) => m_MetadataMapper.GetDirectories(item).Count() == 1;
    }
}
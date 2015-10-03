using System;
using System.Collections.Generic;

namespace MusicFileCop.Core.Metadata
{
    class Artist : IArtist
    {
        readonly IDictionary<Tuple<string, int>, Album> m_Albums = new Dictionary<Tuple<string, int>, Album>(new TupleComparer());
        

        public IEnumerable<IAlbum> Albums => m_Albums.Values;
        
        public string Name { get; internal set; }
        

        internal void AddAlbum(Album album)
        {
            m_Albums.Add(new Tuple<string, int>(album.Name, album.ReleaseYear), album);
        }

        internal bool AlbumExists(string name, int releaseYear) => m_Albums.ContainsKey(new Tuple<string, int>(name, releaseYear));

        internal Album GetAlbum(string name, int releaseYear) => m_Albums[new Tuple<string, int>(name, releaseYear)];

        public void Accept(IVisitor visitor) => visitor.Visit(this);



        private class TupleComparer : IEqualityComparer<Tuple<string, int>>
        {

            public bool Equals(Tuple<string, int> x, Tuple<string, int> y)
            {
                return StringComparer.InvariantCultureIgnoreCase.Equals(x.Item1, y.Item1) &&
                    x.Item2 == y.Item2;
            }

            public int GetHashCode(Tuple<string, int> obj)
            {
                return StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.Item1);
            }
        }
    }
}

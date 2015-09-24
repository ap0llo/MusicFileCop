using Id3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using TagLib.Mpeg;

namespace MusicFileCop
{
    class Program
    {
        static void Main(string[] args)
        {

            var path = @"C:\Users\Andreas\Desktop\01 - Prelude.mp3";

            using (var file = new AudioFile(path))
            {
                var tag = file.GetTag(TagTypes.Id3v2);

                Console.WriteLine("Title: {0}", tag.Title);
                Console.WriteLine("AlbumArtist: {0}", tag.AlbumArtists.FirstOrDefault());
                Console.WriteLine("Artist: {0}", tag.Performers.FirstOrDefault());
                Console.WriteLine("Album: {0}", tag.Album);
            }


            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Model.Output
{
    class ConsoleOutputWriter : 
        IOutputWriter<IFile>, 
        IOutputWriter<IDirectory>,
        IOutputWriter<IArtist>, 
        IOutputWriter<IAlbum>, 
        IOutputWriter<IDisk>, 
        IOutputWriter<ITrack>
    {


//        public void WriteViolation<T>(IRule<T> violatedRule, T item) where T : ICheckable
//        {

//        }
        public void WriteViolation(IRule<IFile> violatedRule, IFile file)
        {
           Console.WriteLine($"File {file.FullPath} violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<IDirectory> violatedRule, IDirectory directory)
        {
            Console.WriteLine($"Directory {directory.FullPath} violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<IArtist> violatedRule, IArtist artist)
        {
            Console.WriteLine($"Artist '{artist.Name}' violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<IAlbum> violatedRule, IAlbum album)
        {
            Console.WriteLine($"Album '{album.Name}' by '{album.Artist.Name}' violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<IDisk> violatedRule, IDisk disk)
        {
            Console.WriteLine($"Disk {disk.DiskNumber} from Album '{disk.Album.Name}' by '{disk.Album.Artist.Name}' violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<ITrack> violatedRule, ITrack track)
        {
            Console.WriteLine($"Track {track.Disk.DiskNumber}.{track.TrackNumber} ('{track.Name}') from Album '{track.Album.Name}' by '{track.Album.Artist.Name}' violates Rule {violatedRule.GetType().Name}");
        }
    }
}

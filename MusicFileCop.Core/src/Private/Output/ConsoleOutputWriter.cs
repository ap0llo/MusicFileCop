using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Output;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Core.Output
{
    class ConsoleOutputWriter : 
        IOutputWriter<IFile>, 
        IOutputWriter<IDirectory>,
        IOutputWriter<IArtist>, 
        IOutputWriter<IAlbum>, 
        IOutputWriter<IDisk>, 
        IOutputWriter<ITrack>
    {


        public void WriteViolation(IRule<IFile> violatedRule, Severity severity, IFile file)
        {
           Console.WriteLine($"File {file.FullPath} violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<IDirectory> violatedRule, Severity severity, IDirectory directory)
        {
            Console.WriteLine($"Directory {directory.FullPath} violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<IArtist> violatedRule, Severity severity, IArtist artist)
        {
            Console.WriteLine($"Artist '{artist.Name}' violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<IAlbum> violatedRule, Severity severity, IAlbum album)
        {
            Console.WriteLine($"Album '{album.Name}' by '{album.Artist.Name}' violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<IDisk> violatedRule, Severity severity, IDisk disk)
        {
            Console.WriteLine($"Disk {disk.DiskNumber} from Album '{disk.Album.Name}' by '{disk.Album.Artist.Name}' violates Rule {violatedRule.GetType().Name}");
        }

        public void WriteViolation(IRule<ITrack> violatedRule, Severity severity, ITrack track)
        {
            Console.WriteLine($"Track {track.Disk.DiskNumber}.{track.TrackNumber} ('{track.Name}') from Album '{track.Album.Name}' by '{track.Album.Artist.Name}' violates Rule {violatedRule.GetType().Name}");
        }
    }
}

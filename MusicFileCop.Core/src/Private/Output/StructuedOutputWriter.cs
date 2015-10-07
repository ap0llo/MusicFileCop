using System;
using System.Collections.Generic;
using System.Linq;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Core.Output
{
    class StructuedOutputWriter :
        IOutputWriter<IFile>,
        IOutputWriter<IDirectory>,
        IOutputWriter<IArtist>,
        IOutputWriter<IAlbum>,
        IOutputWriter<IDisk>,
        IOutputWriter<ITrack>, IDisposable
    {
        readonly IMetadataMapper m_MetadataMapper;
        readonly IDictionary<IDirectory, List<IRule>> m_RuleViolationsByDirectory = new Dictionary<IDirectory, List<IRule>>();
        readonly IDictionary<IFile, List<IRule>> m_RuleViolationsByFile = new Dictionary<IFile, List<IRule>>();


        public StructuedOutputWriter(IMetadataMapper metadataMapper)
        {
            if (metadataMapper == null)
            {
                throw new ArgumentNullException(nameof(metadataMapper));
            }
            m_MetadataMapper = metadataMapper;
        }

        public void Dispose()
        {
            WriteToConsole();
        }


        public void WriteViolation(IRule<IAlbum> violatedRule, Severity severity, IAlbum album)
        {
            foreach (var directory in m_MetadataMapper.GetDirectories(album))
            {
                AddRuleViolation(directory, violatedRule);
            }
        }

        public void WriteViolation(IRule<IArtist> violatedRule, Severity severity, IArtist artist)
        {
            foreach (var directory in m_MetadataMapper.GetDirectories(artist))
            {
                AddRuleViolation(directory, violatedRule);
            }
        }

        public void WriteViolation(IRule<IDirectory> violatedRule, Severity severity, IDirectory item)
        {
            AddRuleViolation(item, violatedRule);
        }

        public void WriteViolation(IRule<IDisk> violatedRule, Severity severity, IDisk disk)
        {
            foreach (var directory in m_MetadataMapper.GetDirectories(disk))
            {
                AddRuleViolation(directory, violatedRule);
            }
        }

        public void WriteViolation(IRule<IFile> violatedRule, Severity severity, IFile item)
        {
            AddRuleViolation(item, violatedRule);
        }

        public void WriteViolation(IRule<ITrack> violatedRule, Severity severity, ITrack track)
        {
            var file = m_MetadataMapper.GetFile(track);
            AddRuleViolation(file, violatedRule);
        }


        void AddRuleViolation(IDirectory directory, IRule violatedRule)
        {
            if (!m_RuleViolationsByDirectory.ContainsKey(directory))
            {
                m_RuleViolationsByDirectory.Add(directory, new List<IRule>());
            }

            m_RuleViolationsByDirectory[directory].Add(violatedRule);
        }

        void AddRuleViolation(IFile file, IRule violatedRule)
        {
            if (!m_RuleViolationsByFile.ContainsKey(file))
            {
                m_RuleViolationsByFile.Add(file, new List<IRule>());
            }

            m_RuleViolationsByFile[file].Add(violatedRule);
        }

        IEnumerable<IRule> GetRuleViolations(IDirectory directory)
        {
            if (!m_RuleViolationsByDirectory.ContainsKey(directory))
            {
                return Enumerable.Empty<IRule>();
            }
            return m_RuleViolationsByDirectory[directory];
        }

        IEnumerable<IRule> GetRuleViolations(IFile file)
        {
            if (!m_RuleViolationsByFile.ContainsKey(file))
            {
                return Enumerable.Empty<IRule>();
            }
            return m_RuleViolationsByFile[file];
        }


        void WriteToConsole()
        {
            var roots = m_RuleViolationsByDirectory.Keys.Select(GetRoot)
                .Union(m_RuleViolationsByFile.Keys.Select(GetRoot))
                .Distinct().ToList();

            foreach (var rootDirectory in roots)
            {
                WriteToConsole(rootDirectory, 0);
            }
        }

        void WriteToConsole(IDirectory directory, int indentationDepth)
        {
            var violations = GetRuleViolations(directory).ToList();
            if (violations.Any())
            {
                WriteLine(directory.FullPath, indentationDepth);
                WriteLine();
                WriteToConsole(violations, indentationDepth + 1);
            }

            foreach (var file in directory.Files)
            {
                WriteToConsole(file, indentationDepth + 1);
            }

            foreach (var dir in directory.Directories)
            {
                WriteToConsole(dir, indentationDepth + 1);
            }
        }

        void WriteToConsole(IFile file, int indentationDepth)
        {
            var violations = GetRuleViolations(file).ToList();
            if (violations.Any())
            {
                WriteLine(file.FullPath, indentationDepth);
                WriteToConsole(violations, indentationDepth + 1);
            }
        }

        void WriteToConsole(IEnumerable<IRule> violatedRules, int indentationDepth)
        {
            WriteLine("Violated Rules:", indentationDepth);
            foreach (var ruleType in violatedRules.Distinct())
            {
                WriteLine(ruleType.Id, indentationDepth + 1);
            }
            WriteLine();
        }


        IDirectory GetRoot(IDirectory dir)
        {
            while (dir.ParentDirectory != null)
            {
                dir = dir.ParentDirectory;
            }
            return dir;
        }

        IDirectory GetRoot(IFile file) => GetRoot(file.Directory);


        void WriteLine() => Console.WriteLine();

        void WriteLine(string message, int indentationDepth)
        {
            var prefix = indentationDepth > 0
                ? Enumerable.Range(0, indentationDepth).Select(x => "  ").Aggregate((a, b) => a + b)
                : "";

            Console.WriteLine(prefix + message);
        }
    }
}
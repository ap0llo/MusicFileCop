using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Rules;

namespace MusicFileCop.Core.Output
{
    /// <summary>
    ///     Output Writer that collects all rule violations and can write them to a TextWriter ordered by directory and file
    /// </summary>
    internal class StructuedTextOutputWriter :
        IOutputWriter<IFile>,
        IOutputWriter<IDirectory>,
        IOutputWriter<IArtist>,
        IOutputWriter<IAlbum>,
        IOutputWriter<IDisk>,
        IOutputWriter<ITrack>,
        ITextOutputWriter
    {
        readonly IMetadataMapper m_MetadataMapper;
        readonly IDictionary<IDirectory, List<IRule>> m_RuleViolationsByDirectory = new Dictionary<IDirectory, List<IRule>>();
        readonly IDictionary<IFile, List<IRule>> m_RuleViolationsByFile = new Dictionary<IFile, List<IRule>>();


        public StructuedTextOutputWriter(IMetadataMapper metadataMapper)
        {
            if (metadataMapper == null)
            {
                throw new ArgumentNullException(nameof(metadataMapper));
            }
            m_MetadataMapper = metadataMapper;
        }


        public void WriteViolation(IRule<IAlbum> violatedRule, Severity severity, IAlbum album)
        {
            // add violations to all directories contain files that are part of the album
            foreach (var directory in m_MetadataMapper.GetDirectories(album))
            {
                AddRuleViolation(directory, violatedRule);
            }
        }

        public void WriteViolation(IRule<IArtist> violatedRule, Severity severity, IArtist artist)
        {
            // add violations to all directories contain files that are from this artist
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
            //add violation for all of the disk's directories
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

        public void WriteTo(TextWriter writer)
        {
            // determine root directories (usually, there shouldn't be more than one)
            var roots = m_RuleViolationsByDirectory.Keys.Select(GetRoot)
                .Union(m_RuleViolationsByFile.Keys.Select(GetRoot))
                .Distinct().ToList();

            foreach (var rootDirectory in roots)
            {
                WriteTo(writer, rootDirectory, 0);
            }
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


        void WriteTo(TextWriter writer, IDirectory directory, int indentationDepth)
        {
            // get violations for the current directory
            var violations = GetRuleViolations(directory).ToList();
            if (violations.Any())
            {
                // write directory path, then write all violated rules
                writer.WriteIndentedLine(directory.FullPath, indentationDepth);
                writer.WriteLine();
                WriteTo(writer, violations, indentationDepth + 1);
            }

            //recurse

            foreach (var file in directory.Files)
            {
                WriteTo(writer, file, indentationDepth + 1);
            }

            foreach (var dir in directory.Directories)
            {
                WriteTo(writer, dir, indentationDepth + 1);
            }
        }

        void WriteTo(TextWriter writer, IFile file, int indentationDepth)
        {
            // get all violations for the file
            var violations = GetRuleViolations(file).ToList();

            if (violations.Any())
            {
                //write file path and violations to output
                writer.WriteIndentedLine(file.FullPath, indentationDepth);
                WriteTo(writer, violations, indentationDepth + 1);
            }
        }

        void WriteTo(TextWriter writer, IEnumerable<IRule> violatedRules, int indentationDepth)
        {
            // write the specified rules to the output
            writer.WriteIndentedLine("Violated Rules:", indentationDepth);
            foreach (var rule in violatedRules.Distinct())
            {
                writer.WriteIndentedLine(rule.Id, indentationDepth + 1);
            }
            writer.WriteLine();
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
    }
}
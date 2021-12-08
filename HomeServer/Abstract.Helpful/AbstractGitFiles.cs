using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Abstract.Helpful.Lib.Logging;

namespace Abstract.Helpful.Lib
{
    public abstract class AbstractGitFiles
    {
        private readonly string _gitDirectoryKeyFileName;
        
        private string _currentAssemblyDirectory = null;
        private string _gitDirectory = null;

        protected AbstractGitFiles(string gitDirectoryKeyFileName)
        {
            _gitDirectoryKeyFileName = gitDirectoryKeyFileName;
        }

        private string CurrentAssemblyDirectory
        {
            get
            {
                if (_currentAssemblyDirectory == null)
                {
                    string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                    UriBuilder uri = new UriBuilder(codeBase);
                    var path = Uri.UnescapeDataString(uri.Path);
                    _currentAssemblyDirectory = Path.GetDirectoryName(path);
                }
                return _currentAssemblyDirectory;
            }
        }

        protected string GitDirectory
        {
            get
            {
                if (_gitDirectory == null)
                {
                    _gitDirectory = FindGitDirectory(CurrentAssemblyDirectory);
                }
                return _gitDirectory;
            }
        }

        protected string GitSubDirectory(string path)
        {
            if (GitDirectory == string.Empty)
                return path;
            
            return Path.Combine(GitDirectory, path);
        }

        private string FindGitDirectory(string deepDirectory)
        {
            try
            {
                var directorySeparator = '\\';
            
                var directoryParts = deepDirectory
                    .Split(directorySeparator)
                    .Reverse()
                    .ToList();

                var fullDirectories = new List<string>();
                for (var i = 0; i <= directoryParts.Count; i++)
                {
                    var fullDirectoryBuilder = new StringBuilder();
                    for (var k = directoryParts.Count - 1; k >= i; k--)
                    {
                        fullDirectoryBuilder.Append(directoryParts[k]);
                        if (k != i)
                            fullDirectoryBuilder.Append(directorySeparator);
                    }
                    var fullDirectory = fullDirectoryBuilder.ToString();
                    fullDirectories.Add(fullDirectory);
                }
            
                var gitDirectory = fullDirectories
                    .FirstOrDefault(d => Directory
                        .GetFiles(d)
                        .Select(sd => sd
                            .Split(directorySeparator)
                            .Last())
                        .Contains(_gitDirectoryKeyFileName));

                if (gitDirectory == null)
                    return string.Empty;

                return gitDirectory;
            }
            catch (Exception e)
            {
                StaticLogger.Log(e.ToPrettyDevelopersString());
                return string.Empty;
            }
        }
    }
}
using System.Text.RegularExpressions;
using ProjectLens.config;
using ProjectLens.core;
using ProjectLens.utils;

namespace ProjectLens.manager
{
    public static class DirectoryManager
    {
        public static List<ProjectItem> GetProjects()
        {
            var projects = new List<ProjectItem>();
            try
            {
                string workDir = Config.GetWorkDirectory().Value;
                string excludePattern = @"System Volume Information|^\$";

                var folderPaths = Directory
                    .GetDirectories(workDir)
                    .Where(path =>
                        !Regex.IsMatch(
                            Path.GetFileName(path),
                            excludePattern,
                            RegexOptions.IgnoreCase
                        )
                    );

                foreach (string path in folderPaths)
                {
                    string typePath = Path.Combine(path, "project-type.txt");
                    string type = File.Exists(typePath)
                        ? File.ReadAllText(typePath).Trim()
                        : "type undefined";

                    projects.Add(
                        new ProjectItem
                        {
                            Path = path,
                            Name = Path.GetFileName(path),
                            Type = type,
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error: {ex.Message}");
            }
            return projects;
        }
    }
}

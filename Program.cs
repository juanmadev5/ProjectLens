using ProjectLens.app;
using ProjectLens.config;
using ProjectLens.core;
using ProjectLens.manager;
using ProjectLens.utils;

namespace ProjectLens
{
    public class Program
    {
        public static void Main()
        {
            if (Config.GetWorkDirectory().Key == ActionResult.undefined)
            {
                Console.WriteLine(Config.GetWorkDirectory().Value);
                Screen.Clear();
                Console.WriteLine(AppStrings.welcome);
                while (true)
                {
                    Console.WriteLine(AppStrings.setWorkDir);
                    Console.WriteLine(AppStrings.enterWorkDir);
                    string? dir = Console.ReadLine();

                    if (dir == null)
                    {
                        Console.WriteLine(AppStrings.invalidDir);
                    }
                    else
                    {
                        while (true)
                        {
                            Console.WriteLine(AppStrings.setDir.Replace("{dir}", dir));
                            string? option = Console.ReadLine();
                            if (option != null || option?.ToLower() == "y")
                            {
                                break;
                            }
                        }
                        Config.SetWorkDirectory(dir);
                        Console.WriteLine(AppStrings.dirSetSuccess);
                        break;
                    }
                }
            }

            int selectedIndex = 0;
            var projects = DirectoryManager.GetProjects();

            while (true)
            {
                Screen.Clear();
                Console.WriteLine(AppStrings.cliHeader);
                Console.WriteLine(
                    AppStrings.currentDir.Replace("{dir}", $"{Config.GetWorkDirectory().Value}")
                );

                for (int i = 0; i < projects.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"> {projects[i].Name, -30} | {projects[i].Type}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {projects[i].Name, -30} | {projects[i].Type}");
                    }
                }

                Console.WriteLine(AppStrings.actions);

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Q && key.Modifiers == ConsoleModifiers.Control)
                    break;

                if (key.Key == ConsoleKey.UpArrow)
                    selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : projects.Count - 1;
                else if (key.Key == ConsoleKey.DownArrow)
                    selectedIndex = (selectedIndex < projects.Count - 1) ? selectedIndex + 1 : 0;
                else if (key.Key == ConsoleKey.Enter)
                {
                    var selectedProject = projects[selectedIndex];
                    OpenProject(selectedProject);

                    projects = DirectoryManager.GetProjects();
                }
                else if (key.Key == ConsoleKey.N && key.Modifiers == ConsoleModifiers.Control)
                {
                    Console.Write(AppStrings.enterProjectName);
                    string? newFolderName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newFolderName))
                    {
                        string newFolderPath = Path.Combine(
                            Config.GetWorkDirectory().Value,
                            newFolderName.Trim()
                        );
                        if (!Directory.Exists(newFolderPath))
                        {
                            Directory.CreateDirectory(newFolderPath);
                            Console.WriteLine(AppStrings.projectCreatedSuccess);
                            Thread.Sleep(1000);
                            Console.Write(AppStrings.enterProjectType);
                            string? projectType = Console.ReadLine();
                            if (!string.IsNullOrEmpty(projectType))
                            {
                                string projectTypeFile = Path.Combine(
                                    newFolderPath,
                                    "project-type.txt"
                                );
                                File.WriteAllText(projectTypeFile, projectType.Trim());
                            }
                            projects = DirectoryManager.GetProjects();
                        }
                        else
                        {
                            Console.WriteLine(AppStrings.folderExists);
                            Thread.Sleep(2000);
                        }
                    }
                }
            }
        }

        private static void OpenProject(ProjectItem project)
        {
            Screen.Clear();
            Console.WriteLine(
                AppStrings.optionsForprojectHeader.Replace("{projectName}", project.Name)
            );
            Console.WriteLine(AppStrings.route.Replace("{projectPath}", project.Path));
            Console.WriteLine(AppStrings.openOnEditor);
            Console.WriteLine(AppStrings.setProjectType);
            Console.WriteLine(AppStrings.goBack);
            Console.WriteLine(AppStrings.openOnTerminal);
            Console.WriteLine(AppStrings.deleteProjectOption);

            var opcion = Console.ReadKey(true).Key;

            switch (opcion)
            {
                case ConsoleKey.D1:
                    var editors = GetAvailableEditors();
                    if (editors.Count == 0)
                    {
                        Console.WriteLine(AppStrings.editorsNotFound);
                        Thread.Sleep(2000);
                        break;
                    }

                    Console.WriteLine(AppStrings.selectEditor);
                    int i = 1;
                    var editorList = editors.ToList();
                    foreach (var e in editorList)
                    {
                        Console.WriteLine($"{i++}. {e.Key}");
                    }

                    Console.Write(AppStrings.actionCancel);

                    if (
                        int.TryParse(Console.ReadLine(), out int sel)
                        && sel > 0
                        && sel <= editorList.Count
                    )
                    {
                        string command = editorList[sel - 1].Value;
                        System.Diagnostics.Process.Start(
                            new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = command,
                                Arguments = $"\"{project.Path}\"",
                                UseShellExecute = true,
                            }
                        );
                    }
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Write(AppStrings.enterProjectType);
                    string? nuevoTipo = Console.ReadLine();
                    if (!string.IsNullOrEmpty(nuevoTipo))
                    {
                        string archivoTipo = Path.Combine(project.Path, "project-type.txt");
                        File.WriteAllText(archivoTipo, nuevoTipo.Trim());
                        Console.WriteLine(AppStrings.typeSettedSuccess);
                        Thread.Sleep(1000);
                    }
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    string terminalCommand = $"/K cd /d \"{project.Path}\"";

                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = terminalCommand,
                            UseShellExecute = true,
                        }
                    );

                    Environment.Exit(0);
                    break;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    Console.Write(AppStrings.deleteProject);
                    string? confirm = Console.ReadLine();
                    if (confirm != null && confirm?.ToLower() == "y")
                    {
                        try
                        {
                            SetAttributesNormal(new DirectoryInfo(project.Path));
                            Directory.Delete(project.Path, true);
                            Console.WriteLine(AppStrings.deleteProjectSuccess);
                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{AppStrings.errorOnDeleteProject} {ex.Message}");
                            Thread.Sleep(2000);
                        }
                    }
                    break;
            }
        }

        private static void SetAttributesNormal(DirectoryInfo path)
        {
            foreach (var file in path.GetFiles("*", SearchOption.AllDirectories))
                file.Attributes = FileAttributes.Normal;

            foreach (var dir in path.GetDirectories("*", SearchOption.AllDirectories))
                SetAttributesNormal(dir);
        }

        private static Dictionary<string, string> GetAvailableEditors()
        {
            var potentialEditors = new Dictionary<string, string>
            {
                { "Antigravity", "antigravity" },
                { "Visual Studio Code", "code" },
                { "Sublime Text", "subl" },
                { "Notepad++", "notepad++" },
                { "Vim", "vim" },
            };

            var installedEditors = new Dictionary<string, string>();

            foreach (var editor in potentialEditors)
            {
                if (CanRunCommand(editor.Value))
                {
                    installedEditors.Add(editor.Key, editor.Value);
                }
            }

            string vsPath = GetVisualStudioPath();
            if (!string.IsNullOrEmpty(vsPath))
            {
                installedEditors.Add("Visual Studio", vsPath);
            }

            return installedEditors;
        }

        private static bool CanRunCommand(string cmd)
        {
            try
            {
                using var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "where";
                process.StartInfo.Arguments = cmd;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private static string GetVisualStudioPath()
        {
            try
            {
                using var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "Microsoft Visual Studio",
                    "Installer",
                    "vswhere.exe"
                );

                process.StartInfo.Arguments = "-latest -property productPath";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd().Trim();
                return File.Exists(output) ? output : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

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
                Console.WriteLine("Bienvenido/a a Project Lens! :D");
                while (true)
                {
                    Console.WriteLine(
                        "[!] Para continuar debe establecer la dirección de su carpeta de proyectos"
                    );
                    Console.WriteLine(
                        "A continuación, ingrese la dirección (ej: C:/Usuario/Proyectos)"
                    );
                    string? dir = Console.ReadLine();

                    if (dir == null)
                    {
                        Console.WriteLine("[!] Debe ingresar al menos una letra.");
                    }
                    else
                    {
                        while (true)
                        {
                            Console.WriteLine(
                                $"Desea establecer [{dir}] como su dirección de proyectos? (y/n)"
                            );
                            string? option = Console.ReadLine();
                            if (option != null || option?.ToLower() == "y")
                            {
                                break;
                            }
                        }
                        Config.SetWorkDirectory(dir);
                        Console.WriteLine("Directorio establecido exitosamente!");
                        break;
                    }
                }
            }

            int selectedIndex = 0;
            var projects = DirectoryManager.GetProjects();

            while (true)
            {
                Screen.Clear();
                Console.WriteLine("----------ProjectLens v1.0----------");
                Console.WriteLine($"Directorio: {Config.GetWorkDirectory().Value}\n");

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

                Console.WriteLine(
                    "\n[Ctrl+Q] Salir | [Ctrl+N] Nueva carpeta | [Enter] Abrir | [Flechas Arriba/Abajo] Navegar"
                );

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
                    Console.Write("\nIngrese el nombre de la nueva carpeta de proyecto: ");
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
                            Console.WriteLine("Carpeta creada exitosamente.");
                            Thread.Sleep(1000);
                            Console.Write("Ingrese el tipo de proyecto: ");
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
                            Console.WriteLine("[!] Ya existe una carpeta con ese nombre.");
                            Thread.Sleep(2000);
                        }
                    }
                }
            }
        }

        private static void OpenProject(ProjectItem project)
        {
            Screen.Clear();
            Console.WriteLine($"--- OPCIONES PARA: {project.Name} ---");
            Console.WriteLine($"Ruta: {project.Path}\n");
            Console.WriteLine("1. Abrir en Editor de código");
            Console.WriteLine("2. Asignar/Cambiar tipo de proyecto");
            Console.WriteLine("3. Volver");
            Console.WriteLine("4 - Abrir una terminal en este directorio");
            Console.WriteLine("5 - [! Peligro] Eliminar proyecto");

            var opcion = Console.ReadKey(true).Key;

            switch (opcion)
            {
                case ConsoleKey.D1:
                    var editors = GetAvailableEditors();
                    if (editors.Count == 0)
                    {
                        Console.WriteLine("\n[!] No se detectaron editores conocidos en el PATH.");
                        Thread.Sleep(2000);
                        break;
                    }

                    Console.WriteLine("\nSeleccione un editor:");
                    int i = 1;
                    var editorList = editors.ToList();
                    foreach (var e in editorList)
                    {
                        Console.WriteLine($"{i++}. {e.Key}");
                    }

                    Console.Write(
                        "Presione enter para cancelar la acción o ingrese el numero del editor que desea usar: "
                    );

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
                    Console.Write("\nIngrese el nuevo tipo (ej: Python, React, C#): ");
                    string? nuevoTipo = Console.ReadLine();
                    if (!string.IsNullOrEmpty(nuevoTipo))
                    {
                        string archivoTipo = Path.Combine(project.Path, "project-type.txt");
                        File.WriteAllText(archivoTipo, nuevoTipo.Trim());
                        Console.WriteLine("Tipo asignado correctamente.");
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
                    Console.Write(
                        "\n[!] Está a punto de eliminar el proyecto. Esta acción no se puede deshacer. ¿Desea continuar? (y/n): "
                    );
                    string? confirm = Console.ReadLine();
                    if (confirm != null && confirm?.ToLower() == "y")
                    {
                        try
                        {
                            Directory.Delete(project.Path, true);
                            Console.WriteLine("Proyecto eliminado exitosamente.");
                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[!] Error al eliminar el proyecto: {ex.Message}");
                            Thread.Sleep(2000);
                        }
                    }
                    break;
            }
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

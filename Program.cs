using ProjectLens.config;
using ProjectLens.core;
using ProjectLens.utils;

namespace ProjectLens
{
    public class Program
    {
        public static void Main()
        {
            KeyValuePair<ActionResult, string> workdir = Config.GetWorkDirectory();

            if(workdir.Key == ActionResult.undefined)
            {
                Console.WriteLine(workdir.Value);
                Screen.Clear();
                Console.WriteLine("Bienvenido/a a Project Lens! :D");
                while (true)
                {
                    Console.WriteLine("[!] Para continuar debe establecer la dirección de su carpeta de proyectos");
                    Console.WriteLine("A continuación, ingrese la dirección (ej: C:/Usuario/Proyectos)");
                    string? dir = Console.ReadLine();

                    if(dir == null)
                    {
                        Console.WriteLine("[!] Debe ingresar al menos una letra.");
                    } else
                    {
                        while (true)
                        {
                            Console.WriteLine($"Desea establecer [{dir}] como su dirección de proyectos? (y/n)");
                            string? option = Console.ReadLine();
                            if(option != null || option?.ToLower() == "y")
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

            Screen.Clear();

            workdir = Config.GetWorkDirectory();

            Screen.Message($"Directorio de proyectos: {workdir}");
        }
    }
}

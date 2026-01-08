namespace ProjectLens.app
{
    public static class AppStrings {
        public const string cliHeader = $"----------ProjectLens v{AppDetails.appVersion}----------";
        public const string welcome = "Bienvenido/a a Project Lens! :D";
        public const string setWorkDir = "[!] Para continuar debe establecer la dirección de su carpeta de proyectos";
        public const string enterWorkDir = "A continuación, ingrese la dirección (ej: C:/Usuario/Proyectos)";
        public const string invalidDir = "[X] La dirección ingresada no es válida. Intente nuevamente.";
        public const string setDir = "Desea establecer [{dir}] como su dirección de proyectos? (y/n)";
        public const string dirSetSuccess = "[✓] Dirección de proyectos establecida con éxito.";
        public const string currentDir = "Su dirección de proyectos actual es: [{dir}]\n";
        public const string actions = "\n[Ctrl+Q] Salir | [Ctrl+N] Nueva carpeta | [Enter] Abrir | [Flechas Arriba/Abajo] Navegar";
        public const string enterProjectType = "\nIngrese el nuevo tipo (ej: Python, React, C#): ";
        public const string typeSettedSuccess = "[✓] Tipo de proyecto establecido con éxito.";
        public const string deleteProject = "\n[!] Está a punto de eliminar el proyecto. Esta acción no se puede deshacer. ¿Desea continuar? (y/n): ";
        public const string deleteProjectSuccess = "[✓] Proyecto eliminado con éxito.";
        public const string errorOnDeleteProject = "[X] Ocurrió un error al eliminar el proyecto:";
    }
}


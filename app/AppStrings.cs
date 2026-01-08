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
        public const string enterProjectType = "\nIngrese el nuevo tipo (ej: Python, React, C#) (no escriba nada y presione enter para cancelar): ";
        public const string typeSettedSuccess = "[✓] Tipo de proyecto establecido con éxito.";
        public const string deleteProject = "\n[!] Está a punto de eliminar el proyecto. Esta acción no se puede deshacer. ¿Desea continuar? (y/n): ";
        public const string deleteProjectSuccess = "[✓] Proyecto eliminado con éxito.";
        public const string errorOnDeleteProject = "[X] Ocurrió un error al eliminar el proyecto:";
        public const string enterProjectName = "\nIngrese el nombre de la nueva carpeta de proyecto (no escriba nada y presione enter para cancelar acción): ";
        public const string projectCreatedSuccess = "[✓] Nueva carpeta de proyecto creada con éxito.";
        public const string folderExists = "[X] Ya existe una carpeta con ese nombre. Intente nuevamente.";
        public const string optionsForprojectHeader = "--- OPCIONES PARA: {projectName} ---";
        public const string route = "Ruta: {projectPath}\n";

        public const string openOnEditor = "1 - Abrir en Editor de código";
        public const string setProjectType = "2 - Establecer/Modificar tipo de proyecto";
        public const string goBack = "3/Enter sin escribir nada - Volver atrás";
        public const string openOnTerminal = "4 - Abrir una terminal en este directorio";
        public const string deleteProjectOption = "5 - [! Peligro] Eliminar proyecto";

        public const string editorsNotFound = "\n[!] No se detectaron editores conocidos en el PATH.";
        public const string selectEditor = "\nSeleccione un editor para abrir el proyecto:";
        public const string actionCancel = "Presione enter para cancelar la acción o ingrese el numero del editor que desea usar: ";
    }
}


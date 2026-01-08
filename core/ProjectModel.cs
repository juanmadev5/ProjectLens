
namespace ProjectLens.core
{

    /// <summary>
    /// Describe el modelo de un proyecto.
    /// </summary>
    /// <param name="ProjectDir">Nombre de la carpeta del proyecto.</param>
    /// <param name="PrLanguage">Lenguaje de programación relacionado al proyecto.</param>
    public record ProjectModel(String ProjectDir, String PrLanguage);
}

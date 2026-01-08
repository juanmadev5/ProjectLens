using ProjectLens.core;

namespace ProjectLens.config
{
    public static class Config
    {
        public static KeyValuePair<ActionResult, string> GetWorkDirectory()
        {
            try
            {
                var workDir = File.ReadAllText("config.txt");
                if (workDir == null)
                {
                    return new KeyValuePair<ActionResult, string>(
                        ActionResult.undefined,
                        "work directory is not defined"
                    );
                }
                return new KeyValuePair<ActionResult, string>(ActionResult.success, workDir);
            }
            catch (FileNotFoundException exception)
            {
                return new KeyValuePair<ActionResult, string>(
                    ActionResult.success,
                    exception.Message
                );
            }
        }

        public static void SetWorkDirectory(string directory)
        {
            File.WriteAllText("config.txt", $"{directory}");
        }
    }
}

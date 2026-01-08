namespace ProjectLens.utils
{
    public static class Screen
    {
        public static void Clear()
        {
            System.Console.Clear();
        }

        public static void Message(string message) { 
            Console.Write(message);
        }
    }
}

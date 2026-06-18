using System.Reflection;

namespace Chronos.Classes
{
    public static class Config
    {
        public static readonly string? AppName =
            Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly string AppDescription =
            "A simple yet powerful text editor.";

        public static readonly string? AppVersion =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        public const string GitHubRepo =
            "https://github.com/nam3lol/Chronos";
    }
}

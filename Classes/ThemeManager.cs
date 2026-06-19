using Chronos.Properties;
using System.Windows;

namespace Chronos.Classes
{
    public static class ThemeManager
    {
        private static string _currentTheme = Settings.Default.Theme;

        public static string CurrentTheme 
            => _currentTheme;

        public static event Action? ThemeChanged;

        public static void SetTheme(string themeName)
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;

            for (int i = dictionaries.Count - 1; i >= 0; i--)
            {
                var src = dictionaries[i].Source?.OriginalString;

                if (src != null && src.Contains("/Themes/"))
                    dictionaries.RemoveAt(i);
            }

            var dict = new ResourceDictionary
            {
                Source = new Uri($"Resources/Themes/{themeName}.xaml", UriKind.Relative)
            };

            dictionaries.Add(dict);

            _currentTheme = themeName;
            ThemeChanged?.Invoke();
        }
    }
}

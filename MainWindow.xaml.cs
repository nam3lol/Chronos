using Chronos.Properties;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chronos
{
    public partial class MainWindow : Window
    {
        public bool LoadSavedEditorContent { get; set; } = true;

        Geometry? geometry_Maximize =
            Application.Current.TryFindResource("Maximize") as Geometry;

        Geometry? geometry_Restore =
            Application.Current.TryFindResource("Restore") as Geometry;

        Geometry? geometry_SidebarEnabled =
            Application.Current.TryFindResource("Sidebar_Enabled") as Geometry;

        Geometry? geometry_SidebarDisabled =
            Application.Current.TryFindResource("Sidebar_Disabled") as Geometry;

        Geometry? geometry_BottombarEnabled =
            Application.Current.TryFindResource("Bottombar_Enabled") as Geometry;

        Geometry? geometry_BottombarDisabled =
            Application.Current.TryFindResource("Bottombar_Disabled") as Geometry;

        public MainWindow() =>
            InitializeComponent();

        public void LoadSettings()
        {
            ToggleSidebar(Settings.Default.Sidebar);
            ToggleBottombar(Settings.Default.Bottombar);

            Topmost = Settings.Default.Topmost;

            avalonEdit_TextEditor.FontFamily = new FontFamily(Settings.Default.EditorFont);
        }

        public void SaveSettings()
        {
            Settings.Default.Sidebar = border_Sidebar.Visibility == Visibility.Visible;
            Settings.Default.Bottombar = border_Bottombar.Visibility == Visibility.Visible;
            Settings.Default.Topmost = Topmost;
            Settings.Default.EditorFont = avalonEdit_TextEditor.FontFamily.Source;
            Settings.Default.SavedEditorContent = avalonEdit_TextEditor.Text;
            Settings.Default.Save();
        }

        public void ToggleMaximized(bool maximized)
        {
            if (maximized)
            {
                WindowState = WindowState.Maximized;
                path_Maximize.Data = geometry_Restore;
            }
            else
            {
                WindowState = WindowState.Normal;
                path_Maximize.Data = geometry_Maximize;
            }
        }
        public void ToggleSidebar(bool enabled)
        {
            if (enabled)
            {
                border_Sidebar.Visibility = Visibility.Visible;
                path_Sidebar.Data = geometry_SidebarEnabled;
                Settings.Default.Sidebar = true;
            }
            else
            {
                border_Sidebar.Visibility = Visibility.Collapsed;
                path_Sidebar.Data = geometry_SidebarDisabled;
                Settings.Default.Sidebar = false;
            }
        }

        public void ToggleBottombar(bool enabled)
        {
            if (enabled)
            {
                border_Bottombar.Visibility = Visibility.Visible;
                path_Bottombar.Data = geometry_BottombarEnabled;
                Settings.Default.Bottombar = true;
            }
            else
            {
                border_Bottombar.Visibility = Visibility.Collapsed;
                path_Bottombar.Data = geometry_BottombarDisabled;
                Settings.Default.Bottombar = false;
            }
        }

        private void TextEditor_TextChanged(object? sender, EventArgs e)
        {
            UpdateUndoRedoState();
        }

        private void Drag(object sender, MouseButtonEventArgs e) => DragMove();

        private void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Chronos - Open File";
            dialog.Filter =
                "Normal Text File (*.txt;)|*.txt|" +
                "Markdown File (*.md;*.markdown)|*.md;*.markdown|" +
                "All Files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;

                string contents = File.ReadAllText(filePath);

                avalonEdit_TextEditor.Text = contents;
            }
        }

        private void SaveFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Title = "Chronos - Save File";
            dialog.Filter =
                "Normal Text File (*.txt;)|*.txt|" +
                "Markdown File (*.md;*.markdown)|*.md;*.markdown|" +
                "All Files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;

                string contents = File.ReadAllText(filePath);

                File.WriteAllText(filePath, contents);
            }
        }

        private void UpdateUndoRedoState()
        {
            bool canUndo = avalonEdit_TextEditor.CanUndo;
            bool canRedo = avalonEdit_TextEditor.CanRedo;

            button_Undo.IsEnabled = canUndo;
            button_Redo.IsEnabled = canRedo;

            menuItem_Undo.IsEnabled = canUndo;
            menuItem_Redo.IsEnabled = canRedo;
        }

        private async void buttonClick_WindowControls(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "button_Close":
                        SaveSettings();
                        AnimLib.Fade(this, 1, 0, 500);
                        await Task.Delay(500);
                        Close();
                        break;
                    case "button_Maximize":
                        ToggleMaximized(WindowState != WindowState.Maximized);
                        break;
                    case "button_Minimize":
                        WindowState = WindowState.Minimized;
                        break;
                }
            }
        }

        private void buttonClick_ToolbarControlsRight(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "button_ToggleSidebar":
                        if (border_Sidebar.Visibility == Visibility.Visible)
                            ToggleSidebar(false);
                        else
                            ToggleSidebar(true);
                        break;
                    case "button_ToggleBottombar":
                        if (border_Bottombar.Visibility == Visibility.Visible) 
                            ToggleBottombar(false);
                        else
                            ToggleBottombar(true);
                        break;
                    case "button_Settings":
                        MessageBoxResult result = MessageBox.Show(
                            "Toggle Topmost?",
                            "Chronos",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        bool yes = result == MessageBoxResult.Yes;

                        if (yes)
                        {
                            Topmost = !Topmost;
                        }
                        break;
                }
            }
        }

        private void buttonClick_ToolbarControlsLeft(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "button_OpenFile":
                        OpenFile();
                        break;
                    case "button_SaveFile":
                        SaveFile();
                        break;
                    case "button_Cut":
                        avalonEdit_TextEditor.Cut();
                        break;
                    case "button_Copy":
                        avalonEdit_TextEditor.Copy();
                        break;
                    case "button_Paste":
                        avalonEdit_TextEditor.Paste();
                        break;
                    case "button_Delete":
                        avalonEdit_TextEditor.Delete();
                        break;
                    case "button_Undo":
                        avalonEdit_TextEditor.Undo();
                        UpdateUndoRedoState();
                        break;
                    case "button_Redo":
                        avalonEdit_TextEditor.Redo();
                        UpdateUndoRedoState();
                        break;
                }
            }
        }

        private void menuItemClick_File(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Name)
                {
                    case "menuItem_NewWindow":
                        SaveSettings();

                        var window = new MainWindow
                        {
                            LoadSavedEditorContent = false
                        };

                        window.Show();
                        break;
                    case "menuItem_OpenFile":
                        OpenFile();
                        break;
                    case "menuItem_SaveFile":
                        SaveFile();
                        break;
                    case "menuItem_Exit":
                        Application.Current.Shutdown();
                        break;
                }
            }
        }
        private void menuItemClick_Edit(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Name)
                {
                    case "menuItem_Cut":
                        avalonEdit_TextEditor.Cut();
                        break;
                    case "menuItem_Copy":
                        avalonEdit_TextEditor.Copy();
                        break;
                    case "menuItem_Paste":
                        avalonEdit_TextEditor.Paste();
                        break;
                    case "menuItem_Delete":
                        avalonEdit_TextEditor.Delete();
                        break;
                    case "menuItem_Undo":
                        avalonEdit_TextEditor.Undo();
                        UpdateUndoRedoState();
                        break;
                    case "menuItem_Redo":
                        avalonEdit_TextEditor.Redo();
                        UpdateUndoRedoState();
                        break;
                    case "menuItem_SearchOnline":

                        string query = avalonEdit_TextEditor.SelectedText;
                        string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });

                        break;
                }
            }
        }
        private void menuItemClick_View(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Name)
                {
                    case "menuItem_ToggleSidebar":
                        if (border_Sidebar.Visibility == Visibility.Visible)
                        {
                            ToggleSidebar(false);
                        }
                        else
                        {
                            ToggleSidebar(true);
                        }
                        break;
                    case "menuItem_ToggleBottombar":
                        if (border_Bottombar.Visibility == Visibility.Visible)
                        {
                            ToggleBottombar(false);
                        }
                        else
                        {
                            ToggleBottombar(true);
                        }
                        break;
                }
            }
        }

        public async Task StartSplashAnimAsync()
        {
            grid_Splash.Margin = new Thickness(0, 0, 0, 0);
            AnimLib.Fade(this, 0, 1, 500);
            await Task.Delay(1000);
            AnimLib.Fade(grid_Splash, 0, 1, 500);

            await Task.Delay(2500); // loading code goes here

            AnimLib.Fade(grid_Splash, 1, 0, 500);
            await Task.Delay(1000);
            grid_BorderGrid.Visibility = Visibility.Visible;
            AnimLib.Fade(grid_BorderGrid, 0, 1, 500);

            grid_Splash.Visibility = Visibility.Collapsed;
            grid_Splash.Margin = new Thickness(999, 999, 999, 999);
        }

        private void InitializeUIState()
        {
            Opacity = 0;
            grid_BorderGrid.Opacity = 0;
            grid_BorderGrid.Visibility = Visibility.Collapsed;
            grid_Splash.Opacity = 0;
        }

        private void InitializeEditor()
        {
            avalonEdit_TextEditor.TextChanged += TextEditor_TextChanged;

            if (LoadSavedEditorContent)
                avalonEdit_TextEditor.Text = Settings.Default.SavedEditorContent;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeUIState();

            await StartSplashAnimAsync();

            InitializeEditor();
            LoadSettings();

            UpdateUndoRedoState();
        }
    }
}
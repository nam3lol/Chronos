using Chronos.Properties;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chronos
{
    public partial class MainWindow : Window
    {
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

        public MainWindow()
        {
            InitializeComponent();
            Opacity = 0;
            grid_BorderGrid.Opacity = 0;
            grid_BorderGrid.Visibility = Visibility.Collapsed;
            grid_Splash.Opacity = 0;

            SplashAnimation();

            UpdateUndoRedoButtons();
            avalonEdit_TextEditor.TextChanged += TextEditor_TextChanged;
        }

        public void LoadSettings()
        {
            ToggleSidebar(Settings.Default.Sidebar);
            ToggleBottombar(Settings.Default.Bottombar);
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
            UpdateUndoRedoButtons();
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

        private void UpdateUndoRedoButtons()
        {
            button_Undo.IsEnabled = avalonEdit_TextEditor.CanUndo;
            button_Redo.IsEnabled = avalonEdit_TextEditor.CanRedo;

            menuItem_Undo.IsEnabled = avalonEdit_TextEditor.CanUndo;
            menuItem_Redo.IsEnabled = avalonEdit_TextEditor.CanRedo;
        }

        private void buttonClick_WindowControls(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "button_Close":
                        Close();
                        break;
                    case "button_Maximize":
                        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                        path_Maximize.Data = WindowState == WindowState.Maximized ? Geometry.Parse(geometry_Restore?.ToString()) : Geometry.Parse(geometry_Maximize?.ToString());
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
                        {
                            ToggleSidebar(false);
                        }
                        else
                        {
                            ToggleSidebar(true);
                        }
                        break;
                    case "button_ToggleBottombar":
                        if (border_Bottombar.Visibility == Visibility.Visible)
                        {
                            ToggleBottombar(false);
                        }
                        else
                        {
                            ToggleBottombar(true);
                        }
                        break;
                    case "button_Settings":
                        MessageBox.Show("settings go BRRRR");
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
                        UpdateUndoRedoButtons();
                        break;
                    case "button_Redo":
                        avalonEdit_TextEditor.Redo();
                        UpdateUndoRedoButtons();
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
                        MainWindow window = new MainWindow();
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
                        UpdateUndoRedoButtons();
                        break;
                    case "menuItem_Redo":
                        avalonEdit_TextEditor.Redo();
                        UpdateUndoRedoButtons();
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

        public async void SplashAnimation()
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
    }
}
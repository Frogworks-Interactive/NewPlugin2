using Dalamud.Interface.ImGuiFileDialog;
using ImGuiNET;
using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace OofPlugin
{
    partial class PluginUI : IDisposable
    {
        private Configuration configuration;
        private FileDialogManager manager { get; }

        private bool settingsVisible = false;

        public bool SettingsVisible
        {
            get { return settingsVisible; }
            set { settingsVisible = value; }
        }

        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
            manager = new FileDialogManager
            {
                AddedWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking,
            };
        }

        public void Dispose()
        {
        }

        public void Draw()
        {
            if (SettingsVisible) DrawSettingsWindow();
        }


        public void DrawSettingsWindow()
        {

            ImGui.SetNextWindowSize(new Vector2(400, 340), ImGuiCond.Always);
            if (ImGui.Begin("oof options", ref settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                var SliderFloat = configuration.SliderFloat;

                if (ImGui.SliderFloat("Volume", ref SliderFloat, 0.0f, 1.0f))
                {
                    configuration.SliderFloat = SliderFloat;
                    configuration.Save();
                }
                ImGui.Separator();

                var BooleanValue = configuration.BooleanValue;
                if (ImGui.Checkbox("Automatically sort on Inventory Open", ref BooleanValue))
                {
                    configuration.BooleanValue = BooleanValue;
                    configuration.Save();
                }

                // filedialog example


                if (configuration.FileImportPath.Length > 0)
                {
                    var formatString = FileNameRegex().Match(configuration.FileImportPath);
                    if (formatString.Success) ImGui.TextUnformatted(formatString.Value);
                }
                else ImGui.TextUnformatted("no file selected");


                if (ImGui.Button("Browse file"))
                {
                    void UpdatePath(bool success, string path)
                    {
                        if (!success || path.Length == 0) return;
                        configuration.FileImportPath = path;
                        configuration.Save();
                    }
                    //change filter for all files
                    manager.OpenFileDialog("Open Image...", "Audio{.wav}, All Files{*}", UpdatePath);
                }
                manager.Draw();
            }
            ImGui.End();
        }
        // Set up the file selector with the right flags and custom side bar items.
        public static FileDialogManager SetupFileManager()
        {
            var fileManager = new FileDialogManager
            {
                AddedWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking,
            };

            // Remove Videos and Music.
            fileManager.CustomSideBarItems.Add(("Videos", string.Empty, 0, -1));
            fileManager.CustomSideBarItems.Add(("Music", string.Empty, 0, -1));

            return fileManager;
        }

        //regex to get filename out of path name

        [GeneratedRegex("[^\\\\]+$")]
        private static partial Regex FileNameRegex();
    }
}

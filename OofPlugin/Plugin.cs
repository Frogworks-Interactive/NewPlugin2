using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;

//shoutout anna clemens

namespace OofPlugin
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "NewPlugin2";

        private const string commandSettings = "/testcommand";

        //example how to inject services
        [PluginService] public static Framework Framework { get; private set; } = null!;
        [PluginService] public static ClientState ClientState { get; private set; } = null!;

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        private Configuration Configuration { get; init; }
        private PluginUI PluginUi { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            PluginInterface = pluginInterface;
            CommandManager = commandManager;

            Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(PluginInterface);
            PluginUi = new PluginUI(Configuration);

            CommandManager.AddHandler(commandSettings, new CommandInfo(OnCommand)
            {
                HelpMessage = "play command"
            });
            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            Framework.Update += FrameworkOnUpdate;
        }

        private void OnCommand(string command, string args)
        {
            if (command == commandSettings) PluginUi.SettingsVisible = true;

        }

        private void DrawUI()
        {
            PluginUi.Draw();
        }

        private void DrawConfigUI()
        {
            PluginUi.SettingsVisible = true;
        }
        private void FrameworkOnUpdate(Framework framework)
        {
            // do stuff on frameworkupdate
        }


        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
            PluginUi.Dispose();
            CommandManager.RemoveHandler(commandSettings);
            Framework.Update -= FrameworkOnUpdate;
        }


    }
}

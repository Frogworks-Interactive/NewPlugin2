using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace OofPlugin
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;
        public bool BooleanValue { get; set; } = true;
        public float SliderFloat { get; set; } = 0.5f;
        public string FileImportPath { get; set; } = string.Empty;
        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;
        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }
        public void Save()
        {
            this.pluginInterface!.SavePluginConfig(this);
        }
    }
}

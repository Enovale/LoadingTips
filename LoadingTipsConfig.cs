using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using LethalConfig;
using LethalConfig.ConfigItems;

namespace LoadingTips
{
    internal class LoadingTipsConfig
    {
        public ConfigEntry<string> TipPrefix { get; }
        public ConfigEntry<bool> RandomizeTip { get; }

        public IReadOnlyList<string> Tips => _tips.AsReadOnly();
        private readonly List<string> _tips = [];
        
        private readonly string _tipsPath = Path.Combine(Paths.ConfigPath, nameof(LoadingTips) + ".txt");

        private const string _defaultTip =
            $"Create tips by editing the {nameof(LoadingTips)}.txt file in your BepInEx config directory.";
        
        public LoadingTipsConfig(ConfigFile config)
        {
            TipPrefix = config.Bind("General", nameof(TipPrefix), string.Empty,
                "This text will precede all loading screen tips (e.g. TIP: My Tip Here)");
            RandomizeTip = config.Bind("General", nameof(RandomizeTip), true,
                "Whether to show a random tip from the list every time it is shown or show them sequentially.");
            
            LoadTips();
        }
        
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal void InitializeLethalConfig()
        {
            var tipPrefixField = new TextInputFieldConfigItem(TipPrefix, false);
            LethalConfigManager.AddConfigItem(tipPrefixField);
            var randomizeTipField = new BoolCheckBoxConfigItem(RandomizeTip, false);
            LethalConfigManager.AddConfigItem(randomizeTipField);
            var reloadTipsButton = new GenericButtonConfigItem("General", "Reload Tips",
                "Reloads the tips from the txt file in BepInEx/config/LoadingTips.txt", "Reload", LoadTips);
            LethalConfigManager.AddConfigItem(reloadTipsButton);
        }

        public void LoadTips()
        {
            _tips.Clear();

            try
            {
                if (!File.Exists(_tipsPath))
                    File.WriteAllText(_tipsPath, _defaultTip, Encoding.UTF8);
            }
            catch (Exception e)
            {
                LoadingTips.Logger.LogError($"Failed to create tips file: {e}");
                return;
            }

            try
            {
                var rawLines = File.ReadAllLines(_tipsPath);
                _tips.AddRange(rawLines.Select(l => l.Trim().Split("##").First().Replace("\\n", Environment.NewLine)).Where(l => !string.IsNullOrWhiteSpace(l)));
            }
            catch (Exception e)
            {
                LoadingTips.Logger.LogError($"Failed to read tips file: {e}");
                return;
            }
        }
    }
}
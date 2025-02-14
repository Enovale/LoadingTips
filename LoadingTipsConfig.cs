using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using BepInEx;
using BepInEx.Configuration;

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
                _tips.AddRange(File.ReadAllLines(_tipsPath, Encoding.UTF8));
            }
            catch (Exception e)
            {
                LoadingTips.Logger.LogError($"Failed to read tips file: {e}");
                return;
            }
        }
    }
}
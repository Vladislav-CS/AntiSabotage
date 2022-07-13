using System;
using System.Collections.Generic;
using AntiSabotage.Resources;
using Exiled.API.Features;
using HarmonyLib;
using Player = Exiled.Events.Handlers.Player;

namespace AntiSabotage
{
    public sealed class Plugin : Plugin<Config, Translations>
    {
        public override string Name => "AntiSabotage";

        public override string Author => "Ficus-x";

        public override string Prefix => Name;

        public override Version Version { get; } = new(1, 0, 0);

        public override Version RequiredExiledVersion { get; } = new(5, 0, 0);

        public static Plugin Instance { get; private set; }

        private Handler _handler;
        
        private Harmony _harmony;

        public readonly List<string> Abusers = new();

        public override void OnEnabled()
        {
            if (string.IsNullOrEmpty(Config.WebhookUrl))
            {
                Log.Error("The webhook URL must be set for this plugin to launch!");
                return;
            }
            
            Instance = this;

            _harmony = new Harmony($"AntiSabotage.{DateTime.Now.Ticks}");
            _harmony.PatchAll();

            RegisterEvents();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            
            _harmony.UnpatchAll();
            _handler = null;
            
            UnregisterEvents();
            
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _handler = new Handler();

            Player.ChangingGroup += _handler.OnChangingGroup;
        }

        private void UnregisterEvents()
        {
            Player.ChangingGroup -= _handler.OnChangingGroup;

            _handler = null;
        }
    }
}

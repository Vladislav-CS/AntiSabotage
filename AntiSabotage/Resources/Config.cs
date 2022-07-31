using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace AntiSabotage.Resources
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("How many seconds will be the period of watching bans after starting banning")]
        public float BanTimeout { get; set; } = 20f;
        
        [Description("How many bans the administator can give. If they ban more that it their badge will be removed")]
        public int BanLimit { get; set; } = 5;
        
        [Description("How many seconds will be the period of watching kicks after starting kicking")]
        public float KickTimeout { get; set; } = 20f;
        
        [Description("How many kicks the administator can give. If they kick more that it their badge will be removed")]
        public float KickLimit { get; set; } = 5;
        
        [Description("URL of webhook")]
        public string WebhookUrl { get; set; } = string.Empty;

        [Description("Roles or users to be pinged when sabotaging or using notified command")]
        public List<string> PingedPeople { get; set; } = new ()
        {
            "<@&981638149572821062>", "<@510752968551825409>"
        };
        
        [Description("Commands to be notified using webhook")]
        public List<string> NotifiedCommands { get; set; } = new ()
        {
            "noclip"
        };

        public bool ShouldNotifiedCommandPingedPeople { get; set; } = true;
        
        [Description("Note that the colour has to start with #")]
        public string EmbedColour { get; set; } = "#ff4040";
    }
}

using Exiled.API.Interfaces;

namespace AntiSabotage.Resources
{
    public sealed class Translations : ITranslation
    {
        public string NotifiedBroadcast { get; set; } = "<color=red>Your badge have been removed from sabotaging this server. Please contact with your manager.</color>";
        
        public string AttentionText { get; set; } =
            "The administrator tried to sabotage the server. To give the badge back you need to execute command: unblock %AdminID% in Remote Admin.";

        public string NotifiedCommandText { get; set; } = "The administrator used the notified command.";

        public string EmbedTitle { get; set; } = "Attention!";
        
        public string Port { get; set; } = "Port";

        public string Command { get; set; } = "Command";

        public string Administrator { get; set; } = "Administrator";

        public string AmountAffected { get; set; } = "Amount of affected";
    }
}
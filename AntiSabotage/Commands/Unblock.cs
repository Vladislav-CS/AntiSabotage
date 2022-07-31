using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace AntiSabotage.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public sealed class Unblock : ICommand
    {
        public string Command => "Unblock";
        
        public string[] Aliases { get; } = Array.Empty<string>();
        
        public string Description => "Unblocks the administrator from removing their badge.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("asb.unblock"))
            {
                response = "You cannot use this command. Required permission: asb.unblock";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: unblock steamID@steam";
                return false;
            }

            if (!Plugin.Instance.Abusers.Contains(arguments.ElementAt(0)))
            {
                response = "This administrator is not in blacklist.";
                return false;
            }

            Plugin.Instance.Abusers.Remove(arguments.ElementAt(0));

            response = "This administrator was removed from blacklist.";
            return true;
        }
    }
}
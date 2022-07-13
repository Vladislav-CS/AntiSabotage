using System;
using System.Collections.Generic;
using System.Linq;
using DSharp4Webhook.Core;
using DSharp4Webhook.Core.Constructor;
using DSharp4Webhook.Util;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using MEC;
using RemoteAdmin;

namespace AntiSabotage.Patches
{
    [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
    public static class RemoteAdminCommand
    {
        public static readonly IWebhook Webhook = WebhookProvider.CreateStaticWebhook(Plugin.Instance.Config.WebhookUrl);
        
        public static readonly EmbedBuilder EmbedBuilder = ConstructorProvider.GetEmbedBuilder();
        public static readonly EmbedFieldBuilder FieldBuilder = ConstructorProvider.GetEmbedFieldBuilder();
        public static readonly MessageBuilder MessageBuilder = ConstructorProvider.GetMessageBuilder();
        public static readonly MessageMentionBuilder MentionBuilder = ConstructorProvider.GetMentionBuilder();

        public static readonly Dictionary<string, byte> BanCounter = new();
        public static readonly Dictionary<string, byte> KickCounter = new();

        public static bool Prefix(string q, CommandSender sender)
        {
            string[] query = q.Split(' ');
            Player commandSender = Player.Get(sender);
            
            CheckNotifiedCommand(commandSender, query);
            
            return CheckBans(query, commandSender) && CheckKicks(query, commandSender);
        }

        public static bool CheckBans(string[] query, Player sender)
        {
            if (!query[0].ToLower().Equals("ban") || query[2].Equals("0"))
                return true;

            if (sender == null || sender.AuthenticationType == AuthenticationType.Northwood)
                return true;
            
            int banCount = query[1].Split('.').Length - 1;

            if (!BanCounter.ContainsKey(sender.UserId))
                BanCounter.Add(sender.UserId, 0);

            BanCounter[sender.UserId] += (byte) banCount;
            
            Timing.CallDelayed(Plugin.Instance.Config.BanTimeout, () => { BanCounter[sender.UserId] = 0; });
            
            if (BanCounter[sender.UserId] > Plugin.Instance.Config.BanLimit)
            {
                sender.Group = new UserGroup();
                Plugin.Instance.Abusers.Add(sender.UserId);
                
                sender.Broadcast(15, Plugin.Instance.Translation.NotifiedBroadcast, shouldClearPrevious: true);
                Webhook.SendMessage(PrepareMessageForSabotaging(sender, "ban", BanCounter[sender.UserId])).Queue((_, isSuccessful) =>
                {
                    if (!isSuccessful)
                        Log.Error("Error while sending a message");
                });
                return false;
            }

            return true;
        }

        public static bool CheckKicks(string[] query, Player sender)
        {
            if (!query[0].ToLower().Equals("ban") || !query[2].Equals("0"))
                return true;

            if (sender == null || sender.AuthenticationType == AuthenticationType.Northwood)
                return true;
            
            int kickCount = query[1].Split('.').Length - 1;

            if (!KickCounter.ContainsKey(sender.UserId))
                KickCounter.Add(sender.UserId, 0);

            KickCounter[sender.UserId] += (byte) kickCount;

            Timing.CallDelayed(Plugin.Instance.Config.KickTimeout, () => { KickCounter[sender.UserId] = 0; });
            
            if (KickCounter[sender.UserId] > Plugin.Instance.Config.KickLimit)
            {
                sender.Group = new UserGroup();
                Plugin.Instance.Abusers.Add(sender.UserId);
                
                sender.Broadcast(15, Plugin.Instance.Translation.NotifiedBroadcast, shouldClearPrevious: true);
                Webhook.SendMessage(PrepareMessageForSabotaging(sender, "kick", KickCounter[sender.UserId])).Queue((_, isSuccessful) =>
                {
                    if (!isSuccessful)
                        Log.Error("Error while sending a message");
                });
                return false;
            }
            return true;
        }

        public static void CheckNotifiedCommand(Player sender, string[] command)
        {
            if (Plugin.Instance.Config.NotifiedCommands.Select(notifiedCommand => notifiedCommand.ToLower()).Contains(command[0].ToLower()))
                Webhook.SendMessage(PrepareMessageForNotifiedCommand(sender, command)).Queue((_, isSuccessful) =>
                {
                    if (!isSuccessful)
                        Log.Error("Error while sending a message");
                });
        }
        
        public static IMessage PrepareMessageForSabotaging(Player sender, string command, int amount)
        {
            ResetBuilders();

            FieldBuilder.Inline = false;
            
            FieldBuilder.Name = Plugin.Instance.Translation.Administrator;
            FieldBuilder.Value = $"{sender.Nickname} ({sender.UserId})";
            EmbedBuilder.AddField(FieldBuilder.Build());
            
            FieldBuilder.Name = Plugin.Instance.Translation.Port;
            FieldBuilder.Value = Server.Port.ToString();
            EmbedBuilder.AddField(FieldBuilder.Build());
            
            FieldBuilder.Name = Plugin.Instance.Translation.Command;
            FieldBuilder.Value = command;
            EmbedBuilder.AddField(FieldBuilder.Build());
            
            FieldBuilder.Name = Plugin.Instance.Translation.AmountAffected;
            FieldBuilder.Value = amount.ToString();
            EmbedBuilder.AddField(FieldBuilder.Build());
            
            EmbedBuilder.Title = Plugin.Instance.Translation.EmbedTitle;
            EmbedBuilder.Append(Plugin.Instance.Translation.AttentionText.Replace("%AdminID%", sender.UserId));

            EmbedBuilder.Color = (uint?) ColorUtil.FromHex(Plugin.Instance.Config.EmbedColour);
            EmbedBuilder.Timestamp = DateTimeOffset.UtcNow;

            MessageBuilder.Append(string.Join(", ", Plugin.Instance.Config.PingedPeople));
            MessageBuilder.AddEmbed(EmbedBuilder.Build());
            
            MentionBuilder.Roles = Plugin.Instance.Config.PingedPeople.ToHashSet();
            MentionBuilder.AllowedMention = AllowedMention.ROLES | AllowedMention.USERS;
            
            MessageBuilder.SetMessageMention(MentionBuilder.Build());
            return MessageBuilder.Build();
        }
        
        public static IMessage PrepareMessageForNotifiedCommand(Player sender, string[] command)
        {
            ResetBuilders();
            
            FieldBuilder.Inline = false;
            
            FieldBuilder.Name = Plugin.Instance.Translation.Administrator;
            FieldBuilder.Value = $"{sender.Nickname} ({sender.UserId})";
            EmbedBuilder.AddField(FieldBuilder.Build());
            
            FieldBuilder.Name = Plugin.Instance.Translation.Command;
            FieldBuilder.Value = $"```{string.Join(" ", command)}```";
            EmbedBuilder.AddField(FieldBuilder.Build());
            
            FieldBuilder.Name = Plugin.Instance.Translation.Port;
            FieldBuilder.Value = Server.Port.ToString();
            EmbedBuilder.AddField(FieldBuilder.Build());
            
            EmbedBuilder.Title = Plugin.Instance.Translation.EmbedTitle;
            EmbedBuilder.Append(Plugin.Instance.Translation.NotifiedCommandText);

            EmbedBuilder.Color = (uint?) ColorUtil.FromHex(Plugin.Instance.Config.EmbedColour);
            EmbedBuilder.Timestamp = DateTimeOffset.UtcNow;
            
            if (Plugin.Instance.Config.ShouldNotifiedCommandPingedPeople)
                MessageBuilder.Append(string.Join(", ", Plugin.Instance.Config.PingedPeople));
            
            MessageBuilder.AddEmbed(EmbedBuilder.Build());
            
            MentionBuilder.Roles = Plugin.Instance.Config.PingedPeople.ToHashSet();
            MentionBuilder.AllowedMention = AllowedMention.ROLES | AllowedMention.USERS;
            
            MessageBuilder.SetMessageMention(MentionBuilder.Build());
            return MessageBuilder.Build();
        }

        public static void ResetBuilders()
        {
            EmbedBuilder.Reset();
            FieldBuilder.Reset();
            MessageBuilder.Reset();
            MentionBuilder.Reset();
        }
    }
}
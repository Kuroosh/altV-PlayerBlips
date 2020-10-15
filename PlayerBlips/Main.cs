using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace PlayerBlips
{
    public class PlayerBlipResource : Resource
    {
        public override IEntityFactory<IPlayer> GetPlayerFactory() { return new MyPlayerFactory(); }
        public override void OnStart() { }
        public override void OnStop() { }

    }
    public class PlayerBlipSettingsModel
    {
        public bool RandomColor { get; set; }
        public int StandardColor { get; set; }
        public int UpdateInterval { get; set; }
    }
    public class Main : IScript
    {
        private static readonly string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/settings.json");
        private static readonly PlayerBlipSettingsModel SettingsModel = JsonSerializer.Deserialize<PlayerBlipSettingsModel>(jsonString);
        private static readonly bool RandomColor = SettingsModel.RandomColor;
        private static readonly int UpdateInterval = SettingsModel.UpdateInterval;
        public static readonly int StandardColor = SettingsModel.StandardColor;

        private static readonly List<int> RandomBlipIntegers = new List<int>
        {
            { 1 },
            { 2 },
            { 3 },
            { 5 },
            { 8 },
            { 9 },
            { 27 },
            { 28 },
            { 47 },
            { 48 },
            { 49 },
            { 76 },
            { 77 },
            { 83 },
        };
        private static int GetPlayerColor(PlayerBlipEntity player)
        {
            // If Random Color activated
            if (RandomColor)
            {
                if (player.Color != StandardColor)
                    return player.Color;
                // Get Random Color from our list.
                Random random = new Random();
                player.Color = random.Next(0, RandomBlipIntegers.Count);
                return player.Color;
            }
            return StandardColor;
        }
        private static void OnPlayerBlipTick(Object unused)
        {
            try
            {
                foreach (PlayerBlipEntity player in Alt.GetAllPlayers().ToList())
                {
                    foreach (PlayerBlipEntity otherplayers in Alt.GetAllPlayers().ToList())
                    {
                        if (player is null || !player.Exists || otherplayers is null || !otherplayers.Exists || player == otherplayers) continue;
                        Alt.Server.TriggerClientEvent(player, "PlayerBlips:Update", otherplayers.Id, otherplayers.Position, GetPlayerColor(otherplayers), otherplayers.Name);
                    }
                }
            }
            catch { }
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public static void OnPlayerDisconnected(PlayerBlipEntity player, string reason)
        {
            try
            {
                Alt.EmitAllClients("PlayerBlips:Delete", player.Id);
            }
            catch { }
        }

        private static Timer UpdateIntervalTimer = new Timer(OnPlayerBlipTick, null, UpdateInterval, UpdateInterval);
    }
}

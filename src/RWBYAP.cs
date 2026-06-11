using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Roost;
using RwbyAP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RwbyAP;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class RWBYAP : BaseUnityPlugin
{
    public const string GAME = "RWBY Grimm Eclipse";

    internal static new ManualLogSource Logger;
    public static Harmony Harmony;
    public static APConnection Connection;
    private static System.Collections.Queue chatQueue = new();
    public static Dictionary<string, long> Levels = new() {
        {"Emerald_Forest_01", 101},
        {"Emerald_Forest_02", 102},
        {"Mountain_Glenn_01", 103},
        {"Mountain_Glenn_02", 104},
        {"Forever_Fall_01", 105},
        {"Forever_Fall_02", 106},
        {"Merlot_Island_01", 107},
        {"Merlot_Island_02", 108},
        {"Merlot_Lab_01", 109},
        {"Merlot_Lab_02", 110},
    };

    public static Dictionary<long, List<SkillChoice>> SkillMap = new() {
        {201, new (){new("Ruby87bb")}},
        {202, new (){new("Rubyf903")}},
        {203, new (){new("Rubyd065", "Ruby3ccd"), new("Ruby85fd")}},
        {204, new (){new("Rubyd48d"), new("Ruby90b9")}},
        {205, new (){new("Ruby1350"), new("Ruby6976", "Ruby8418")}},
        {206, new (){new("Gene44ef")}},
        {207, new (){new("Gene77e7")}},
        {208, new (){new("Gene1af2")}},
        {209, new (){new("Gene3084")}},
        {210, new (){new("Genec877")}},
        {211, new (){new("Gene51be"), new("Geneea9f")}},

        {301, new (){new("Weis10ef")}},
        {302, new (){new("Weisc8c0")}},
        {303, new (){new("Weis888f"), new("Weisd6c7")}},
        {304, new (){new("Weisc662"), new("Weisd758")}},
        {305, new (){new("Weis34d3"), new("Weis3bf7")}},
        {306, new (){new("Gene44ef")}},
        {307, new (){new("Gene77e7")}},
        {308, new (){new("Gene1af2")}},
        {309, new (){new("Gene3084")}},
        {310, new (){new("Genec877")}},
        {311, new (){new("Gene51be"), new("Geneea9f")}},

        {401, new (){new("Blakc0b9")}},
        {402, new (){new("Blak3360")}},
        {403, new (){new("Blaka263", "Blakcf66")}},
        {404, new (){new("Blak39ba"), new("Blak312b")}},
        {405, new (){new("Blak07c6", "Blakf5cf"), new("Blak358c")}},
        {406, new (){new("Gene44ef")}},
        {407, new (){new("Gene77e7")}},
        {408, new (){new("Gene1af2")}},
        {409, new (){new("Gene3084")}},
        {410, new (){new("Genec877")}},
        {411, new (){new("Gene51be"), new("Geneea9f")}},

        {501, new (){new("Yang1053")}},
        {502, new (){new("Yang08a4")}},
        {503, new (){new("Yang59fc"), new("Yang74ca")}},
        {504, new (){new("Yang9f71"), new("Yangcfaf")}},
        {505, new (){new("Yang0809"), new("Yang2cfd", "Yangfec7")}},
        {506, new (){new("Gene44ef")}},
        {507, new (){new("Gene77e7")}},
        {508, new (){new("Gene1af2")}},
        {509, new (){new("Gene3084")}},
        {510, new (){new("Genec877")}},
        {511, new (){new("Gene51be"), new("Geneea9f")}},
    };

    private void Awake()
    {
        Logger = base.Logger;
        var manager = GameObject.Find("BepInEx_Manager");
        if (manager != null) manager.hideFlags = HideFlags.HideAndDontSave;
        Harmony = new("rwbyap.gameplay");
        Harmony harmony = new("rwbyap.essential");
        harmony.PatchByInterface(typeof(IRwbyEssentialPatch));
    }

    private void Update() {
        while (chatQueue.Count > 0)
        {
            SendChat(chatQueue.Dequeue().ToString());
        }

        if (Connection == null) return;
        while (Connection.Items.Any())
        {
            var item = Connection.Items.DequeueItem();

            if (SkillMap.ContainsKey(item.ItemId))
            {
                foreach (var skill in SkillMap.GetValueSafe(item.ItemId))
                {
                    var cid = "";
                    if (item.ItemId < 300) cid = "Rubyb2cc";
                    else if (item.ItemId < 400) cid = "Weis9ad1";
                    else if (item.ItemId < 500) cid = "Blak8346";
                    else if (item.ItemId < 600) cid = "Yangcde5";
                    var pd = Singleton_MonoBehaviour<ApplicationManager>.Instance.Data.GetLocalPlayerData();
                    var cu = Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.CharacterUpgradeDatabase.Find(skill.Selected);
                    if (pd.GetPlayerCharacter() != null && pd.GetPlayerCharacter().PlayableCharacterDefinition.ID == cid && !pd.GetPlayerCharacter().HasAppliedCharacterUpgrade(cu))
                    {
                        Singleton_MonoBehaviour<GameManager>.Instance.AcquireCharacterUpgrade(pd, cu);
                    }

                    Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.GetCharacterData(cid).PurchasedUpgrades.Add(cu.ID);
                    return;
                }
            }

            Logger.LogInfo($"Received {item.ItemDisplayName} from {item.Player.Alias}'s {item.LocationDisplayName}");
        }
    }

    public static bool LocationMatches(Vector3 a, Vector3 b)
    {
        var threshold = 0.2;
        if (System.Math.Abs(a.x - b.x) > threshold) return false;
        if (System.Math.Abs(a.y - b.y) > threshold) return false;
        if (System.Math.Abs(a.z - b.z) > threshold) return false;
        return true;
    }

    public static void SendChat(string msg)
    {
        Logger.LogInfo($"[Log CHAT] {msg}");
        var log = Singleton_MonoBehaviour<ApplicationManager>.Instance.ChatLog;
        log += $"{msg}{System.Environment.NewLine}";
        while (log.Length >= 4000)
        {
            var x = log.IndexOf(System.Environment.NewLine);
            log = log.Substring(x + System.Environment.NewLine.Length);
        }
        typeof(ApplicationManager).GetField("m_gameChatLog", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Singleton_MonoBehaviour<ApplicationManager>.Instance, log);
    }

    public static void AddAPMessage(Archipelago.MultiClient.Net.MessageLog.Messages.LogMessage msg)
    {
        var colorizedParts = msg.Parts.Select(part => {
            if (part.IsBackgroundColor) return part.Text;
            var c = part.Color;
            var hex = $"{c.R:X2}{c.G:X2}{c.B:X2}";
            if (hex == "008000") hex = "44C444";
            return $"<color=#{hex}>{part.Text}</color>";
        });
        chatQueue.Enqueue(colorizedParts.Join(delimiter: ""));
    }
}

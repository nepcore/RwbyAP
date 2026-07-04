using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Roost;
using RwbyAP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RwbyAP;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class RWBYAP : BaseUnityPlugin
{
    public const string GAME = "RWBY Grimm Eclipse";

    internal static new ManualLogSource Logger;
    public static Harmony Harmony;
    public static APConnection Connection;
    private static System.Collections.Queue chatQueue = new();

    public static bool ProfileLoaded = false;
    public static Stat ItemsReceivedStat = new Stat();
    public static int ItemsProcessed = 0;
    public static long ArtifactsRequired = 40;
    public static long ArtifactsFound => Connection.Items.AllItemsReceived.Count(item => item.ItemId == 99);

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

        {601, new (){new("Jaun1d80")}},
        {602, new (){new("Jaun2ddc")}},
        {603, new (){new("Jaun834b"), new("Jaun317d")}},
        {604, new (){new("Jaun5d28"), new("Jaun648a")}},
        {605, new (){new("Jaun1ad6", "Jauna1f9", "Jaun0fe3")}},
        {606, new (){new("Gene44ef")}},
        {607, new (){new("Gene77e7")}},
        {608, new (){new("Gene1af2")}},
        {609, new (){new("Gene3084")}},
        {610, new (){new("Genec877")}},
        {611, new (){new("Gene51be"), new("Geneea9f")}},

        {701, new (){new("Noraa4b4")}},
        {702, new (){new("Norac00b")}},
        {703, new (){new("Noraee24"), new("Nora06b8", "Nora88cf")}},
        {704, new (){new("Norabf3d"), new("Noradaf5")}},
        {705, new (){new("Nora8e09"), new("Nora3c41")}},
        {706, new (){new("Gene44ef")}},
        {707, new (){new("Gene77e7")}},
        {708, new (){new("Gene1af2")}},
        {709, new (){new("Gene3084")}},
        {710, new (){new("Genec877")}},
        {711, new (){new("Gene51be"), new("Geneea9f")}},

        {801, new (){new("Pyrr72e1")}},
        {802, new (){new("Pyrrced7")}},
        {803, new (){new("Pyrr8ef6"), new("Pyrr4187")}},
        {804, new (){new("Pyrrd3ee"), new("Pyrr0a4b")}},
        {805, new (){new("Pyrr0906"), new("Pyrr53dd")}},
        {806, new (){new("Gene44ef")}},
        {807, new (){new("Gene77e7")}},
        {808, new (){new("Gene1af2")}},
        {809, new (){new("Gene3084")}},
        {810, new (){new("Genec877")}},
        {811, new (){new("Gene51be"), new("Geneea9f")}},

        {901, new (){new("Ren fa46")}},
        {902, new (){new("Ren 419f")}},
        {903, new (){new("Ren d6df"), new("Ren 1f27", "Ren db5c")}},
        {904, new (){new("Ren 1dc7"), new("Ren 81b9")}},
        {905, new (){new("Ren 8f75"), new("Ren 5e88")}},
        {906, new (){new("Gene44ef")}},
        {907, new (){new("Gene77e7")}},
        {908, new (){new("Gene1af2")}},
        {909, new (){new("Gene3084")}},
        {910, new (){new("Genec877")}},
        {911, new (){new("Gene51be"), new("Geneea9f")}},
    };

    private void Awake()
    {
        Logger = base.Logger;
        var manager = GameObject.Find("BepInEx_Manager");
        if (manager != null) manager.hideFlags = HideFlags.HideAndDontSave;
        ItemsReceivedStat.ID = "AP_ItemsReceived";
        ItemsReceivedStat.name = "AP Items Received";
        Harmony = new("rwbyap.gameplay");
        Harmony harmony = new("rwbyap.essential");
        harmony.PatchByInterface(typeof(IRwbyEssentialPatch));
    }

    public static void KillRandomPlayer()
    {
        if (!Singleton_MonoBehaviour<ConnectionManager>.Instance.IsServer) return;
        if (Singleton_MonoBehaviour<ApplicationManager>.Instance.State is GameState_Main)
        {
            var players = Singleton_MonoBehaviour<GameManager>.Instance.GameData.ValidPlayers.Select(p => p.GetPlayerCharacter()).Where(p => p.IsAlive).ToArray();
            var player = players[Random.Range(0, players.Length)];
            player.photonView.RPC("Die", PhotonTargets.All, player.photonView.viewID, new int[0], "Killba39");
        }
    }

    public static void KillAllPlayers()
    {
        if (!Singleton_MonoBehaviour<ConnectionManager>.Instance.IsServer) return;
        if (Singleton_MonoBehaviour<ApplicationManager>.Instance.State is GameState_Main)
        {
            foreach (var player in Singleton_MonoBehaviour<GameManager>.Instance.GameData.ValidPlayers.Select(p => p.GetPlayerCharacter()).Where(p => p.IsAlive))
                player.photonView.RPC("Die", PhotonTargets.All, player.photonView.viewID, new int[0], "Killba39");
        }
    }

    private void Update() {
        if (Connection?.DeathLinkWaitingToProcess != null)
        {
            if (Connection.DeathLinkWaitingToProcess.Cause != null) RWBYAP.SendChat(Connection.DeathLinkWaitingToProcess.Cause);
            else RWBYAP.SendChat($"{Connection.DeathLinkWaitingToProcess.Source} died");
            if (Connection.DeathLinkReceiveMode == DeathLinkMode.Single) KillRandomPlayer();
            else KillAllPlayers();
            Connection.DeathLinkWaitingToProcess = null;
        }

        var messages = System.Math.Min(chatQueue.Count, 3);
        for (var i = 0; i < messages; i++)
        {
            SendChat(chatQueue.Dequeue().ToString());
        }

        if (Connection == null || !ProfileLoaded) return;
        var stopAt = ItemsProcessed + 10;
        while (ItemsProcessed < stopAt && Connection.Items.Any())
        {
            var item = Connection.Items.DequeueItem();

            if (ItemsProcessed < Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.GetStat(ItemsReceivedStat))
            {
                Logger.LogInfo($"Skipping '{item.ItemDisplayName}' because it should have been processed in a prior session");
                ItemsProcessed++;
                continue;
            }

            if (item.ItemId >= 1 && item.ItemId <= 8)
            {
                var cid = new string[]{"Rubyb2cc", "Weis9ad1", "Blak8346", "Yangcde5", "Jaun5986", "Noracf67", "Pyrrfad9", "Ren 7bb0"}[item.ItemId - 1];
                var pd = Singleton_MonoBehaviour<ApplicationManager>.Instance.Data.GetLocalPlayerData();
                var gm = Singleton_MonoBehaviour<GameManager>.Instance.Mode;
                if (gm != null && pd != null && pd.GetPlayerCharacter().PlayableCharacterDefinition.ID == cid)
                {
                    gm.AwardExperience(pd, 10);
                }
                else
                {
                    var cd = Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.GetCharacterData(cid);
                    var pc = Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.DefaultProgressionCurve;
                    var before = pc.GetLevel(cd.Experience);
                    cd.Experience += 10;
                    var after = pc.GetLevel(cd.Experience);
                    if (before != after)
                    {
                        if (after == 4) Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.AddStat(Stat.Database.Stat_Level5_Total, 1);

                        if (after == pc.MaxLevel)
                        {
                            var def = Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.PlayableCharacters.Find(cid);
                            Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.AddStat(def.Stat_MaxLevel, 1);
                            Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.AddStat(Stat.Database.Stat_MaxLevel_Total, 1);
                        }

                        var id = Patches.LevelUpDetectionPatch.CharacterIDBases.GetValueSafe(cid) + (after - 1);
                        if (RWBYAP.Connection.Locations.AllMissingLocations.Contains(id)) RWBYAP.Connection.CompleteLocationChecks(id);
                    }
                }
            }

            if (SkillMap.ContainsKey(item.ItemId))
            {
                foreach (var skill in SkillMap.GetValueSafe(item.ItemId))
                {
                    var cid = "";
                    if (item.ItemId < 300) cid = "Rubyb2cc";
                    else if (item.ItemId < 400) cid = "Weis9ad1";
                    else if (item.ItemId < 500) cid = "Blak8346";
                    else if (item.ItemId < 600) cid = "Yangcde5";
                    else if (item.ItemId < 700) cid = "Jaun5986";
                    else if (item.ItemId < 800) cid = "Noracf67";
                    else if (item.ItemId < 900) cid = "Pyrrfad9";
                    else if (item.ItemId < 1000) cid = "Ren 7bb0";
                    var pd = Singleton_MonoBehaviour<ApplicationManager>.Instance.Data.GetLocalPlayerData();
                    var cu = Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.CharacterUpgradeDatabase.Find(skill.Selected);

                    if (Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.GetCharacterData(cid).PurchasedUpgrades.Contains(skill.Selected)) continue;
                    if (pd != null && pd.GetPlayerCharacter() != null && pd.GetPlayerCharacter().HasAppliedCharacterUpgrade(cu)) continue;

                    if (pd.GetPlayerCharacter() != null && pd.GetPlayerCharacter().PlayableCharacterDefinition.ID == cid)
                    {
                        Singleton_MonoBehaviour<GameManager>.Instance.AcquireCharacterUpgrade(pd, cu);
                    }

                    Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.GetCharacterData(cid).PurchasedUpgrades.Add(cu.ID);
                    break;
                }
            }

            Logger.LogInfo($"Received {item.ItemDisplayName} from {item.Player.Alias}'s {item.LocationDisplayName}");
            Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.AddStat(ItemsReceivedStat, 1);
            ItemsProcessed++;
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

    public static System.IO.Stream GetResource(string name)
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
    }

    public static Sprite SpriteFromResource(string name)
    {
        var tex = new Texture2D(2, 2);
        var res = GetResource(name);
        var bytes = new byte[res.Length];
        res.Read(bytes, 0, bytes.Length);
        tex.LoadImage(bytes);
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }

    public static void MakeArtifactLabel(GameObject parent)
    {
        if (ArtifactsRequired == 0) return;

        var icon = new GameObject("Artifact Icon");
        var img = icon.AddComponent<Image>();
        img.sprite = SpriteFromResource("RwbyAP.assets.golden_knight.png");
        icon.transform.SetParent(parent.transform);
        icon.gameObject.SetActive(true);
        var iconrect = icon.GetComponent<RectTransform>();
        iconrect.anchoredPosition = new Vector2(-300, -122);
        iconrect.anchorMax = iconrect.anchorMin = iconrect.pivot = new Vector2(1, 1);
        iconrect.localScale = new Vector3(0.36f, 0.4f, 1);

        var label = new GameObject("Artifact Label");
        var text = label.AddComponent<Text>();
        text.font = GameObject.Find("/Global - GUI(Clone)/PanelRoot/GenericSelectionPanel(Clone)/Title/Text").GetComponent<Text>().font;
        text.fontSize = 36;
        var state = label.AddComponent<GameObjectStateOverride>();
        state.OnUpdate = go => {
            go.GetComponent<Text>().text = $"{ArtifactsFound} / {ArtifactsRequired}";
        };
        label.transform.SetParent(parent.transform);
        label.gameObject.SetActive(true);
        var labelrect = label.GetComponent<RectTransform>();
        labelrect.anchoredPosition = new Vector2(-185, -119);
        labelrect.anchorMax = labelrect.anchorMin = labelrect.pivot = new Vector2(1, 1);
        labelrect.localScale = new Vector3(1, 1, 1);
    }
}

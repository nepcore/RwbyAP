using HarmonyLib;
using Roost;
using System.Collections.Generic;
using System.Reflection;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(GameChallengePointDefense), "GiveWaveAwards")]
public class PointDefenseWaveAwardsPatch : IRwbyGameplayPatch
{
    public static Dictionary<string, Dictionary<string, long>> Waves = new() {
        {"Emerald_Forest_01", new() {
            {"Wave 1", 10010},
            {"Wave 2", 10011},
            {"Wave 3", 10012},
            {"Wave 4", 10013},
            {"Wave 1 Flawless", 10020},
            {"Wave 2 Flawless", 10021},
            {"Wave 3 Flawless", 10022},
            {"Wave 4 Flawless", 10023},
        }},
        {"Emerald_Forest_02", new() {
            {"Wave 1", 20010},
            {"Wave 2", 20011},
            {"Wave 3", 20012},
            {"Wave 4", 20013},
            {"Wave 1 Flawless", 20020},
            {"Wave 2 Flawless", 20021},
            {"Wave 3 Flawless", 20022},
            {"Wave 4 Flawless", 20023},
        }},
        {"Mountain_Glenn_01", new() {
        }},
        {"Mountain_Glenn_02", new() {
        }},
        {"Forever_Fall_01", new() {
        }},
        {"Forever_Fall_02", new() {
        }},
        {"Merlot_Island_01", new() {
        }},
        {"Merlot_Island_02", new() {
        }},
        {"Merlot_Lab_01", new() {
        }},
        {"Merlot_Lab_02", new() {
        }},
    };
    public static void Postfix(GameChallengePointDefense __instance, Prop ___m_generator, int ___m_startWavePointHealth)
    {
        var level = Singleton_MonoBehaviour<ApplicationManager>.Instance?.GetCurrentLevelDefinition()?.SceneName;
        var wave = (int) typeof(GameChallengeWaves).GetField("m_currentWave", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
        wave += 1;
        //RWBYAP.SendChat($"<color=orange>Wave {wave}</color> Completed");
        var id = Waves.GetValueSafe(level)?.GetValueSafe($"Wave {wave}");
        if (id != null && RWBYAP.Connection.Locations.AllMissingLocations.Contains(id.Value))
            RWBYAP.Connection.CompleteLocationChecks(id.Value);
        var fid = Waves.GetValueSafe(level)?.GetValueSafe($"Wave {wave} Flawless");
        if (___m_generator.Stats.m_stats.curHealth >= ___m_startWavePointHealth)
            if (fid != null && RWBYAP.Connection.Locations.AllMissingLocations.Contains(fid.Value))
                RWBYAP.Connection.CompleteLocationChecks(fid.Value);
        //RWBYAP.SendChat($"<color=yellow>Wave {wave} Flawless</color>");
    }
}

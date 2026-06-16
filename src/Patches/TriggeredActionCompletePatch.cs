using HarmonyLib;
using Roost;
using RwbyAP.Models;
using System.Collections.Generic;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(TriggeredAction), "Complete")]
public class TriggeredActionCompletePatch : IRwbyGameplayPatch
{
    public static Dictionary<string, List<Gate>> LevelGates = new() {
        {"Emerald_Forest_01", new() {
            new (10001, "EmeraldForest_gate_crumble_02", "1"),
            new (10002, "EmeraldForest_gate_crumble_02 (1)", "2"),
            new (10003, "EmeraldForest_gate_crumble_02 (2)", "3"),
        }},
        {"Emerald_Forest_02", new() {
            new (20001, "EmeraldForest_gate_crumble_02", "1"),
            new (20002, "ActionGroup (3)", "2"),
            new (20003, "EmeraldForest_gate_crumble_02 (2)", "3A"),
            new (20004, "EmeraldForest_gate_crumble_02 (1)", "3B"),
        }},
        {"Mountain_Glenn_01", new() {
            new (30001, "EmeraldForest_gate_crumble_02", "1"),
            new (30002, "ActionGroup (6)", "2"),
            new (30003, "EmeraldForest_gate_crumble_02 (1)", "3A"),
            new (30004, "EmeraldForest_gate_crumble_02 (2)", "3B"),
        }},
        {"Mountain_Glenn_02", new() {
            new (40001, "EmeraldForest_gate_crumble_02", "1"),
            new (40002, "GateObject (1)", "2"),
        }},
        {"Forever_Fall_01", new() {
            new (50001, "EmeraldForest_gate_crumble_02", "1A"),
            new (50002, "EmeraldForest_gate_crumble_02 (1)", "1B"),
            new (50003, "EmeraldForest_gate_crumble_02 (2)", "2"),
            new (50004, "EmeraldForest_gate_crumble_02 (3)", "3"),
        }},
        {"Forever_Fall_02", new() {
            new (60001, "EmeraldForest_gate_crumble_02", "1"),
            new (60002, "EmeraldForest_gate_crumble_02 (1)", "2"),
            new (60003, "EmeraldForest_gate_crumble_02 (4)", "3"),
        }},
        {"Merlot_Island_01", new() {
            new (70001, "ForsakenDesert_gate_crumble_02", "1"),
            new (70002, "ForsakenDesert_gate_crumble_02 (1)", "2"),
            new (70003, "ActionGroup (6)", "3"),
            new (70004, "ForsakenDesert_gate_crumble_02 (3)", "4"),
        }},
        {"Merlot_Island_02", new() {
            new (80001, "ActionGroup (1)", "1"),
            new (80002, "ActionGroup (8)", "2"),
            new (80003, "ActionGroup (9)", "3A"),
            new (80004, "ActionGroup (20)", "3B"),
        }},
        {"Merlot_Lab_01", new() {
            new (90001, "GateObject (1)", "1"),
            new (90002, "GateObject (3)", "2"),
            new (90003, "GateObject", "3"),
            new (90004, "GateObject (2)", "4"),
        }},
        {"Merlot_Lab_02", new() {
        }},
    };

    public static Dictionary<string, long> LevelBaseIDs = new() {
        {"Emerald_Forest_01", 10000},
        {"Emerald_Forest_02", 20000},
        {"Mountain_Glenn_01", 30000},
        {"Mountain_Glenn_02", 40000},
        {"Forever_Fall_01", 50000},
        {"Forever_Fall_02", 60000},
        {"Merlot_Island_01", 70000},
        {"Merlot_Island_02", 80000},
        {"Merlot_Lab_01", 90000},
        {"Merlot_Lab_02", 100000}
    };

    public static void Postfix(TriggeredAction __instance)
    {
#if DEBUG
        RWBYAP.Logger.LogInfo($"TriggeredAction completed {__instance?.name} // {__instance?.GetType()?.FullName} // {__instance?.gameObject?.name} // {__instance?.gameObject?.transform?.position} // {__instance?.gameObject?.transform?.localPosition}");
#endif

        var level = Singleton_MonoBehaviour<ApplicationManager>.Instance?.GetCurrentLevelDefinition()?.SceneName;
        if (__instance is VictoryAction)
        {
            var id = LevelBaseIDs.GetValueSafe(level);
            if (RWBYAP.Connection.Locations.AllMissingLocations.Contains(id))
                RWBYAP.Connection.CompleteLocationChecks(id);
            if (level == "Merlot_Lab_02") RWBYAP.Connection.SetGoalAchieved();
            return;
        }

        if (__instance is AnimationAction || __instance is GateAction || __instance is SwitchAction)
        {
            var gates = LevelGates.GetValueSafe(level);
            var gate = gates?.Find(g => g.Name == __instance?.name);
            if (gate != null)
            {
                if (RWBYAP.Connection.Locations.AllMissingLocations.Contains(gate.Value.ID))
                    RWBYAP.Connection.CompleteLocationChecks(gate.Value.ID);
                return;
            }
        }
    }
}

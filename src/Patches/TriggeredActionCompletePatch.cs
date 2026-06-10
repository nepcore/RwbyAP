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
            new (10001, "EmeraldForest_gate_crumble_02", 1),
            new (10002, "EmeraldForest_gate_crumble_02 (1)", 2),
            new (10003, "EmeraldForest_gate_crumble_02 (2)", 3),
        }},
        {"Emerald_Forest_02", new() {
            new (20001, "EmeraldForest_gate_crumble_02", 1),
            new (20003, "EmeraldForest_gate_crumble_02 (2)", 3), // Encounter 3A
        }},
        {"Mountain_Glenn_01", new() {
            new (-1, "EmeraldForest_gate_crumble_02", 1),
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
                RWBYAP.Connection.Locations.CompleteLocationChecks(id);
            if (level == "Merlot_Lab_02") RWBYAP.Connection.SetGoalAchieved();
            //RWBYAP.SendChat($"Level <color=red>{Singleton_MonoBehaviour<ApplicationManager>.Instance?.GetCurrentLevelDefinition()?.PrettySceneName}</color> Completed");
            return;
        }

        if (__instance is AnimationAction)
        {
            var gates = LevelGates.GetValueSafe(level);
            var baseID = LevelBaseIDs.GetValueSafe(level);
            var gate = gates?.Find(g => g.Name == __instance?.name);
            if (gate != null)
            {
                if (RWBYAP.Connection.Locations.AllMissingLocations.Contains(gate.Value.ID))
                    RWBYAP.Connection.Locations.CompleteLocationChecks(gate.Value.ID);
                //RWBYAP.SendChat($"Cleared <color=silver>Encounter {i}</color>");
                return;
            }

            //RWBYAP.SendChat($"AnimationAction {__instance?.name} at {__instance?.transform.position}");
        }

        //if (__instance is AdHocEncounter)
        //    RWBYAP.SendChat($"Completed AdHocEncounter {__instance?.name} at {__instance?.transform.position}");

        //if (__instance is DynamicEncounter)
        //    RWBYAP.SendChat($"Completed DynamicEncounter {__instance?.name} at {__instance?.transform.position}");

        //if (__instance is GameChallengeAction)
        //    RWBYAP.SendChat($"Completed GameChallengeAction {__instance?.name} at {__instance?.transform.position}");
    }
}

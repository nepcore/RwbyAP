using HarmonyLib;
using Roost;
using System.Collections.Generic;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(Pickup), "OnTriggerEnter")]
public class ArtifactPickupPatch : IRwbyGameplayPatch
{
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
    public static Dictionary<string, long> Artifacts = new() {
        {"Golden Knight", 90},
        {"Golden Rook", 91},
        {"Black Knight", 92},
        {"Black Rook", 93},
    };

    public static void Postfix(Pickup __instance)
    {
        if (__instance.gameObject.name.StartsWith("Artifact_"))
        {
            var parts = __instance.gameObject.name.Split('_');
            // yes, if knight then rook else knight
            // whoever named the artifacts in the game
            // has apparently never seen a chess board
            var piece = parts[1] == "knight" ? "Rook" : "Knight";
            var color = parts[2] == "gold" ? "Golden" : "Black";
            //RWBYAP.SendChat($"Found <color=purple>{color} {piece} Artifact</color>");
            var level = Singleton_MonoBehaviour<ApplicationManager>.Instance?.GetCurrentLevelDefinition()?.SceneName;
            var id = LevelBaseIDs.GetValueSafe(level) + Artifacts.GetValueSafe($"{color} {piece}");
            //RWBYAP.SendChat($"Found <color=purple>{color} {piece} Artifact</color> // {level} // {id} // {id != null} // {RWBYAP.Connection.Locations.AllMissingLocations.Contains(id.Value)}");
            if (RWBYAP.Connection.Locations.AllMissingLocations.Contains(id))
                RWBYAP.Connection.Locations.CompleteLocationChecks(id);
        }
    }
}

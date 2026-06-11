using HarmonyLib;
using Roost;
using System.Collections.Generic;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(MicroprogressionController), "LevelUp")]
public class LevelUpDetectionPatch : IRwbyGameplayPatch
{
    private static Dictionary<string, int> CharacterIDBases = new() {
        {"Rubyb2cc", 100},
        {"Weis9ad1", 200},
        {"Blak8346", 300},
        {"Yangcde5", 400},
        {"Jaun5986", -1},
        {"Noracf67", -1},
        {"Pyrrfad9", -1},
        {"Ren 7bb0", -1},
    };

    public static void Postfix(PlayableCharacterDefinition ___m_playableCharacterDefinition, PlayerCharacter ___m_playerCharacter)
    {
        var level = System.Convert.ToInt32(Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.DefaultProgressionCurve.GetDisplayLevel(___m_playerCharacter.Data.Experience));
        var id = CharacterIDBases.GetValueSafe(___m_playableCharacterDefinition.ID) + (level - 2);
        if (RWBYAP.Connection.Locations.AllMissingLocations.Contains(id)) RWBYAP.Connection.CompleteLocationChecks(id);
        //RWBYAP.SendChat($"<color=cyan>{___m_playableCharacterDefinition.name.Replace(" Definition", "")}</color> Level Up ({level} // {id} // {id.GetType().FullName})");
    }
}

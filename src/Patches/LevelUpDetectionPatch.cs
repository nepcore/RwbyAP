using HarmonyLib;
using Roost;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(MicroprogressionController), "LevelUp")]
public class LevelUpDetectionPatch : IRwbyGameplayPatch
{
    public static Dictionary<string, int> CharacterIDBases = new() {
        {"Rubyb2cc", 100},
        {"Weis9ad1", 200},
        {"Blak8346", 300},
        {"Yangcde5", 400},
        {"Jaun5986", 500},
        {"Noracf67", 600},
        {"Pyrrfad9", 700},
        {"Ren 7bb0", 800},
    };

    public static void Postfix(PlayableCharacterDefinition ___m_playableCharacterDefinition, PlayerCharacter ___m_playerCharacter)
    {
        Singleton_MonoBehaviour<ApplicationManager>.Instance.StartCoroutine(CheckLevelUp(___m_playableCharacterDefinition, ___m_playerCharacter));
    }

    private static IEnumerator CheckLevelUp(PlayableCharacterDefinition ___m_playableCharacterDefinition, PlayerCharacter ___m_playerCharacter)
    {
        yield return new WaitForSeconds(2);
        var level = System.Convert.ToInt32(Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.DefaultProgressionCurve.GetDisplayLevel(___m_playerCharacter.Data.Experience));
        var id = CharacterIDBases.GetValueSafe(___m_playableCharacterDefinition.ID) + (level - 2);
        if (RWBYAP.Connection.Locations.AllMissingLocations.Contains(id)) RWBYAP.Connection.CompleteLocationChecks(id);
    }
}

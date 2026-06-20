using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(GameChallengeBoss), "StartChallenge")]
public class BossSpawnPatch : IRwbyGameplayPatch
{
    public static IEnumerator<WaitForSeconds> SpawnBoss(GameChallengeBoss challenge)
    {
        yield return new WaitForSeconds(2);
        try {
            challenge.StartChallenge();
        } catch {} // ignore exceptions
    }

    public static bool Prefix(GameChallengeBoss __instance, BossChallengeAction ___m_bossChallengeAction)
    {
        if (RWBYAP.ArtifactsFound >= RWBYAP.ArtifactsRequired) return true;
        ___m_bossChallengeAction.StartCoroutine(SpawnBoss(__instance));
        return false;
    }
}

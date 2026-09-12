using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(GameChallengeBoss), "StartChallenge")]
public class BossSpawnPatch : IRwbyGameplayPatch
{
    private static long lastReport = 0;
    public static IEnumerator<WaitForSeconds> SpawnBoss(GameChallengeBoss challenge)
    {
        yield return new WaitForSeconds(2);
        try {
            challenge.StartChallenge();
        } catch {} // ignore exceptions
    }

    public static bool Prefix(GameChallengeBoss __instance, BossChallengeAction ___m_bossChallengeAction)
    {
        if (RWBYAP.ArtifactsFound >= RWBYAP.ArtifactsRequired && RWBYAP.LevelsCompleted >= RWBYAP.LevelCompletionsRequired) return true;
        var now = (long) System.DateTimeOffset.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        if (lastReport < now - 60)
        {
            lastReport = now;
            if (RWBYAP.ArtifactsFound < RWBYAP.ArtifactsRequired)
                RWBYAP.SendChat(
                    $"You have only found {RWBYAP.ArtifactsFound} Artifacts. You need at least {RWBYAP.ArtifactsRequired}."
                );
            if (RWBYAP.LevelsCompleted < RWBYAP.LevelCompletionsRequired)
                RWBYAP.SendChat(
                    $"You have only completed {RWBYAP.LevelsCompleted} levels. You need to complete at least {RWBYAP.LevelCompletionsRequired}."
                );
            RWBYAP.SendChat("The boss can not spawn until these conditions are fulfilled.");
        }
        ___m_bossChallengeAction.StartCoroutine(SpawnBoss(__instance));
        return false;
    }
}

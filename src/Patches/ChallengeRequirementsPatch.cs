using HarmonyLib;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(MicroprogressionController), "DoesPlayerMeetChallengeRequirements")]
public class ChallengeRequirementsPatch : IRwbyGameplayPatch
{
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}

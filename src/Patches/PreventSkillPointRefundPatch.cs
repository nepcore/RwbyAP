using HarmonyLib;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PlayerProfile.CharacterData), "IsEligibleForSkillPointRefund")]
public class PreventSkillPointRefundPatch : IRwbyGameplayPatch
{
    public static bool Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
}

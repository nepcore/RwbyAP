using HarmonyLib;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(MicroprogressionController), "CanPlayerAffordUpgrade")]
public class PreventManualSkillSelectionPatch : IRwbyGameplayPatch
{
    public static bool Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
}

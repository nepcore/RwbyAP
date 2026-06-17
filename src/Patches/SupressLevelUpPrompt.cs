using HarmonyLib;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(MicroprogressionController), "Update")]
public class SuppressLevelUpPrompt : IRwbyGameplayPatch
{
    public static bool Prefix(ref bool ___m_newSkills)
    {
        ___m_newSkills = false;
        return true;
    }
}

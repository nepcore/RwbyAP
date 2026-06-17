using HarmonyLib;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(MicroprogressionController), "LevelUp")]
public class SuppressLevelUpPrompt : IRwbyGameplayPatch
{
    public static bool Prefix()
    {
        return false;
    }
}

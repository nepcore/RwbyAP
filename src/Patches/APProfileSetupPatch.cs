using HarmonyLib;
using Roost;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PlayerProfile), "Reload")]
public class APProfileSetupPatch : IRwbyGameplayPatch
{
    public static bool Prefix(ref PlayerProfile.ProfileData ___m_profileData)
    {
        ___m_profileData = new();
        Singleton_MonoBehaviour<ApplicationManager>.Instance.State.Update();
        return false;
    }
}

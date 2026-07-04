using HarmonyLib;
using Roost;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(GameMode), "OnGameOverVictory")]
public class ReturnToLobbyAfterLevelPatch : IRwbyGameplayPatch
{
    public static bool Prefix()
    {
        if (!Singleton_MonoBehaviour<ConnectionManager>.Instance.IsServer) return false;
        Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.Update();
        Singleton_MonoBehaviour<ApplicationManager>.Instance.ReturnToLobby();
        return false;
    }
}

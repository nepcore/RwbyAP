using HarmonyLib;
using Roost;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(GameMode), "OnGameOverVictory")]
public class ReturnToLobbyAfterLevelPatch : IRwbyGameplayPatch
{
    public static bool Prefix()
    {
        Singleton_MonoBehaviour<ApplicationManager>.Instance.ReturnToLobby();
        return false;
    }
}

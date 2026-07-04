using HarmonyLib;
using Roost;
using RwbyAP.Models;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(GameMode), "GameOver")]
public class TeamDeathLinkPatch : IRwbyGameplayPatch
{
    public static void Postfix()
    {
        if (!Singleton_MonoBehaviour<ConnectionManager>.Instance.IsServer) return;
        if (RWBYAP.Connection.DeathLinkSendMode == DeathLinkMode.All)
            RWBYAP.Connection.SendDeathLink();
    }
}

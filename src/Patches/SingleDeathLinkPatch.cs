using HarmonyLib;
using Roost;
using RwbyAP.Models;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(BaseEntity), "Die")]
public class SingleDeathLinkPatch : IRwbyGameplayPatch
{
    public static void Postfix(BaseEntity __instance)
    {
        if (!Singleton_MonoBehaviour<ConnectionManager>.Instance.IsServer) return;
        if (__instance is PlayerCharacter && RWBYAP.Connection.DeathLinkSendMode == DeathLinkMode.Single)
            RWBYAP.Connection.SendDeathLink();
    }
}

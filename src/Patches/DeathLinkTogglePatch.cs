using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PreGameLobbyController), "Initialize")]
public class DeathLinkTogglePatch : IRwbyGameplayPatch
{
    public static void Postfix(GenericSelectionPanel ___m_lobbyPanel)
    {
        var btn = ___m_lobbyPanel.AllocateGenericSelectionItem();
        btn.Label.text = $"DEATH LINK: {(RWBYAP.Connection.IsDeathLinkOn() ? "ON" : "OFF")}";
        btn.Button.onClick.AddListener(() => {
            RWBYAP.Logger.LogInfo("Toggling death link");
            RWBYAP.Connection.ToggleDeathLink();
            ___m_lobbyPanel.StartCoroutine(UpdateLabel(btn));
        });
    }

    public static IEnumerator UpdateLabel(GenericSelectionItem btn)
    {
        yield return new WaitForEndOfFrame();
        btn.Label.text = $"DEATH LINK: {(RWBYAP.Connection.IsDeathLinkOn() ? "ON" : "OFF")}";
    }
}

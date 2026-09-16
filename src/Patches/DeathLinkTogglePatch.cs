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

// prestige challenge 1: 4 characters level 10
[HarmonyPatch(typeof(PreGameLobbyController), "Initialize")]
public class PrestigePatch : IRwbyGameplayPatch
{
    public static void Postfix(GenericSelectionItem ___m_privacyButton)
    {
        ___m_privacyButton.Button.onClick = new();
        if (___m_privacyButton == null) return;
        ___m_privacyButton.Label.text = "RANK UP";
        ___m_privacyButton.Button.onClick.AddListener(() => {
            var am = Roost.Singleton_MonoBehaviour<ApplicationManager>.Instance;
            var m = am.GetType().GetMethod("ChangeLocalState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var currState = am.State;
            var actions = new PrestigePanelController.Actions();
            UnityEngine.Events.UnityAction back = () => {
                m.Invoke(am, [currState]);
            };
            actions.OnBack = back;
            actions.OnPrestigeLevelUp = () => {
                am.Profile.IncreasePrestigeLevel();
                m.Invoke(am, [new GameState_PrestigeAward(back)]);
            };
            m.Invoke(am, [new GameState_PrestigePanel(actions)]);
        });
    }
}

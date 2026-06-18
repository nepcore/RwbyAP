using HarmonyLib;
using Roost;

namespace RwbyAP;

[HarmonyPatch(typeof(InGameMenuPanelController), "Initialize")]
public class PauseReturnToLobbyPatch : IRwbyGameplayPatch
{
    public static void Postfix(GenericSelectionPanel ___m_inGameMenuPanel)
    {
        if (!PhotonNetwork.isMasterClient) return;
        ___m_inGameMenuPanel.SelectionItems[4].Localization.Term = "AP_ReturnToLobby";
        ___m_inGameMenuPanel.SelectionItems[4].Button.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        ___m_inGameMenuPanel.SelectionItems[4].Button.onClick.AddListener(() => {
            Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.Update();
            Singleton_MonoBehaviour<ApplicationManager>.Instance.ReturnToLobby();
        });
    }
}

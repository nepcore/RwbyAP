using HarmonyLib;
using Roost;

namespace RwbyAP;

[HarmonyPatch(typeof(LevelIntroPanelController), "Initialize")]
public class PreLevelReturnToLobbyPatch : IRwbyGameplayPatch
{
    public static void Postfix(GenericSelectionPanel ___m_levelIntroPanel)
    {
        if (!PhotonNetwork.isMasterClient) return;
        ___m_levelIntroPanel.SelectionItems[3].Localization.Term = "AP_ReturnToLobby";
        ___m_levelIntroPanel.SelectionItems[3].Button.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        ___m_levelIntroPanel.SelectionItems[3].Button.onClick.AddListener(() => {
            Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.Update();
            Singleton_MonoBehaviour<ApplicationManager>.Instance.ReturnToLobby();
        });
    }
}

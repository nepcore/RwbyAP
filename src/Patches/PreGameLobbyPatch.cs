using HarmonyLib;
using Roost;
using System.Linq;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PreGameLobbyController), "Initialize")]
public class PreGameLobbyPatch : IRwbyGameplayPatch
{
    public static void Postfix(PreGameLobbyController __instance, GenericSelectionItem ___m_privacyButton, GenericSelectionPanel ___m_lobbyPanel)
    {
        RWBYAP.MakeArtifactLabel(___m_lobbyPanel.gameObject);

        if (!Singleton_MonoBehaviour<ConnectionManager>.Instance.IsServer) return;
        ___m_privacyButton.gameObject.SetActive(false);
        var campaign = Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.CampaignDefinitionDatabase.GetFirstCampaignInStyle(CampaignDefinition.CampaignStyle.Campaign);
        Singleton_MonoBehaviour<GameManager>.Instance.GameData.CampaignID = campaign.ID;
        Singleton_MonoBehaviour<ConnectionManager>.Instance.RoomProperties.CampaignDefinition = campaign;
        var level = campaign.Levels.First();
        foreach (var l in campaign.Levels)
        {
            var id = RWBYAP.Levels.GetValueSafe(l.SceneName);
            if (RWBYAP.Connection.Items.AllItemsReceived.Select(item => item.ItemId).Contains(id))
            {
                level = l;
                break;
            }
        }
        Singleton_MonoBehaviour<ConnectionManager>.Instance.RoomProperties.LevelDefinition = level;
        Singleton_MonoBehaviour<ConnectionManager>.Instance.PublishRoomProperties();
    }
}

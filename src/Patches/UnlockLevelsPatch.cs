using HarmonyLib;
using System.Linq;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PlayerProfile), "IsLevelUnlockedForPlayer")]
public class UnlockLevelsPatch : IRwbyGameplayPatch
{
    public static bool Prefix(CampaignDefinition __0, LevelDefinition __1, ref bool __result)
    {
        if (__0.Style != CampaignDefinition.CampaignStyle.Campaign)
        {
            __result = false;
        }
        else if (__1.SceneName != "Forsaken_Desert" && __1.SceneName != "Forsaken_Desert_01")
        {
            var id = RWBYAP.Levels.GetValueSafe(__1.SceneName);
            __result = RWBYAP.Connection.Items.AllItemsReceived.Select(item => item.ItemId).Contains(id);
        }
        else __result = false;
        return false;
    }
}

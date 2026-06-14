using HarmonyLib;
using Newtonsoft.Json;
using Roost;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PlayerProfile), "Reload")]
public class APProfileSetupPatch : IRwbyGameplayPatch
{
    public static bool Prefix(ref PlayerProfile.ProfileData ___m_profileData)
    {
        var loaded = RWBYAP.Connection.DataStorage[$"{RWBYAP.Connection.Team}_{RWBYAP.Connection.Slot}_rwbyge_profile"];
        string json = null;
        if (loaded != null) json = loaded.To<string>();
        if (json != null)
        {
            try
            {
                RWBYAP.Logger.LogInfo("Trying to load existing profile from datastorage");
                ___m_profileData = JsonConvert.DeserializeObject<PlayerProfile.ProfileData>(json);
            }
            catch (System.Exception e)
            {
                RWBYAP.Logger.LogError($"Failed decoding profile: {e.Message}\n{e.StackTrace}");
                ___m_profileData = new();
                Singleton_MonoBehaviour<ApplicationManager>.Instance.State.Update();
            }
        }
        else
        {
            RWBYAP.Logger.LogInfo("No profile found, creating new one");
            ___m_profileData = new();
            Singleton_MonoBehaviour<ApplicationManager>.Instance.State.Update();
        }
        Singleton_MonoBehaviour<ConnectionManager>.Instance.PublishLocalPlayerNetworkProperties();
        RWBYAP.ProfileLoaded = true;
        return false;
    }
}

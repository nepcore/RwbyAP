using HarmonyLib;
using Roost;
using System.Collections;
using System.Reflection;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(NetworkListener), "OnLeftRoom")]
public class DisconnectPatch : IRwbyGameplayPatch
{
    public static IEnumerator ReturnToMainMenu()
    {
        yield return new UnityEngine.WaitForEndOfFrame();
        if (Singleton_MonoBehaviour<ApplicationManager>.Instance.State is GameState_ModeSelectionMenu)
        {
            var state = (GameState_ModeSelectionMenu) Singleton_MonoBehaviour<ApplicationManager>.Instance.State;
            var actions = (ModeSelectionPanelController.Actions) state.GetType().GetField("m_actions", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(state);
            actions.OnBack();
        }
    }

    public static void Postfix(NetworkListener __instance)
    {
        if (RWBYAP.Connection == null || !RWBYAP.Connection.Connected) return;
        RWBYAP.Logger.LogInfo("Disconnecting from AP");
        RWBYAP.Connection.Disconnect();
        __instance.StartCoroutine(ReturnToMainMenu());
    }
}

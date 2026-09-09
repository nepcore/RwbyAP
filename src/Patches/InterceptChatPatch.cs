using HarmonyLib;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(ShellUIChatBox), "Initialize")]
public class InterceptChatPatch : IRwbyGameplayPatch
{
    public static void Postfix(ShellUIChatBox __instance, ref System.Action<string> ___m_onChatSent)
    {
        var orig = ___m_onChatSent;
        ___m_onChatSent = msg => {
            if (!msg.Trim().StartsWith("!") || msg.Trim() == "!") orig(msg);
            else
            {
                RWBYAP.Connection.SendAPMessage(msg.Trim().Substring(1));
            };
        };
    }
}

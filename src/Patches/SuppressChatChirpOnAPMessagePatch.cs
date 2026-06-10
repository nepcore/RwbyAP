using HarmonyLib;
using Roost;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(ShellUIChatBox), "UpdateChatText")]
public class SuppressChatChirpOnAPMessagePatch : IRwbyGameplayPatch
{
    public static bool Prefix(string ___m_previousChatText, string __0, ref SoundEffect ___m_chirpSoundEffect, ref SoundEffect __state)
    {
        if (__0 != ___m_previousChatText)
        {
            var lines = __0.Split(new [] {System.Environment.NewLine}, System.StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) return true;
            var line = lines[lines.Length - 1];
            foreach (var player in Singleton_MonoBehaviour<GameManager>.Instance?.GameData?.Players)
            {
                if (line.StartsWith($"{player.DisplayName}:"))
                {
                    RWBYAP.Logger.LogInfo($"[CHAT] {line}");
                    return true;
                }
            }
            __state = ___m_chirpSoundEffect;
            ___m_chirpSoundEffect = null;
        }
        return true;
    }

    public static void Postfix(ref SoundEffect ___m_chirpSoundEffect, SoundEffect __state)
    {
        if (__state != null && ___m_chirpSoundEffect == null) ___m_chirpSoundEffect = __state;
    }
}

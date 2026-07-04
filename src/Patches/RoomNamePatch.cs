using HarmonyLib;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(ConnectionManager), "GetDesiredRoomName")]
public class RoomNamePatch : IRwbyGameplayPatch
{
    private static System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
    public static bool Prefix(ref string __result)
    {
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes($"{RWBYAP.Connection.Host}:{RWBYAP.Connection.Port}/{RWBYAP.Connection.Seed}"));
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        __result = $"{RWBYAP.Connection.SlotName}.{sb.ToString()}";
        return false;
    }
}

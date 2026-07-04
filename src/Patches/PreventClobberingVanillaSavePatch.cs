using HarmonyLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PlayerProfile), "Update")]
public class PreventClobberingVanillaSavePatch : IRwbyGameplayPatch
{
    private static long lastSave = 0;
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        bool ldstrProfile = false;
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Ldstr && System.Convert.ToString(instruction.operand) == "Profile")
            {
                ldstrProfile = true;
            }
            else if (instruction.opcode == OpCodes.Ldstr)
            {
                ldstrProfile = false;
            }

            if (ldstrProfile && instruction.opcode == OpCodes.Call && ((MethodInfo) instruction.operand).Name == "UpdateCloudData")
            {
                // replacing method call with another one to not break lineup of jump instructions
                yield return CodeInstruction.Call(typeof(PreventClobberingVanillaSavePatch), "StoreProfile", [typeof(string), typeof(PlayerProfile.ProfileData)]);
            }
            else yield return instruction;
        }
    }

    public static void StoreProfile(string str, PlayerProfile.ProfileData d)
    {
        var now = (long) System.DateTimeOffset.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        if (lastSave + 30 > now) return;

        if (!RWBYAP.Connection.Connected)
        {
            RWBYAP.Logger.LogWarning("Not connected to AP instance, won't save profile");
            return;
        }

        var json = JsonConvert.SerializeObject(d);
        if (json == null)
        {
            RWBYAP.Logger.LogWarning("Failed to serialize profile, won't save profile");
            return;
        }

        RWBYAP.Logger.LogInfo("Writing profile to datastorage");
        new System.Threading.Thread(() => {
            RWBYAP.Connection.DataStorage[$"{RWBYAP.Connection.Team}_{RWBYAP.Connection.Slot}_rwbyge_profile_{OnlineManager.GetLocalUserDisplayName()}"] = json;
        }).Start();
        lastSave = now;
    }
}

using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PlayerProfile), "Update")]
public class PreventClobberingVanillaSavePatch : IRwbyGameplayPatch
{
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
                yield return CodeInstruction.Call(typeof(PreventClobberingVanillaSavePatch), "Dummy", [typeof(string), typeof(PlayerProfile.ProfileData)]);
            }
            else yield return instruction;
        }
    }

    public static void Dummy(string str, PlayerProfile.ProfileData d) {}
}

using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PlayerData), "AddCharacterUpgrade")]
public class RemoveSkillPointCheckPatch : IRwbyGameplayPatch
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        bool flag = false;
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Bge) flag = true;

            if (flag && instruction.opcode == OpCodes.Ret)
            {
                flag = false;
                yield return new CodeInstruction(OpCodes.Nop);
            }
            else yield return instruction;
        }
    }
}

using HarmonyLib;

namespace RwbyAP;

public static class HarmonyExtensions
{
    public static void PatchByInterface(this Harmony harmony, System.Type type)
    {
        foreach (var t in type.Assembly.GetTypes())
        {
            if (type.IsAssignableFrom(t)) harmony.PatchAll(t);
        }
    }
}

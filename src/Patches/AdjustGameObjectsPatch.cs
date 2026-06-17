using HarmonyLib;
using Roost;
using RwbyAP.Models;
using UnityEngine;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(GameState_Main), "Initialize")]
public class AdjustGameObjectsPatch : IRwbyGameplayPatch
{
    public static void Postfix()
    {
        if (Singleton_MonoBehaviour<ApplicationManager>.Instance.GetCurrentLevelDefinition().SceneName == "Merlot_Lab_02")
        {
            foreach (var obj in Object.FindObjectsOfType<AnimationAction>())
            {
                if (obj.gameObject.name == "Merlot_Door_01" && RWBYAP.LocationMatches(obj.transform.position, new Vector3(-57.0f, -6.7f, 221.5f)))
                {
                    var marker = obj.gameObject.AddComponent<Marker>();
                    marker.Name = "Encounter 2";
                }

                if (obj.gameObject.name == "ActionGroup (2)" && RWBYAP.LocationMatches(obj.transform.position, new Vector3(-163.7f, -14.0f, 128.6f)))
                {
                    var marker = obj.gameObject.AddComponent<Marker>();
                    marker.Name = "Encounter 3";
                }
            }

            foreach (var obj in Object.FindObjectsOfType<SwitchAction>())
            {
                if (obj.gameObject.name == "ActionGroup (6)" && RWBYAP.LocationMatches(obj.transform.position, new Vector3(-333.8f, -19.7f, 15.9f)))
                {
                    var marker = obj.gameObject.AddComponent<Marker>();
                    marker.Name = "Encounter 4";
                }
            }
        }
    }
}

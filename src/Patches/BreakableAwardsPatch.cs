using HarmonyLib;
using Roost;
using RwbyAP.Models;
using System.Collections.Generic;
using UnityEngine;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(Prop), "OnDeath")]
public class BreakableAwardsPatch : IRwbyGameplayPatch
{
    public static Dictionary<string, List<Breakable>> Breakables = new() {
        {"Emerald_Forest_01", new() {
            new (10100, "Before Arena 1 Pot 1", new Vector3(209.2f, 26.4f, -202.5f)),
            new (10101, "Before Arena 1 Pot 2", new Vector3(207.7f, 26.4f, -201.5f)),
            new (10102, "Before Arena 1 Pot 3", new Vector3(208.1f, 26.4f, -198.9f)),
            new (10103, "Arena 1 Pot 1", new Vector3(167.4f, 17.9f, -152.7f)),
            new (10104, "Arena 1 Pot 2", new Vector3(165.3f, 17.9f, -153.3f)),
            new (10105, "Arena 1 Pot 3", new Vector3(164.0f, 17.9f, -152.5f)),
            new (10106, "Before Arena 2 Pot 1", new Vector3(102.9f, 17.8f, -124.0f)),
            new (10107, "Before Arena 2 Pot 2", new Vector3(105.0f, 17.9f, -124.0f)),
            new (10108, "Before Arena 2 Pot 3", new Vector3(101.6f, 17.8f, -122.5f)),
            new (10109, "Arena 2 Pot 1", new Vector3(-13.2f, 7.2f, -118.3f)),
            new (10110, "Arena 2 Pot 2", new Vector3(-13.1f, 7.2f, -116.4f)),
            new (10111, "Arena 2 Pot 3", new Vector3(-14.9f, 7.2f, -117.6f)),
            new (10112, "Arena 2 Pot 4", new Vector3(-8.2f, 7.2f, -116.6f)),
            new (10113, "Arena 2 Pot 5", new Vector3(-6.2f, 7.2f, -117.3f)),
            new (10114, "Arena 2 Crate 1", new Vector3(32.4f, 7.8f, -173.5f)),
            new (10115, "Arena 2 Crate 2", new Vector3(33.6f, 7.6f, -176.4f)),
            new (10116, "Arena 2 Crate 3", new Vector3(25.5f, 8.0f, -182.1f)),
            new (10117, "Before Arena 3 Pot 1", new Vector3(-150.0f, 15.5f, -171.9f)),
            new (10118, "Before Arena 3 Pot 2", new Vector3(-149.7f, 15.5f, -169.7f)),
            new (10119, "Arena 3 Pot 1", new Vector3(-178.1f, 0.9f, -79.5f)),
            new (10120, "Arena 3 Pot 2", new Vector3(-179.4f, 0.9f, -78.5f)),
            new (10121, "Arena 3 Crate 1", new Vector3(-201.6f, 1.4f, -85.8f)),
            new (10122, "Arena 3 Crate 2", new Vector3(-159.4f, 5.6f, -47.3f)),
            new (10123, "Arena 3 Crate 3", new Vector3(-160.7f, 5.6f, -44.3f)),
            new (10124, "Arena 3 Crate 4", new Vector3(-158.7f, 6.5f, -32.9f)),
            new (10125, "By Bridge Pot 1", new Vector3(-156.3f, 17.5f, -5.2f)),
            new (10126, "By Bridge Crate 1", new Vector3(-154.5f, 17.5f, -3.0f)),
            new (10127, "By Bridge Crate 2", new Vector3(-149.7f, 17.5f, -4.0f)),
            new (10128, "By Bridge Crate 3", new Vector3(-154.8f, 17.5f, 0.0f)),
            new (10129, "By Bridge Crate 4", new Vector3(-138.3f, 17.4f, -14.8f)),
            new (10130, "By Artifact Crate 1", new Vector3(-112.7f, 10.7f, -72.7f)),
            new (10131, "By Artifact Crate 2", new Vector3(-60.2f, 10.0f, -108.7f)),
            new (10132, "Cave Crate 1", new Vector3(-145.3f, 11.9f, 50.5f)),
            new (10133, "Cave Crate 2", new Vector3(-129.0f, 9.7f, 65.3f)),
            new (10134, "Cave Crate 3", new Vector3(-38.1f, 2.6f, 72.9f)),
        }},
        {"Emerald_Forest_02", new() {
            new (20100, "By Artifact Crate 1", new Vector3(143.1f, 31.8f, -88.0f)),
            new (20101, "By Artifact Crate 2", new Vector3(147.4f, 31.8f, -85.3f)),
            new (20102, "By Artifact Crate 3", new Vector3(150.3f, 31.8f, -86.5f)),
            new (20103, "By Artifact Crate 4", new Vector3(166.2f, 34.2f, -99.0f)),
            new (20104, "Before Arena 1 Pot 1", new Vector3(160.8f, 31.9f, -24.5f)),
            new (20105, "Before Arena 1 Pot 2", new Vector3(148.1f, 31.8f, -20.7f)),
            new (20106, "Before Arena 1 Pot 3", new Vector3(148.3f, 31.8f, -18.7f)),
            new (20107, "Before Arena 1 Crate 1", new Vector3(143.2f, 31.8f, -12.0f)),
            new (20108, "Before Arena 1 Crate 2", new Vector3(90.0f, 31.8f, 50.7f)),
            new (20109, "Arena 1 Crate 1", new Vector3(21.2f, 21.8f, 200.5f)),
            new (20110, "Arena 1 Crate 2", new Vector3(21.4f, 21.7f, 204.4f)),
            new (20111, "Before Arena 2 Crate 1", new Vector3(-7.9f, 26.1f, 247.7f)),
            new (20112, "Before Arena 2 Crate 2", new Vector3(-39.1f, 26.2f, 241.4f)),
            new (20113, "Before Arena 2 Crate 3", new Vector3(-42.3f, 26.2f, 243.4f)),
            new (20114, "Before Arena 2 Crate 4", new Vector3(-88.8f, 26.2f, 240.6f)),
            new (20115, "Arena 2 Pot 1", new Vector3(-86.5f, 25.9f, 154.6f)),
            new (20116, "Arena 2 Pot 2", new Vector3(-85.4f, 25.9f, 153.3f)),
            new (20117, "Arena 2 Pot 3", new Vector3(-95.8f, 25.9f, 145.0f)),
            new (20118, "Arena 2 Pot 4", new Vector3(-66.2f, 25.9f, 143.0f)),
            new (20119, "Arena 2 Pot 5", new Vector3(-65.3f, 25.9f, 141.7f)),
            new (20120, "Arena 2 Pot 6", new Vector3(-64.8f, 25.9f, 135.2f)),
            new (20121, "Arena 2 Pot 7", new Vector3(-86.4f, 27.3f, 122.1f)),
            new (20122, "Arena 2 Pot 8", new Vector3(-86.3f, 27.3f, 120.8f)),
            new (20123, "Arena 2 Pot 9", new Vector3(-65.6f, 25.9f, 93.4f)),
            new (20124, "Arena 2 Pot 10", new Vector3(-66.9f, 25.9f, 92.0f)),
            new (20125, "Arena 2 Pot 11", new Vector3(-67.9f, 25.8f, 64.1f)),
            new (20126, "Arena 2 Pot 12", new Vector3(-68.3f, 25.8f, 62.2f)),
            new (20127, "Arena 2 Pot 13", new Vector3(-70.4f, 25.8f, 62.7f)),
            new (20128, "Arena 2 Crate", new Vector3(-61.9f, 25.9f, 109.4f)),
            new (20129, "Before Arena 3 Crate 1", new Vector3(-172.1f, 26.0f, 77.7f)),
            new (20130, "Arena 3A Crate 1", new Vector3(-135.6f, 20.1f, 8.3f)),
            new (20131, "Arena 3A Crate 2", new Vector3(-135.4f, 20.1f, 4.4f)),
            new (20132, "Arena 3A Crate 3", new Vector3(-140.1f, 20.1f, 1.3f)),
            new (20133, "Arena 3A Crate 4", new Vector3(-170.1f, 20.0f, -28.1f)),
            new (20134, "Arena 3A Crate 5", new Vector3(-171.0f, 20.0f, -32.0f)),
            new (20135, "Arena 3A Crate 6", new Vector3(-168.3f, 20.0f, -34.1f)),
            new (20136, "Before Arena 4 Crate 1", new Vector3(-186.3f, 5.3f, -114.1f)),
            new (20137, "Before Arena 4 Crate 2", new Vector3(-158.6f, 5.5f, -92.9f)),
            new (20138, "Before Arena 4 Crate 3", new Vector3(-130.9f, 3.8f, -146.8f)),
            new (20139, "Before Arena 4 Crate 4", new Vector3(-110.9f, 0.1f, -144.4f)),
            new (20140, "Before Arena 4 Crate 5", new Vector3(-107.8f, 0.1f, -141.6f)),
            new (20141, "Right of Arena 4 Crate 1", new Vector3(-73.5f, 6.5f, -169.5f)),
            new (20142, "Right of Arena 4 Crate 2", new Vector3(-75.0f, 6.5f, -173.6f)),
            new (20143, "Arena 4 Crate 1", new Vector3(-98.0f, 5.2f, -111.7f)),
            new (20144, "Arena 4 Crate 2", new Vector3(-32.8f, 5.2f, -126.2f)),
            new (20145, "Before Arena 2 Crate 5", new Vector3(-24.2f, 24.0f, 213.9f)),
            new (20146, "Before Arena 4 Crate 6", new Vector3(-210.7f, 5.2f, -59.3f)),
            new (20147, "Before Arena 4 Crate 7", new Vector3(-213.4f, 5.2f, -60.5f)),
        }},
        {"Mountain_Glenn_01", new() {
        }},
        {"Mountain_Glenn_02", new() {
        }},
        {"Forever_Fall_01", new() {
        }},
        {"Forever_Fall_02", new() {
        }},
        {"Merlot_Island_01", new() {
        }},
        {"Merlot_Island_02", new() {
        }},
        {"Merlot_Lab_01", new() {
        }},
        {"Merlot_Lab_02", new() {
        }},
    };

    public static void Postfix(GameObject ___m_livingGameObject)
    {
        #if DEBUG
        RWBYAP.Logger.LogInfo($"Prop died: {___m_livingGameObject?.transform?.parent?.name}, {___m_livingGameObject?.gameObject?.transform?.position}");
        #endif

        var level = Singleton_MonoBehaviour<ApplicationManager>.Instance.GetCurrentLevelDefinition();
        var levelName = level.SceneName;
        var breakables = Breakables.GetValueSafe(levelName);
        if (breakables == null)
        {
            //RWBYAP.SendChat("Unknown Prop");
            RWBYAP.Logger.LogInfo($"Prop died: {___m_livingGameObject?.transform?.parent?.name}, {___m_livingGameObject?.gameObject?.transform?.position}");
            return;
        }
        foreach (var breakable in breakables)
        {
            if (RWBYAP.LocationMatches(breakable.Position, ___m_livingGameObject.transform.position))
            {
                if (RWBYAP.Connection.Locations.AllMissingLocations.Contains(breakable.ID))
                {
                    RWBYAP.Connection.Locations.CompleteLocationChecks(breakable.ID);
                }
                return;
            }
        }
        //RWBYAP.SendChat("Unknown Prop");
        RWBYAP.Logger.LogInfo($"Prop died: {___m_livingGameObject?.transform?.parent?.name}, {___m_livingGameObject?.gameObject?.transform?.position}");
    }
}

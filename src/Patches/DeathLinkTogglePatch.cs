using HarmonyLib;
using System.Collections;
using UnityEngine;

using System.Linq;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(PreGameLobbyController), "Initialize")]
public class DeathLinkTogglePatch : IRwbyGameplayPatch
{
    public static void Postfix(GenericSelectionPanel ___m_lobbyPanel)
    {
        var btn = ___m_lobbyPanel.AllocateGenericSelectionItem();
        btn.Label.text = $"DEATH LINK: {(RWBYAP.Connection.IsDeathLinkOn() ? "ON" : "OFF")}";
        btn.Button.onClick.AddListener(() => {
            RWBYAP.Logger.LogInfo("Toggling death link");
            RWBYAP.Connection.ToggleDeathLink();
            ___m_lobbyPanel.StartCoroutine(UpdateLabel(btn));
        });
    }

    public static IEnumerator UpdateLabel(GenericSelectionItem btn)
    {
        yield return new WaitForEndOfFrame();
        btn.Label.text = $"DEATH LINK: {(RWBYAP.Connection.IsDeathLinkOn() ? "ON" : "OFF")}";
    }
}

// rank 1:
// - 4x lv 10
// rank 2:
// - 1x lv 10
// - 10x revive
// - 50x ranged kill
// - 1x 50 combo
// - 20x ursa kill
// rank 3:
// - 1x lv 10
// - 300x creep kill
// - 20x team attack
// - 50x counter
// - 4x mastery challenge
// rank 4:
// - 1x lv 10
// - 300x beowolf kill
// - 20x team attack setup
// - 100x destroyed prop
// - 10x artifact found
// rank 5:
// - 1x lv 10
// - 150x alpha creep kill
// - 5x flawless wave
// - 20x revive
// - 100x ultimate
// rank 6:
// - 2x lv 10
// - 150x alpha beowolf kill
// - 6x mastery challenge
// - 10x wave mvp
// - 15x artifact found
// rank 7:
// - 2x lv 10
// - 100x boarbatsk kill
// - 100x enemy stunned
// - 100x team attack
// - 50x assist
// rank 8:
// - 2x lv 10
// - 30x ursa kill
// - 20x mutant beowolf kill
// - 200x android kill
// - 8x mastery challenge
// rank 9:
// - 2x lv 10
// - 100x mutant creep kill
// - 200x enemy stunned
// - 1x 75 combo
// - 50x wave mvp
// rank 10:
// - 4x lv 10
// - 3000x enemy kill
// - 1x 100 combo
// - 10x mastery challenge
// - 50x flawless wave
[HarmonyPatch(typeof(PreGameLobbyController), "Initialize")]
public class PrestigePatch : IRwbyGameplayPatch
{
    public static void Postfix(GenericSelectionItem ___m_privacyButton)
    {
        ___m_privacyButton.Button.onClick = new();
        if (___m_privacyButton == null) return;
        ___m_privacyButton.Label.text = "RANK UP";
        ___m_privacyButton.Button.onClick.AddListener(() => {
            var am = Roost.Singleton_MonoBehaviour<ApplicationManager>.Instance;
            var m = am.GetType().GetMethod("ChangeLocalState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var currState = am.State;
            var actions = new PrestigePanelController.Actions();
            UnityEngine.Events.UnityAction back = () => {
                m.Invoke(am, [currState]);
            };
            actions.OnBack = back;
            actions.OnPrestigeLevelUp = () => {
                am.Profile.IncreasePrestigeLevel();
                m.Invoke(am, [new GameState_PrestigeAward(back)]);
            };
            m.Invoke(am, [new GameState_PrestigePanel(actions)]);
        });
    }
}

[HarmonyPatch(typeof(ProgressionCurve), "GetPrestigeLevel")]
public class PrestigeLevelsPatch : IRwbyGameplayPatch
{
    private static PrestigeLevel[] levels = makeLevels();

    public static bool Prefix(int __0, ref PrestigeLevel __result)
    {
        if (levels.Length - 1 < __0) {
            RWBYAP.Logger.LogInfo("entry not available...");
            __result = new();
            return false;
        }
        RWBYAP.Logger.LogInfo($"returning {__0} // {levels[__0]}");
        __result = levels[__0];
        return false;
    }

    private static PrestigeLevel[] makeLevels()
    {
        RWBYAP.Logger.LogInfo($"setting up prestige level info for max {Roost.Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.DefaultProgressionCurve.GetMaxPrestigeLevel()}");
        var levels = new PrestigeLevel[Roost.Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.DefaultProgressionCurve.GetMaxPrestigeLevel()];
        for (var i = 0; i < Roost.Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.DefaultProgressionCurve.GetMaxPrestigeLevel(); i++)
        {
            levels[i] = levelBase(i);
        }
        return levels;
    }

    private static PrestigeLevel levelBase(int level)
    {
        RWBYAP.Logger.LogInfo($"setting up prestige level info for level {level}");
        var l = new PrestigeLevel();
        var orig = Roost.Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.DefaultProgressionCurve.GetPrestigeLevel(level);
        l.Icon = orig.Icon;
        l.HUDIcon = orig.HUDIcon;
        l.hideFlags = orig.hideFlags;
        l.name = orig.name;
        l.AwardDescriptionLocalizationKey = orig.AwardDescriptionLocalizationKey;
        l.PrerequisiteAchievements = new Achievement[orig.PrerequisiteAchievements.Length];
        for (var i = 0; i < orig.PrerequisiteAchievements.Length; i++)
        {
            l.PrerequisiteAchievements[i] = new();
            l.PrerequisiteAchievements[i].DescriptionLocalizationKey = orig.PrerequisiteAchievements[i].DescriptionLocalizationKey;
            l.PrerequisiteAchievements[i].ID = orig.PrerequisiteAchievements[i].ID;
            l.PrerequisiteAchievements[i].IsMasteryChallenge = orig.PrerequisiteAchievements[i].IsMasteryChallenge;
            l.PrerequisiteAchievements[i].IsMaxLevelAchievement = orig.PrerequisiteAchievements[i].IsMaxLevelAchievement;
            l.PrerequisiteAchievements[i].MaxLevelChallengeQuantity = orig.PrerequisiteAchievements[i].MaxLevelChallengeQuantity;
            l.PrerequisiteAchievements[i].NameLocalizationKey = orig.PrerequisiteAchievements[i].NameLocalizationKey;
            l.PrerequisiteAchievements[i].RequiredStatQuantity = orig.PrerequisiteAchievements[i].RequiredStatQuantity;
            l.PrerequisiteAchievements[i].RequiredStatType = orig.PrerequisiteAchievements[i].RequiredStatType;
            l.PrerequisiteAchievements[i].UnlockPath = orig.PrerequisiteAchievements[i].UnlockPath;
            l.PrerequisiteAchievements[i].hideFlags = orig.PrerequisiteAchievements[i].hideFlags;
            l.PrerequisiteAchievements[i].name = orig.PrerequisiteAchievements[i].name;
        }
        l.PrerequisiteAchievements = l.PrerequisiteAchievements.Where(achievement => achievement.IsMaxLevelAchievement).ToArray();
        return l;
    }
}

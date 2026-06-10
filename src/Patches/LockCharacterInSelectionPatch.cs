using HarmonyLib;
using Roost;
using System.Collections.Generic;
using System.Linq;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(CharacterSelectionController), "UpdateEntryImagesAndName")]
public class LockCharacterInSelectionPatch : IRwbyGameplayPatch
{
    private static Dictionary<string, int> CharacterIDs = new() {
        {"Rubyb2cc", 200},
        {"Weis9ad1", 300},
        {"Blak8346", 400},
        {"Yangcde5", 500},
        {"Jaun5986", -1},
        {"Noracf67", -1},
        {"Pyrrfad9", -1},
        {"Ren 7bb0", -1},
    };

    public static void Postfix(CharacterSelectionController __instance, CharacterSelectionWidget ___m_characterSelectionWidget, int ___m_teamSelectionIndex, int ___m_costumeSelectionIndex, ref CharacterSelectionEntry ___m_currentSelectedEntry)
    {
        for (int i = 0; i < ___m_characterSelectionWidget.CharacterEntries.Length; i++)
        {
            CharacterSelectionEntry characterSelectionEntry = ___m_characterSelectionWidget.CharacterEntries[i];
            PlayableCharacterDefinition playableCharacterDefinition = Singleton_MonoBehaviour<GameManager>.Instance.GameplayDatabase.PlayableTeams.GetOrderedPlayableTeamDefinitions()[___m_teamSelectionIndex].TeamRoster[i];
            bool unlocked = RWBYAP.Connection.Items.AllItemsReceived.Select(item => item.ItemId).Contains(CharacterIDs.GetValueSafe(playableCharacterDefinition.ID));
            characterSelectionEntry.Button.enabled = unlocked;
            if (!unlocked)
            {
                var sprites = characterSelectionEntry.Button.spriteState;
                sprites.highlightedSprite = playableCharacterDefinition.GetCharacterSelectionNonHighlightImage(___m_costumeSelectionIndex);
                characterSelectionEntry.Button.spriteState = sprites;
            }
        }
    }
}

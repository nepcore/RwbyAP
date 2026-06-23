from dataclasses import dataclass
from Options import Accessibility, OptionGroup, OptionSet, PerGameCommonOptions, ProgressionBalancing, Range, Toggle

class ArtifactsInPool(Range):
    """How many artifacts should be in the item pool"""

    display_name = "Number of artifacts in the pool"

    range_start = 0
    range_end = 100
    default = 50

class ArtifactsRequiredPercentage(Range):
    """How many of the artifacts in the pool you need to goal in percent"""

    display_name = "Percentage of artifacts required to goal"

    range_start = 0
    range_end = 100
    default = 80

class MaxCharacters(Range):
    """How many characters should at most be included in the generated seed"""

    display_name = "Max characters"

    range_start = 1
    range_end = 8
    default = 8

class CharacterLevelChecks(Range):
    """How many level up checks should be included per character"""

    display_name = "Level up checks per character"

    range_start = 2
    range_end = 10
    default = 10

class JnprEnabled(Toggle):
    """If characters from the team JNPR DLC should be added to the item pool"""

    display_name = "JNPR DLC enabled"

class CharactersDisabled(OptionSet):
    """Characters in this list will never appear in the generated seed"""

    display_name = "Characters disabled"

    valid_keys = ["Ruby", "Weiss", "Blake", "Yang", "Jaune", "Nora", "Pyrrha", "Ren"]

@dataclass
class RWBYOptions(PerGameCommonOptions):
    artifacts_in_pool: ArtifactsInPool
    artifacts_required_percentage: ArtifactsRequiredPercentage
    max_characters: MaxCharacters
    character_level_checks: CharacterLevelChecks
    jnpr_enabled: JnprEnabled
    characters_disabled: CharactersDisabled

option_groups = [
    OptionGroup("Artifacts", [
        ArtifactsInPool,
        ArtifactsRequiredPercentage,
    ]),
    OptionGroup("Characters", [
        MaxCharacters,
        CharacterLevelChecks,
        JnprEnabled,
        CharactersDisabled,
    ]),
    OptionGroup("Advanced", [
        ProgressionBalancing,
        Accessibility,
    ])
]
option_presets = {}

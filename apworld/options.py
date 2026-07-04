from dataclasses import dataclass
from Options import Accessibility, Choice, DeathLink, OptionGroup, OptionSet, PerGameCommonOptions, ProgressionBalancing, Range, Toggle

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

class StartingCharacters(Range):
    """How many characters should be available at the start"""

    display_name = "Starting characters"

    range_start = 1
    range_end = 8
    default = 1

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

class DeathLinkReceiveMode(Choice):
    """
    How incoming deathlinks should be handled
    "single" downs a single random player
    "all" downs all players
    """

    display_name = "Death Link Receive Mode"

    option_single = 1
    option_all = 2

    default = 1

class DeathLinkSendMode(Choice):
    """
    When death links should be sent out
    "single" sends a death link whenever any player goes down
    "all" sends a death link when all players are downed
    """

    display_name = "Death Link Send Mode"

    option_single = 1
    option_all = 2

    default = 2

@dataclass
class RWBYOptions(PerGameCommonOptions):
    artifacts_in_pool: ArtifactsInPool
    artifacts_required_percentage: ArtifactsRequiredPercentage
    max_characters: MaxCharacters
    starting_characters: StartingCharacters
    character_level_checks: CharacterLevelChecks
    jnpr_enabled: JnprEnabled
    characters_disabled: CharactersDisabled
    death_link: DeathLink
    death_link_receive_mode: DeathLinkReceiveMode
    death_link_send_mode: DeathLinkSendMode

option_groups = [
    OptionGroup("Artifacts", [
        ArtifactsInPool,
        ArtifactsRequiredPercentage,
    ]),
    OptionGroup("Characters", [
        MaxCharacters,
        StartingCharacters,
        CharacterLevelChecks,
        JnprEnabled,
        CharactersDisabled,
    ]),
    OptionGroup("Death Link", [
        DeathLink,
        DeathLinkReceiveMode,
        DeathLinkSendMode,
    ]),
    OptionGroup("Advanced", [
        ProgressionBalancing,
        Accessibility,
    ])
]
option_presets = {}

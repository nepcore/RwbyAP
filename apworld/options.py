from dataclasses import dataclass
from Options import Accessibility, OptionGroup, PerGameCommonOptions, ProgressionBalancing, Range

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

@dataclass
class RWBYOptions(PerGameCommonOptions):
    artifacts_in_pool: ArtifactsInPool
    artifacts_required_percentage: ArtifactsRequiredPercentage

option_groups = [
    OptionGroup("Artifacts", [
        ArtifactsInPool,
        ArtifactsRequiredPercentage,
    ]),
    OptionGroup("Advanced", [
        ProgressionBalancing,
        Accessibility,
    ])
]
option_presets = {}

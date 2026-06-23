from __future__ import annotations
from typing import TYPE_CHECKING
from rule_builder.rules import Has, HasAll, HasFromListUnique, Or, True_
from math import floor
if TYPE_CHECKING:
    from .world import RWBYWorld

def set_all_rules(world: RWBYWorld) -> None:
    chars = ["Ruby", "Weiss", "Blake", "Yang"]
    if world.options.jnpr_enabled:
        chars += ["Jaune", "Nora", "Pyrrha", "Ren"]

    levels = [
        "Chapter Unlocked: Boots on the Ground",
        "Chapter Unlocked: Technical Difficulties",
        "Chapter Unlocked: The Collapse",
        "Chapter Unlocked: Below the Surface",
        "Chapter Unlocked: Right on Track",
        "Chapter Unlocked: End of the Line",
        "Chapter Unlocked: The Island of Dr. Merlot",
        "Chapter Unlocked: A Grimm Discovery",
        "Chapter Unlocked: The Grand Tour",
        "Chapter Unlocked: Final Exam",
    ]

    for char in chars:
        for i in range(2, world.options.character_level_checks + 1):
            world.set_rule(world.get_location(f"{char} - Level {i}"), HasFromListUnique(*levels, count = i - 1))

    skills = Or(
        *[HasAll(f"Character Unlocked: {char}", f"{char} - Aura Regeneration", f"{char} - Increased Aura") for char in chars]
    )

    artifacts = True_()
    if world.options.artifacts_in_pool > 0 and world.options.artifacts_required_percentage > 0:
        artifacts_required = floor(world.options.artifacts_in_pool * (world.options.artifacts_required_percentage / 100))
        artifacts = Has("Artifact", count = artifacts_required)

    defeat_mutant_deathstalker = Has("Chapter Unlocked: Final Exam") & skills & artifacts

    world.set_rule(world.get_location("Final Exam - Completed"), defeat_mutant_deathstalker)
    world.set_completion_rule(defeat_mutant_deathstalker)

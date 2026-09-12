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

    chars = [name for name in chars if name not in world.options.characters_disabled]

    levels = [
        "Boots on the Ground",
        "Technical Difficulties",
        "The Collapse",
        "Below the Surface",
        "Right on Track",
        "End of the Line",
        "The Island of Dr. Merlot",
        "A Grimm Discovery",
        "The Grand Tour",
        "Final Exam",
    ]

    for char in chars:
        for i in range(2, world.options.character_level_checks + 1):
            world.set_rule(world.get_location(f"{char} - Level {i}"), HasFromListUnique(*[f"Chapter Unlocked: {level}" for level in levels], count = i - 1))

    skills = Or(
        *[HasAll(f"Character Unlocked: {char}", f"{char} - Aura Regeneration", f"{char} - Increased Aura") for char in chars]
    )

    artifacts = True_()
    if world.options.artifacts_in_pool > 0 and world.options.artifacts_required_percentage > 0:
        artifacts_required = floor(world.options.artifacts_in_pool * (world.options.artifacts_required_percentage / 100))
        artifacts = Has("Artifact", count = artifacts_required)

    level_completions = True_()
    if world.options.level_completions_required > 0:
        level_completions = HasFromListUnique(*[f"Level Complete - {level}" for level in levels], count = world.options.level_completions_required.value)

    defeat_mutant_deathstalker = Has("Chapter Unlocked: Final Exam") & skills & artifacts & level_completions

    world.set_rule(world.get_location("Final Exam - Completed"), defeat_mutant_deathstalker)
    world.set_completion_rule(defeat_mutant_deathstalker)

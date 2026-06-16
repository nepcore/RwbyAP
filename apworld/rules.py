from __future__ import annotations
from typing import TYPE_CHECKING
from rule_builder.rules import Has, HasAll, Or
if TYPE_CHECKING:
    from .world import RWBYWorld

def set_all_rules(world: RWBYWorld) -> None:
    skills = Or(
        *[HasAll(f"Character Unlocked: {char}", f"{char} - Aura Regeneration", f"{char} - Increased Aura") for char in ["Ruby", "Weiss", "Blake", "Yang"]]
    )

    defeat_mutant_deathstalker = Has("Chapter Unlocked: Final Exam") & skills

    world.set_rule(world.get_location("Final Exam - Golden Knight"), defeat_mutant_deathstalker)
    world.set_rule(world.get_location("Final Exam - Completed"), defeat_mutant_deathstalker)
    world.set_completion_rule(defeat_mutant_deathstalker)

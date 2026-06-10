from __future__ import annotations
from typing import TYPE_CHECKING
from BaseClasses import CollectionState
from rule_builder.rules import CanReachLocation
if TYPE_CHECKING:
    from .world import RWBYWorld

def set_all_rules(world: RWBYWorld) -> None:
    world.set_completion_rule(CanReachLocation("Technical Difficulties - Completed"))

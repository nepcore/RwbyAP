from __future__ import annotations
from typing import TYPE_CHECKING
from BaseClasses import Entrance, Region
from rule_builder.rules import Has
if TYPE_CHECKING:
    from .world import RWBYWorld

def create_and_connect_regions(world: RWBYWorld) -> None:
    menu = Region("Menu", world.player, world.multiworld)

    ch1 = Region("Boots on the Ground", world.player, world.multiworld)
    menu.connect(ch1, "Boots on the Ground Entrance", Has("Chapter Unlocked: Boots on the Ground"))
    ch2 = Region("Technical Difficulties", world.player, world.multiworld)
    menu.connect(ch2, "Technical Difficulties Entrance", Has("Chapter Unlocked: Technical Difficulties"))
    ch3 = Region("The Collapse", world.player, world.multiworld)
    menu.connect(ch3, "The Collapse Entrance", Has("Chapter Unlocked: The Collapse"))
    ch4 = Region("Below the Surface", world.player, world.multiworld)
    menu.connect(ch4, "Below the Surface Entrance", Has("Chapter Unlocked: Below the Surface"))
    ch5 = Region("Right on Track", world.player, world.multiworld)
    menu.connect(ch5, "Right on Track Entrance", Has("Chapter Unlocked: Right on Track"))
    ch6 = Region("End of the Line", world.player, world.multiworld)
    menu.connect(ch6, "End of the Line Entrance", Has("Chapter Unlocked: End of the Line"))
    ch7 = Region("The Island of Dr. Merlot", world.player, world.multiworld)
    menu.connect(ch7, "The Island of Dr. Merlot Entrance", Has("Chapter Unlocked: The Island of Dr. Merlot"))
    ch8 = Region("A Grimm Discovery", world.player, world.multiworld)
    menu.connect(ch8, "A Grimm Discovery Entrance", Has("Chapter Unlocked: A Grimm Discovery"))
    ch9 = Region("The Grand Tour", world.player, world.multiworld)
    menu.connect(ch9, "The Grand Tour Entrance", Has("Chapter Unlocked: The Grand Tour"))
    ch10 = Region("Final Exam", world.player, world.multiworld)
    menu.connect(ch10, "Final Exam Entrance", Has("Chapter Unlocked: Final Exam"))

    ruby = Region("Ruby Level Up", world.player, world.multiworld)
    menu.connect(ruby, "Ruby Level Up Entrance", Has("Ruby Unlocked"))
    weiss = Region("Weiss Level Up", world.player, world.multiworld)
    menu.connect(weiss, "Weiss Level Up Entrance", Has("Weiss Unlocked"))
    blake = Region("Blake Level Up", world.player, world.multiworld)
    menu.connect(blake, "Blake Level Up Entrance", Has("Blake Unlocked"))
    yang = Region("Yang Level Up", world.player, world.multiworld)
    menu.connect(yang, "Yang Level Up Entrance", Has("Yang Unlocked"))

    world.multiworld.regions += [
        menu,
        ch1,
        ch2,
        ch3,
        ch4,
        ch5,
        ch6,
        ch7,
        ch8,
        ch9,
        ch10,
        ruby,
        weiss,
        blake,
        yang
    ]

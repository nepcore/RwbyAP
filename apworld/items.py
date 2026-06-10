from __future__ import annotations
from typing import TYPE_CHECKING
from BaseClasses import Item, ItemClassification
if TYPE_CHECKING:
    from .world import RWBYWorld

ITEM_NAME_TO_ID = {
    # Fillers
    "Ruby 10 XP": 1,
    "Weiss 10 XP": 2,
    "Blake 10 XP": 3,
    "Yang 10 XP": 4,
    # Stages
    "Chapter Unlocked: Boots on the Ground": 101,
    "Chapter Unlocked: Technical Difficulties": 102,
    "Chapter Unlocked: The Collapse": 103,
    "Chapter Unlocked: Below the Surface": 104,
    "Chapter Unlocked: Right on Track": 105,
    "Chapter Unlocked: End of the Line": 106,
    "Chapter Unlocked: The Island of Dr. Merlot": 107,
    "Chapter Unlocked: A Grimm Discovery": 108,
    "Chapter Unlocked: The Grand Tour": 109,
    "Chapter Unlocked: Final Exam": 110,
    # Ruby
    "Ruby Unlocked": 200,
    "Ruby Team Player": 201,
    "Ruby Hyperballistic": 202,
    "Ruby Progressive Ranged": 203,
    "Ruby Progressive Team Attack": 204,
    "Ruby Progressive Ultimate": 205,
    "Ruby Survivor": 206,
    "Ruby Heavy Synergy": 207,
    "Ruby Medic": 208,
    "Ruby Increased Aura": 209,
    "Ruby Aura Regeneration": 210,
    "Ruby Ultimate Charge": 211,
    # Weiss
    "Weiss Unlocked": 300,
    "Weiss Ice Queen": 301,
    "Weiss Perfect Form": 302,
    "Weiss Progressive Ranged": 303,
    "Weiss Progressive Team Attack": 304,
    "Weiss Progressive Ultimate": 305,
    "Weiss Survivor": 306,
    "Weiss Heavy Synergy": 307,
    "Weiss Medic": 308,
    "Weiss Increased Aura": 309,
    "Weiss Aura Regeneration": 310,
    "Weiss Ultimate Charge": 311,
    # Blake
    "Blake Unlocked": 400,
    "Blake Frenzy": 401,
    "Blake Fortunate Faunus": 402,
    "Blake Progressive Ranged": 403,
    "Blake Progressive Team Attack": 404,
    "Blake Progressive Ultimate": 405,
    "Blake Survivor": 406,
    "Blake Heavy Synergy": 407,
    "Blake Medic": 408,
    "Blake Increased Aura": 409,
    "Blake Aura Regeneration": 410,
    "Blake Ultimate Charge": 411,
    # Yang
    "Yang Unlocked": 500,
    "Yang Brawler": 501,
    "Yang Second Wind": 502,
    "Yang Progressive Ranged": 503,
    "Yang Progressive Team Attack": 504,
    "Yang Progressive Ultimate": 505,
    "Yang Survivor": 506,
    "Yang Heavy Synergy": 507,
    "Yang Medic": 508,
    "Yang Increased Aura": 509,
    "Yang Aura Regeneration": 510,
    "Yang Ultimate Charge": 511,
}

DEFAULT_ITEM_CLASSIFICATIONS = {
    # Fillers
    "Ruby 10 XP": ItemClassification.filler,
    "Weiss 10 XP": ItemClassification.filler,
    "Blake 10 XP": ItemClassification.filler,
    "Yang 10 XP": ItemClassification.filler,
    # Stages
    "Chapter Unlocked: Boots on the Ground": ItemClassification.progression,
    "Chapter Unlocked: Technical Difficulties": ItemClassification.progression,
    "Chapter Unlocked: The Collapse": ItemClassification.progression,
    "Chapter Unlocked: Below the Surface": ItemClassification.progression,
    "Chapter Unlocked: Right on Track": ItemClassification.progression,
    "Chapter Unlocked: End of the Line": ItemClassification.progression,
    "Chapter Unlocked: The Island of Dr. Merlot": ItemClassification.progression,
    "Chapter Unlocked: A Grimm Discovery": ItemClassification.progression,
    "Chapter Unlocked: The Grand Tour": ItemClassification.progression,
    "Chapter Unlocked: Final Exam": ItemClassification.progression,
    # Ruby
    "Ruby Unlocked": ItemClassification.progression,
    "Ruby Team Player": ItemClassification.useful,
    "Ruby Hyperballistic": ItemClassification.useful,
    "Ruby Progressive Ranged": ItemClassification.useful,
    "Ruby Progressive Team Attack": ItemClassification.useful,
    "Ruby Progressive Ultimate": ItemClassification.useful,
    "Ruby Survivor": ItemClassification.useful,
    "Ruby Heavy Synergy": ItemClassification.useful,
    "Ruby Medic": ItemClassification.useful,
    "Ruby Increased Aura": ItemClassification.useful,
    "Ruby Aura Regeneration": ItemClassification.useful,
    "Ruby Ultimate Charge": ItemClassification.useful,
    # Weiss
    "Weiss Unlocked": ItemClassification.progression,
    "Weiss Ice Queen": ItemClassification.useful,
    "Weiss Perfect Form": ItemClassification.useful,
    "Weiss Progressive Ranged": ItemClassification.useful,
    "Weiss Progressive Team Attack": ItemClassification.useful,
    "Weiss Progressive Ultimate": ItemClassification.useful,
    "Weiss Survivor": ItemClassification.useful,
    "Weiss Heavy Synergy": ItemClassification.useful,
    "Weiss Medic": ItemClassification.useful,
    "Weiss Increased Aura": ItemClassification.useful,
    "Weiss Aura Regeneration": ItemClassification.useful,
    "Weiss Ultimate Charge": ItemClassification.useful,
    # Blake
    "Blake Unlocked": ItemClassification.progression,
    "Blake Frenzy": ItemClassification.useful,
    "Blake Fortunate Faunus": ItemClassification.useful,
    "Blake Progressive Ranged": ItemClassification.useful,
    "Blake Progressive Team Attack": ItemClassification.useful,
    "Blake Progressive Ultimate": ItemClassification.useful,
    "Blake Survivor": ItemClassification.useful,
    "Blake Heavy Synergy": ItemClassification.useful,
    "Blake Medic": ItemClassification.useful,
    "Blake Increased Aura": ItemClassification.useful,
    "Blake Aura Regeneration": ItemClassification.useful,
    "Blake Ultimate Charge": ItemClassification.useful,
    # Yang
    "Yang Unlocked": ItemClassification.progression,
    "Yang Brawler": ItemClassification.useful,
    "Yang Second Wind": ItemClassification.useful,
    "Yang Progressive Ranged": ItemClassification.useful,
    "Yang Progressive Team Attack": ItemClassification.useful,
    "Yang Progressive Ultimate": ItemClassification.useful,
    "Yang Survivor": ItemClassification.useful,
    "Yang Heavy Synergy": ItemClassification.useful,
    "Yang Medic": ItemClassification.useful,
    "Yang Increased Aura": ItemClassification.useful,
    "Yang Aura Regeneration": ItemClassification.useful,
    "Yang Ultimate Charge": ItemClassification.useful,
}

skills = {
    "Ruby": [
        "Ruby Team Player",
        "Ruby Hyperballistic",
        "Ruby Progressive Ranged",
        "Ruby Progressive Ranged",
        "Ruby Progressive Team Attack",
        "Ruby Progressive Team Attack",
        "Ruby Progressive Ultimate",
        "Ruby Progressive Ultimate",
        "Ruby Survivor",
        "Ruby Heavy Synergy",
        "Ruby Medic",
        "Ruby Increased Aura",
        "Ruby Aura Regeneration",
        "Ruby Ultimate Charge",
        "Ruby Ultimate Charge",
    ],
    "Weiss": [
        "Weiss Ice Queen",
        "Weiss Perfect Form",
        "Weiss Progressive Ranged",
        "Weiss Progressive Ranged",
        "Weiss Progressive Team Attack",
        "Weiss Progressive Team Attack",
        "Weiss Progressive Ultimate",
        "Weiss Progressive Ultimate",
        "Weiss Survivor",
        "Weiss Heavy Synergy",
        "Weiss Medic",
        "Weiss Increased Aura",
        "Weiss Aura Regeneration",
        "Weiss Ultimate Charge",
        "Weiss Ultimate Charge",
    ],
    "Blake": [
        "Blake Frenzy",
        "Blake Fortunate Faunus",
        "Blake Progressive Ranged",
        "Blake Progressive Team Attack",
        "Blake Progressive Team Attack",
        "Blake Progressive Ultimate",
        "Blake Progressive Ultimate",
        "Blake Survivor",
        "Blake Heavy Synergy",
        "Blake Medic",
        "Blake Increased Aura",
        "Blake Aura Regeneration",
        "Blake Ultimate Charge",
        "Blake Ultimate Charge",
    ],
    "Yang": [
        "Yang Brawler",
        "Yang Second Wind",
        "Yang Progressive Ranged",
        "Yang Progressive Ranged",
        "Yang Progressive Team Attack",
        "Yang Progressive Team Attack",
        "Yang Progressive Ultimate",
        "Yang Progressive Ultimate",
        "Yang Survivor",
        "Yang Heavy Synergy",
        "Yang Medic",
        "Yang Increased Aura",
        "Yang Aura Regeneration",
        "Yang Ultimate Charge",
        "Yang Ultimate Charge",
    ],
}

class RWBYItem(Item):
    game = "RWBY Grimm Eclipse"

def get_random_filler_item_name(world: RWBYWorld) -> str:
    fillers = [name for name, classification in DEFAULT_ITEM_CLASSIFICATIONS.items() if classification == ItemClassification.filler]
    n = world.random.randint(0, len(fillers) - 1)
    return fillers[n]

def create_item_with_correct_classification(world: RWBYWorld, name: str) -> RWBYItem:
    return RWBYItem(name, DEFAULT_ITEM_CLASSIFICATIONS[name], ITEM_NAME_TO_ID[name], world.player)

def create_all_items(world: RWBYWorld) -> None:
    itempool: list[Item] = []
    precollected: list[Item] = []

    levels = [name for name, _ in ITEM_NAME_TO_ID.items() if name.startswith("Chapter Unlocked:")]
    precollected += levels[:1]
    itempool += levels[1:]

    characters = [
        "Ruby",
        "Weiss",
        "Blake",
        "Yang",
    ]
    world.random.shuffle(characters)

    for character in characters:
        skillpool = skills[character]
        world.random.shuffle(skillpool)
        itempool += skillpool[:16]

    precollected += [f"{characters.pop()} Unlocked"]
    itempool += [f"{name} Unlocked" for name in characters]

    itempool = [name for name in itempool if not name in precollected]

    n_items = len(itempool)
    n_locations = len(world.multiworld.get_unfilled_locations(world.player))
    n_fillers = n_locations - n_items
    itempool += [get_random_filler_item_name(world) for _ in range(n_fillers)]

    world.multiworld.itempool += [world.create_item(name) for name in itempool]
    for precollected_item_name in precollected:
        world.push_precollected(world.create_item(precollected_item_name))

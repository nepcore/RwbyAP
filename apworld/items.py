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
    "Jaune 10 XP": 5,
    "Nora 10 XP": 6,
    "Pyrrha 10 XP": 7,
    "Ren 10 XP": 8,
    # Progression
    "Artifact": 99,
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
    "Character Unlocked: Ruby": 200,
    "Ruby - Team Player": 201,
    "Ruby - Hyperballistic": 202,
    "Ruby - Progressive Ranged": 203,
    "Ruby - Progressive Team Attack": 204,
    "Ruby - Progressive Ultimate": 205,
    "Ruby - Survivor": 206,
    "Ruby - Heavy Synergy": 207,
    "Ruby - Medic": 208,
    "Ruby - Increased Aura": 209,
    "Ruby - Aura Regeneration": 210,
    "Ruby - Ultimate Charge": 211,
    # Weiss
    "Character Unlocked: Weiss": 300,
    "Weiss - Ice Queen": 301,
    "Weiss - Perfect Form": 302,
    "Weiss - Progressive Ranged": 303,
    "Weiss - Progressive Team Attack": 304,
    "Weiss - Progressive Ultimate": 305,
    "Weiss - Survivor": 306,
    "Weiss - Heavy Synergy": 307,
    "Weiss - Medic": 308,
    "Weiss - Increased Aura": 309,
    "Weiss - Aura Regeneration": 310,
    "Weiss - Ultimate Charge": 311,
    # Blake
    "Character Unlocked: Blake": 400,
    "Blake - Frenzy": 401,
    "Blake - Fortunate Faunus": 402,
    "Blake - Progressive Ranged": 403,
    "Blake - Progressive Team Attack": 404,
    "Blake - Progressive Ultimate": 405,
    "Blake - Survivor": 406,
    "Blake - Heavy Synergy": 407,
    "Blake - Medic": 408,
    "Blake - Increased Aura": 409,
    "Blake - Aura Regeneration": 410,
    "Blake - Ultimate Charge": 411,
    # Yang
    "Character Unlocked: Yang": 500,
    "Yang - Brawler": 501,
    "Yang - Second Wind": 502,
    "Yang - Progressive Ranged": 503,
    "Yang - Progressive Team Attack": 504,
    "Yang - Progressive Ultimate": 505,
    "Yang - Survivor": 506,
    "Yang - Heavy Synergy": 507,
    "Yang - Medic": 508,
    "Yang - Increased Aura": 509,
    "Yang - Aura Regeneration": 510,
    "Yang - Ultimate Charge": 511,
    # Jaune
    "Character Unlocked: Jaune": 600,
    "Jaune - Best Teammate Ever": 601,
    "Jaune - Team Synergy": 602,
    "Jaune - Progressive Heavy": 603,
    "Jaune - Progressive Team Attack": 604,
    "Jaune - Progressive Ultimate": 605,
    "Jaune - Survivor": 606,
    "Jaune - Heavy Synergy": 607,
    "Jaune - Medic": 608,
    "Jaune - Increased Aura": 609,
    "Jaune - Aura Regeneration": 610,
    "Jaune - Ultimate Charge": 611,
    # Nora
    "Character Unlocked: Nora": 700,
    "Nora - Power Surge": 701,
    "Nora - Lightning Chain": 702,
    "Nora - Progressive Ranged": 703,
    "Nora - Progressive Team Attack": 704,
    "Nora - Progressive Ultimate": 705,
    "Nora - Survivor": 706,
    "Nora - Heavy Synergy": 707,
    "Nora - Medic": 708,
    "Nora - Increased Aura": 709,
    "Nora - Aura Regeneration": 710,
    "Nora - Ultimate Charge": 711,
    # Pyrrha
    "Character Unlocked: Pyrrha": 800,
    "Pyrrha - Ricochet": 801,
    "Pyrrha - Long Distance Toss": 802,
    "Pyrrha - Progressive Combo": 803,
    "Pyrrha - Progressive Team Attack": 804,
    "Pyrrha - Progressive Ultimate": 805,
    "Pyrrha - Survivor": 806,
    "Pyrrha - Heavy Synergy": 807,
    "Pyrrha - Medic": 808,
    "Pyrrha - Increased Aura": 809,
    "Pyrrha - Aura Regeneration": 810,
    "Pyrrha - Ultimate Charge": 811,
    # Ren
    "Character Unlocked: Ren": 900,
    "Ren - Extra Ammo": 901,
    "Ren - Heavy Hands": 902,
    "Ren - Progressive Heavy": 903,
    "Ren - Progressive Team Attack": 904,
    "Ren - Progressive Ultimate": 905,
    "Ren - Survivor": 906,
    "Ren - Heavy Synergy": 907,
    "Ren - Medic": 908,
    "Ren - Increased Aura": 909,
    "Ren - Aura Regeneration": 910,
    "Ren - Ultimate Charge": 911,
}

DEFAULT_ITEM_CLASSIFICATIONS = {
    # Fillers
    "Ruby 10 XP": ItemClassification.filler,
    "Weiss 10 XP": ItemClassification.filler,
    "Blake 10 XP": ItemClassification.filler,
    "Yang 10 XP": ItemClassification.filler,
    "Jaune 10 XP": ItemClassification.filler,
    "Nora 10 XP": ItemClassification.filler,
    "Pyrrha 10 XP": ItemClassification.filler,
    "Ren 10 XP": ItemClassification.filler,
    # Progression
    "Artifact": ItemClassification.progression,
    # Stages
    "Chapter Unlocked: Boots on the Ground": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: Technical Difficulties": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: The Collapse": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: Below the Surface": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: Right on Track": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: End of the Line": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: The Island of Dr. Merlot": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: A Grimm Discovery": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: The Grand Tour": ItemClassification.progression | ItemClassification.useful,
    "Chapter Unlocked: Final Exam": ItemClassification.progression | ItemClassification.useful,
    # Ruby
    "Character Unlocked: Ruby": ItemClassification.progression | ItemClassification.useful,
    "Ruby - Team Player": ItemClassification.useful,
    "Ruby - Hyperballistic": ItemClassification.useful,
    "Ruby - Progressive Ranged": ItemClassification.useful,
    "Ruby - Progressive Team Attack": ItemClassification.useful,
    "Ruby - Progressive Ultimate": ItemClassification.useful,
    "Ruby - Survivor": ItemClassification.useful,
    "Ruby - Heavy Synergy": ItemClassification.useful,
    "Ruby - Medic": ItemClassification.useful,
    "Ruby - Increased Aura": ItemClassification.progression,
    "Ruby - Aura Regeneration": ItemClassification.progression,
    "Ruby - Ultimate Charge": ItemClassification.useful,
    # Weiss
    "Character Unlocked: Weiss": ItemClassification.progression | ItemClassification.useful,
    "Weiss - Ice Queen": ItemClassification.useful,
    "Weiss - Perfect Form": ItemClassification.useful,
    "Weiss - Progressive Ranged": ItemClassification.useful,
    "Weiss - Progressive Team Attack": ItemClassification.useful,
    "Weiss - Progressive Ultimate": ItemClassification.useful,
    "Weiss - Survivor": ItemClassification.useful,
    "Weiss - Heavy Synergy": ItemClassification.useful,
    "Weiss - Medic": ItemClassification.useful,
    "Weiss - Increased Aura": ItemClassification.progression,
    "Weiss - Aura Regeneration": ItemClassification.progression,
    "Weiss - Ultimate Charge": ItemClassification.useful,
    # Blake
    "Character Unlocked: Blake": ItemClassification.progression | ItemClassification.useful,
    "Blake - Frenzy": ItemClassification.useful,
    "Blake - Fortunate Faunus": ItemClassification.useful,
    "Blake - Progressive Ranged": ItemClassification.useful,
    "Blake - Progressive Team Attack": ItemClassification.useful,
    "Blake - Progressive Ultimate": ItemClassification.useful,
    "Blake - Survivor": ItemClassification.useful,
    "Blake - Heavy Synergy": ItemClassification.useful,
    "Blake - Medic": ItemClassification.useful,
    "Blake - Increased Aura": ItemClassification.progression,
    "Blake - Aura Regeneration": ItemClassification.progression,
    "Blake - Ultimate Charge": ItemClassification.useful,
    # Yang
    "Character Unlocked: Yang": ItemClassification.progression | ItemClassification.useful,
    "Yang - Brawler": ItemClassification.useful,
    "Yang - Second Wind": ItemClassification.useful,
    "Yang - Progressive Ranged": ItemClassification.useful,
    "Yang - Progressive Team Attack": ItemClassification.useful,
    "Yang - Progressive Ultimate": ItemClassification.useful,
    "Yang - Survivor": ItemClassification.useful,
    "Yang - Heavy Synergy": ItemClassification.useful,
    "Yang - Medic": ItemClassification.useful,
    "Yang - Increased Aura": ItemClassification.progression,
    "Yang - Aura Regeneration": ItemClassification.progression,
    "Yang - Ultimate Charge": ItemClassification.useful,
    # Jaune
    "Character Unlocked: Jaune": ItemClassification.progression | ItemClassification.useful,
    "Jaune - Best Teammate Ever": ItemClassification.useful,
    "Jaune - Team Synergy": ItemClassification.useful,
    "Jaune - Progressive Heavy": ItemClassification.useful,
    "Jaune - Progressive Team Attack": ItemClassification.useful,
    "Jaune - Progressive Ultimate": ItemClassification.useful,
    "Jaune - Survivor": ItemClassification.useful,
    "Jaune - Heavy Synergy": ItemClassification.useful,
    "Jaune - Medic": ItemClassification.useful,
    "Jaune - Increased Aura": ItemClassification.progression,
    "Jaune - Aura Regeneration": ItemClassification.progression,
    "Jaune - Ultimate Charge": ItemClassification.useful,
    # Nora
    "Character Unlocked: Nora": ItemClassification.progression | ItemClassification.useful,
    "Nora - Power Surge": ItemClassification.useful,
    "Nora - Lightning Chain": ItemClassification.useful,
    "Nora - Progressive Ranged": ItemClassification.useful,
    "Nora - Progressive Team Attack": ItemClassification.useful,
    "Nora - Progressive Ultimate": ItemClassification.useful,
    "Nora - Survivor": ItemClassification.useful,
    "Nora - Heavy Synergy": ItemClassification.useful,
    "Nora - Medic": ItemClassification.useful,
    "Nora - Increased Aura": ItemClassification.progression,
    "Nora - Aura Regeneration": ItemClassification.progression,
    "Nora - Ultimate Charge": ItemClassification.useful,
    # Pyrrha
    "Character Unlocked: Pyrrha": ItemClassification.progression | ItemClassification.useful,
    "Pyrrha - Ricochet": ItemClassification.useful,
    "Pyrrha - Long Distance Toss": ItemClassification.useful,
    "Pyrrha - Progressive Combo": ItemClassification.useful,
    "Pyrrha - Progressive Team Attack": ItemClassification.useful,
    "Pyrrha - Progressive Ultimate": ItemClassification.useful,
    "Pyrrha - Survivor": ItemClassification.useful,
    "Pyrrha - Heavy Synergy": ItemClassification.useful,
    "Pyrrha - Medic": ItemClassification.useful,
    "Pyrrha - Increased Aura": ItemClassification.progression,
    "Pyrrha - Aura Regeneration": ItemClassification.progression,
    "Pyrrha - Ultimate Charge": ItemClassification.useful,
    # Ren
    "Character Unlocked: Ren": ItemClassification.progression | ItemClassification.useful,
    "Ren - Extra Ammo": ItemClassification.useful,
    "Ren - Heavy Hands": ItemClassification.useful,
    "Ren - Progressive Heavy": ItemClassification.useful,
    "Ren - Progressive Team Attack": ItemClassification.useful,
    "Ren - Progressive Ultimate": ItemClassification.useful,
    "Ren - Survivor": ItemClassification.useful,
    "Ren - Heavy Synergy": ItemClassification.useful,
    "Ren - Medic": ItemClassification.useful,
    "Ren - Increased Aura": ItemClassification.progression,
    "Ren - Aura Regeneration": ItemClassification.progression,
    "Ren - Ultimate Charge": ItemClassification.useful,
}

skills = {
    "Ruby": [
        "Ruby - Team Player",
        "Ruby - Hyperballistic",
        "Ruby - Progressive Ranged",
        "Ruby - Progressive Ranged",
        "Ruby - Progressive Team Attack",
        "Ruby - Progressive Team Attack",
        "Ruby - Progressive Ultimate",
        "Ruby - Progressive Ultimate",
        "Ruby - Survivor",
        "Ruby - Heavy Synergy",
        "Ruby - Medic",
        "Ruby - Increased Aura",
        "Ruby - Aura Regeneration",
        "Ruby - Ultimate Charge",
        "Ruby - Ultimate Charge",
    ],
    "Weiss": [
        "Weiss - Ice Queen",
        "Weiss - Perfect Form",
        "Weiss - Progressive Ranged",
        "Weiss - Progressive Ranged",
        "Weiss - Progressive Team Attack",
        "Weiss - Progressive Team Attack",
        "Weiss - Progressive Ultimate",
        "Weiss - Progressive Ultimate",
        "Weiss - Survivor",
        "Weiss - Heavy Synergy",
        "Weiss - Medic",
        "Weiss - Increased Aura",
        "Weiss - Aura Regeneration",
        "Weiss - Ultimate Charge",
        "Weiss - Ultimate Charge",
    ],
    "Blake": [
        "Blake - Frenzy",
        "Blake - Fortunate Faunus",
        "Blake - Progressive Ranged",
        "Blake - Progressive Team Attack",
        "Blake - Progressive Team Attack",
        "Blake - Progressive Ultimate",
        "Blake - Progressive Ultimate",
        "Blake - Survivor",
        "Blake - Heavy Synergy",
        "Blake - Medic",
        "Blake - Increased Aura",
        "Blake - Aura Regeneration",
        "Blake - Ultimate Charge",
        "Blake - Ultimate Charge",
    ],
    "Yang": [
        "Yang - Brawler",
        "Yang - Second Wind",
        "Yang - Progressive Ranged",
        "Yang - Progressive Ranged",
        "Yang - Progressive Team Attack",
        "Yang - Progressive Team Attack",
        "Yang - Progressive Ultimate",
        "Yang - Progressive Ultimate",
        "Yang - Survivor",
        "Yang - Heavy Synergy",
        "Yang - Medic",
        "Yang - Increased Aura",
        "Yang - Aura Regeneration",
        "Yang - Ultimate Charge",
        "Yang - Ultimate Charge",
    ],
    "Jaune": [
        "Jaune - Best Teammate Ever",
        "Jaune - Team Synergy",
        "Jaune - Progressive Heavy",
        "Jaune - Progressive Heavy",
        "Jaune - Progressive Team Attack",
        "Jaune - Progressive Team Attack",
        "Jaune - Progressive Ultimate",
        "Jaune - Survivor",
        "Jaune - Heavy Synergy",
        "Jaune - Medic",
        "Jaune - Increased Aura",
        "Jaune - Aura Regeneration",
        "Jaune - Ultimate Charge",
        "Jaune - Ultimate Charge",
    ],
    "Nora": [
        "Nora - Power Surge",
        "Nora - Lightning Chain",
        "Nora - Progressive Ranged",
        "Nora - Progressive Ranged",
        "Nora - Progressive Team Attack",
        "Nora - Progressive Team Attack",
        "Nora - Progressive Ultimate",
        "Nora - Progressive Ultimate",
        "Nora - Survivor",
        "Nora - Heavy Synergy",
        "Nora - Medic",
        "Nora - Increased Aura",
        "Nora - Aura Regeneration",
        "Nora - Ultimate Charge",
        "Nora - Ultimate Charge",
    ],
    "Pyrrha": [
        "Pyrrha - Ricochet",
        "Pyrrha - Long Distance Toss",
        "Pyrrha - Progressive Combo",
        "Pyrrha - Progressive Combo",
        "Pyrrha - Progressive Team Attack",
        "Pyrrha - Progressive Team Attack",
        "Pyrrha - Progressive Ultimate",
        "Pyrrha - Progressive Ultimate",
        "Pyrrha - Survivor",
        "Pyrrha - Heavy Synergy",
        "Pyrrha - Medic",
        "Pyrrha - Increased Aura",
        "Pyrrha - Aura Regeneration",
        "Pyrrha - Ultimate Charge",
        "Pyrrha - Ultimate Charge",
    ],
    "Ren": [
        "Ren - Extra Ammo",
        "Ren - Heavy Hands",
        "Ren - Progressive Heavy",
        "Ren - Progressive Heavy",
        "Ren - Progressive Team Attack",
        "Ren - Progressive Team Attack",
        "Ren - Progressive Ultimate",
        "Ren - Progressive Ultimate",
        "Ren - Survivor",
        "Ren - Heavy Synergy",
        "Ren - Medic",
        "Ren - Increased Aura",
        "Ren - Aura Regeneration",
        "Ren - Ultimate Charge",
        "Ren - Ultimate Charge",
    ],
}

class RWBYItem(Item):
    game = "RWBY Grimm Eclipse"

def get_random_filler_item_name(world: RWBYWorld) -> str:
    fillers = ["Ruby 10 XP", "Weiss 10 XP", "Blake 10 XP", "Yang 10 XP"]
    if world.options.jnpr_enabled:
        fillers += ["Jaune 10 XP", "Nora 10 XP", "Pyrrha 10 XP", "Ren 10 XP"]
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

    characters = ["Ruby", "Weiss", "Blake", "Yang"]
    if world.options.jnpr_enabled:
        characters += ["Jaune", "Nora", "Pyrrha", "Ren"]

    world.random.shuffle(characters)

    for character in characters:
        skillpool = skills[character]
        world.random.shuffle(skillpool)
        itempool += skillpool[:13]

    precollected += [f"Character Unlocked: {characters.pop()}"]
    itempool += [f"Character Unlocked: {name}" for name in characters]

    itempool += (["Artifact"] * world.options.artifacts_in_pool)

    itempool = [name for name in itempool if not name in precollected]

    n_items = len(itempool)
    n_locations = len(world.multiworld.get_unfilled_locations(world.player))
    n_fillers = n_locations - n_items
    itempool += [get_random_filler_item_name(world) for _ in range(n_fillers)]

    world.multiworld.itempool += [world.create_item(name) for name in itempool]
    for precollected_item_name in precollected:
        world.push_precollected(world.create_item(precollected_item_name))

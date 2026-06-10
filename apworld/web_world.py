from BaseClasses import Tutorial
from worlds.AutoWorld import WebWorld
from .options import option_groups, option_presets

class RWBYWebWorld(WebWorld):
    game = "RWBY Grimm Eclipse"
    theme = "stone"
    setup_en = Tutorial(
        "Multiworld Setup Guide",
        "A guide to setting up RWBY Grimm Eclipse for MultiWorld.",
        "English",
        "setup_en.md",
        "setup/en",
        ["nepcore"],
    )
    tutorials = [setup_en]
    option_groups = option_groups
    options_presets = option_presets

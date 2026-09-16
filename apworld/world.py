from collections.abc import Mapping
from typing import Any
from worlds.AutoWorld import World
from . import items, locations, regions, rules, web_world
from . import options as rwby_options
from Options import OptionError

class RWBYWorld(World):
    """
    RWBY Grimm Eclipse is a 4 player, online co-op, hack and slash game based upon Rooster Teeth’s international hit series RWBY.
    """

    game = "RWBY Grimm Eclipse"
    web = web_world.RWBYWebWorld()
    options_dataclass = rwby_options.RWBYOptions
    options: rwby_options.RWBYOptions

    location_name_to_id = locations.LOCATION_NAME_TO_ID
    item_name_to_id = items.ITEM_NAME_TO_ID

    ut_can_gen_without_yaml = True

    def create_regions(self) -> None:
        regions.create_and_connect_regions(self)
        locations.create_all_locations(self)

    def set_rules(self) -> None:
        rules.set_all_rules(self)

    def create_items(self) -> None:
        items.create_all_items(self)

    def create_item(self, name: str) -> items.RWBYItem:
        return items.create_item_with_correct_classification(self, name)

    def get_filler_item_name(self) -> str:
        return items.get_random_filler_item_name(self)

    def fill_slot_data(self) -> Mapping[str, Any]:
        data = self.options.as_dict(
            "artifacts_in_pool",
            "artifacts_required_percentage",
            "level_completions_required",
            "jnpr_enabled",
            "death_link",
            "death_link_receive_mode",
            "death_link_send_mode"
        )
        data["characters"] = self.characters
        return data

    def generate_early(self) -> None:
        # if in ut get options from slot data
        re_gen_passthrough = getattr(self.multiworld, "re_gen_passthrough", {})
        if re_gen_passthrough and self.game in re_gen_passthrough:
            slot_data: dict[str, Any] = re_gen_passthrough[self.game]
            for key in ["artifacts_in_pool", "artifacts_required_percentage", "level_completions_required", "jnpr_enabled"]:
                opt = getattr(self.options, key, None)
                setattr(self.options, key, opt.from_any(slot_data[key]))
            self.characters = list(slot_data["characters"])
        else:
            chars = ["Ruby", "Weiss", "Blake", "Yang"]
            if self.options.jnpr_enabled:
                chars += ["Jaune", "Nora", "Pyrrha", "Ren"]
            chars = [char for char in chars if not char in self.options.characters_disabled]

            max_chars = min(self.options.max_characters.value, len(chars))

            if max_chars < self.options.starting_characters:
                raise OptionError("Cannot have more starting characters than characters allowed in generation")

            self.random.shuffle(chars)
            self.characters = chars[:self.options.max_characters]

    def custom_ut_sort(self, region_label: str, location_label: str) -> str | int:
        # fix locations that are not quite in the right place in id order
        if location_label == "Technical Difficulties - Before Arena 2 Crate 5":
            return locations.LOCATION_NAME_TO_ID["Technical Difficulties - Before Arena 2 Crate 4"]
        if location_label == "Technical Difficulties - Before Arena 4 Crate 6":
            return locations.LOCATION_NAME_TO_ID["Technical Difficulties - Before Arena 4 Crate 5"]
        if location_label == "Technical Difficulties - Before Arena 4 Crate 7":
            return locations.LOCATION_NAME_TO_ID["Technical Difficulties - Before Arena 4 Crate 5"]
        # everything else just uses id order
        return locations.LOCATION_NAME_TO_ID[location_label]

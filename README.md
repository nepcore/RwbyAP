# RWBY Grimm Eclipse AP

This is a mod and apworld to integrate [RWBY: Grimm Eclipse](https://store.steampowered.com/app/418340/RWBY_Grimm_Eclipse/) with the [Archipelago](https://archipelago.gg/) multi-game randomizer.

⚠️ It is currently an early WIP, while it should generally work not everything is complete and a lot of polish is most definitely to be done ⚠️

Setup instructions for the mod can be found [here](apworld/docs/setup_en.md). Information on using Archipelago can be found [on the Archipelago website](https://archipelago.gg/tutorial/Archipelago/setup_en)

### Building from source

Due to the game being built on .NET 3.5 the [Archipelago.MultiClient.Net](https://github.com/ArchipelagoMW/Archipelago.MultiClient.Net) library uses websocket-sharp under the hood, however the game includes a stripped copy missing some of the symbols the AP lib needs. Additionally the mainline websocket-sharp version shipped by the AP lib seems to struggle with modern TLS certificates. For the second problem websocket-sharp can be replaced with a [drop-in replacement by Black Sliver](https://github.com/black-sliver/c-wspp-websocket-sharp). To prevent using the games bundled and stripped copy of websocket-sharp the AP lib can be merged with a full copy using e.g. [ILRepack](https://github.com/gluck/il-repack).

Apart from that the mod can be built with a simple `dotnet build`, assuming a .NET SDK is installed on your computer.

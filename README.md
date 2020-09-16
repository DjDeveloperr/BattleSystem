# Pokémon Battle System
## What's this?
This project aims to be the best solution for Pokémon Battles in fan-made games. I can't seem to complete this project alone, because I'm not so good with Pokémon knowledge, especially with the mechanics, so I'm making this project open-source, so everyone's contributions are welcome!
This project is written in C#, and can be easily integrated into Unity projects, as far as I know.

This is basically an event based battle system, you contruct the Battle, listen to events, and let the Battle begin!
There is also a "Text" event which gives out Battle Text.

## Notes
* This project is not a complete working state
* If you want to contribute, use the Pull Requests feature

## TODO List
* [x] Make a good base to work on
* [x] Complete PokéDex module
* [ ] Make the basic moves work
* [ ] Script required Moves
* [ ] Script required Abilities
* [ ] Complete rest of the mechanics

## Example Usage
### Running the source
Make sure to have .NET framework installed
cd to the project dir and
```sh
dotnet run
```
And make choice like "m 1", "m 2" (only 2 moves for each Pokémon in the example)
Invalid usage will break it for now, lol.

### In-code usage
Add this line on top and use the module (No docs are available at the moment)
```cs
using BattleSystem;
```

### BattleSystem.PokeDex
However, this project comes with a PokeDex module which uses JSON files as a source of data.
You can put this on top:
```cs
using BattleSystem.PokeDex;
```
and use it like this
```cs
// names are not case sensitive
var pikachu = Data.GetPokemon("Pikachu");
var thunderbolt = Data.GetMove("Thunderbolt");
var oranberry = Data.GetItem("Oran Berry");
var contrary = Data.GetAbility("Contrary");
var timid = Data.GetNature("Timid");
```
Note that these JSON files were stringified from Showdown!'s data files.

### Contacts
You can reach out to me on Discord, DjDeveloper#7777.

## License
This project is free to use and modify. Yet you cannot distribute it in any other way unless permission granted by the Developer. Check LICENSE file for legal jumbo mumbo.

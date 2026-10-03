# MateriaLib
This mod is a library for other mods to make use of in order to make creating custom materials a simpler process.
This mod must be the client and the server in order to work.
If you encounter any bugs or problems of any kind, do not hesitate to let me know :>

## Important note for developers
As MateriaLib is updated, my goal is to ensure that older mods do not stop working.  
If an update will require action from mod developers, the first number in the version number will change (example : v1.2.3 -> v2.0.0)

## Durability multiplier
In the base game a material's durability multiplier does nothing: a part gets the durability its prefab sets, whatever it is made of.
MateriaLib can make the multiplier work for your own materials. It is off by default, so mods that already set a DurabilityMultiplier keep working as before.

To turn it on for a material, set `useDurabilityMultiplier` to true:
```cs
LibMaterial myMetal = new LibMaterial("My Metal", myHash);
LibMaterial.NewMaterials.Add(myMetal);
myMetal.Configure(new MaterialConfig() { DurabilityMultiplier = 2f });
myMetal.useDurabilityMultiplier = true;
```
A part made from My Metal then has twice the durability of the same part made from a vanilla metal. A crafted tool or weapon adds up the durability of its parts.

- A material made from a template keeps the template's multiplier unless you set DurabilityMultiplier yourself.
- Existing wear is kept when the multiplier is applied, and the bonus is kept after a server restart.
- Vanilla materials and materials with the option off work exactly as in the base game.
- Durability is worked out on the server, so the server must run a MateriaLib version that has this option.

## Settings that do nothing
In the current game build (main-1.7.2.1) the game never reads `MeltingPoint` or `InternalThermalConductivity`, so setting them in MaterialConfig has no effect.

## Potential future updates
- easier way to access and edit unity materials (currently you'll need to use a publicized att dll or reflection to access them)


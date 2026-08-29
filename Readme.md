# Silksong AP Extra Tools

## What's this?

This is a mod that expands **Silksong's Archipelago Randomizer** (aka **Batsong**) and adds **17 new tools** intended to enhance or have an interesting relationship with the Archipelago experience.

These new tools replace Silksong's filler items in the multiworld.

---

## Manual Installation

The mod must be installed in the `BepInEx/plugins` folder.

1. Extract the mod ZIP files from the release page.
2. Copy the contents so that you have the following folders:

```text
BepInEx/plugins/SilksongAPExtra
BepInEx/plugins/silksong_modding-I18N
```

The modified **AP World** also needs to be installed for generations to include the new tools. Enable the `extra_tools` option in your YAML.

---

## Why isn't it available on X mod loader?

It takes time and effort to maintain a mod-loader-specific version, and it's too early in development to consider it.

---

## Mod Dependencie & Compatibility

* This mod depends on **Silksong.I18N** for localization.
* This mod expands **Silksong Archipelago Randomizer v0.4.4** and depends on it.
* This mod uses a different AP World than the aforementioned mod.
* No other mods have been tested.

---

## AP World

This mod requires a modified version of the **AP World** to add the new items.

The items in this mod replace filler items from whichever configuration you have selected in your YAML. If there are not enough filler items to replace, generation will fail.

You'll need to regenerate your template once the modified AP World is installed.

### Can you play with slots that don't use extra tools?

Yes. The modified AP World exposes an `extra_tools` option that allows you to choose whether they are enabled or disabled for each individual slot.

---

## Known Issues

* If you received an AP Extra tool while offline, you'll receive an **"Item not found"** message when connecting. You can click **Reconnect** and continue playing as normal.

---

## I've played for a while and I haven't seen any new tools?

* Tools are placed randomly and replace Silksong's filler items in the multiworld, so you might not receive them early on.
* You might be using an AP World that doesn't have these tools available. Make sure your AP World generates a YAML that exposes the `extra_tools` option.

---

## Why is X not in logic if I have Y?

* Logic involving the tools included in this mod has **not** been taken into account in the AP World.
* Any bugs from the original AP World are also present in the modified version.

---

## Spoiler: What does each new tool do?

### 🔴 Red Tools

**Anchor Spool**
Creates a Silk anchor at your current position. Using the tool again teleports you back to it.

**Moorblade**
Shoots a boomerang-like projectile that damages enemies and can be pogoed.

---

### 🔵 Blue Tools

**Beast Hook**
While you are at 1 or 2 Masks, your needle attacks lifesteal for 1 Mask at the cost of 4 Silk. Has a 15-second cooldown.

**Drifter Wings**
Using the Drifter's Cloak produces a small uplift current that allows you to reach new heights.

**Faydown Clasp**
After using Clawline while airborne for the first time since landing, your Faydown Cloak can be used one additional time.

**Laststitch Band**
While you are at 1 or 2 Masks, binding is 70% faster.

**Lifeline Spool**
Any damage above 1 Mask is reduced to 1 Mask at the cost of 3 Silk.

**Silkseed**
Increases maximum Silk regeneration by 2.

**Snail Badge**
Protects you from enemy damage once every 60 seconds.

---

### 🟡 Yellow Tools

**Architect Seal**
Tool damage is increased by 50%, while nail damage is decreased by 50%.

**Courier Charm**
Deliveries are healed once every 60 seconds.

**Haggler Mask**
Shop prices are 20% cheaper.

**Shakra Beads**
Being inside a room for 12 seconds maps it as if you had the Quill and had sat on a bench.

**Sherma Chime**
Sometimes Silkflies appear to guide you toward the closest location currently in logic. They can be called using Needolin.

**Woven Tools**
For each full Silk Spool, your tools deal 15% more damage.

---

### 🧵 Silk Skills

**Silkstep**
Performs a small mid-air jump that also recharges Swift Step and Faydown Cloak.

**Stillthread**
Slows down time by 20% for 12 seconds.

---

## I'm a mod developer and want to make my own tools for Archipelago?

You can bring it up on the **Archipelago Discord server** in:

`#hollow-knight-silksong-batsong`

We can discuss the details there.

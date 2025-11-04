# Core Design Goal
- Fight, Loot and Progress Character
- Questing and Events
-  Gather and Trade resources
	 in an open world area with economy, challenges and monsters.

___
# Included Mechanics

1. Hybrid Combat
	- Soft lock attack targeting
	- Normal attack
	- 1 point and click skill + 1 targeted skill with cooldowns
	- Stamina depletion while sprinting in combat
	- Basic enemy AI with health and damage stats
	
	-Purpose: Tests the feel of combat and pacing
	
---

2. Traversal
	- Walking and sprinting
	- Prevent Stamina from depleting
	- Small Test Map
	
	-Purpose: Test map/ trail readability and player movement
	
---

3. Resource Gathering
	 - Basic Interaction with resources
		 - Tree - gives wood
		 - Ore - gives gold
		 - Fish - gives fish or pearl
	- Simple random drop rates
	
	-Purpose: Introduces interaction within the world
	
---

4. Looting & Inventory
	- Enemies drop Pilak, Loot Bags and resource or crafting material.
	- Simple Loot Bag Color (common, uncommon, rare)
	- Inventory with limited slots (e.g 20 items max)
	
	-Purpose: Reward feedback loop
	 
---

5.  Questing
	- 2-3 Simple quests
		- Kill X amount of enemies/ Kill X enemies
		- Gather X type of resource
		- Deliver Item to NPC
	- Rewards: Pilak + XP
	
	-Purpose: Gives player direction and progression structure
	
---

6. Progression System
	- Levelling: Gain EXP from combat or quest into stat increase
	- Gear Upgrade: Gear Enhance (+1 to +5 gear)
	- Skill Unlocks : 1 skill unlock on level threshold
	- Skill Upgrade : Tome to improve the star of skills (1 to 5 stars)

	-Purpose: Establishes the player power curve and character progressing
	
---

7. Trading
	-  NPC Merchants where players can:
		- Sell items for pilak
		- Buy potions, materials or gear.
		
	-Purpose: Tests ingame economy loop
	
---

8. Death Penalty
	- Respawn with XP loss (e.g. -10%, -5%)
	- Respawn at the nearest town after some time (e.g. 10 secs, 5 secs)
	
	-Purpose: Introduction to punishment and failure
	
	---
# Prototype Scope Suggestion
|  Aspect   |                           Scope                           |
| :-------: | :-------------------------------------------------------: |
|    Map    |          1 small town + 2 zones (forest & mine)           |
|   NPC's   |       3-4 (Quest NPC, Merchant, Blacksmith, Shaman)       |
|  Enemies  |               3 (melee, ranged, mini boss)                |
|   Items   | 9 (3 different gears, 3 materials, 2 consumable, 1 misc.) |
| Level Cap |                            10                             |
| Duration  |                 10-30 minutes of gameplay                 |

# Testing Goals

|       Focus       |                      Measure                       |
| :---------------: | :------------------------------------------------: |
|      Combat       |   Does it feel fluid? Is the combat responsive?    |
|    Reward Loop    |    Are players inspired or motivated to grind?     |
| Progression Curve |   Does levelling feel satisfying and not grindy?   |
|  Economy Balance  | Does buying and selling feel fair? Are loots fair? |
|    Exploration    |        Is the world intuitive to navigate?         |

# Gameplay Loop
![gameplay-loop](https://github.com/user-attachments/assets/7b738674-b25b-4e75-a453-c79601d56eee)


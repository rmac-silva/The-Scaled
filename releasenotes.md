**Mechanics**
- Muddied: ~~Adds 2 Mud to your Discard pile~~ Add 1 Mud to your Draw pile. Add 1 Mud to your Discard pile.
- Muddied: Muddied cards now gain Retain.

**Card Changes**:
- Updated Chomp, Deal 9(12) damage. Setup: Apply 2 Vulnerable and 2 Weak.
- Updated Lurk, Gain 11(16) Block. Ambush 2(3).
- Reworked Calculated Strike ~~0~~1E, Retain. Deal ~~6(9)~~7(10) damage. Ambush ~~5~~1(2).
- Reworked Concussion Rare 3(2)E Skill: Setup: Stun the enemy. Exhaust.
- Reworked Hunter's Mark: 1(0) Cost Uncommon Skill: Setup: The next attack against this enemy deals double damage.
- Reworked Inertia, 0 Cost Uncommon Skill: Setup: deal 6(9) damage, at the end of your turn increase this damage by 6(9). Exhaust.
- Reworked Respite, Setup: Gain 2(3) Energy.
- Reworked Suffocate, 1 Cost Rare Power: (Innate) At the start of your turn apply 1 ambush to all enemies.
- Reworked Left Hook, 0 Cost Uncommon Attack: Deal 4(6) Damage. Setup: Add two copies of Left Hook into your hand. Exhaust.
- Reworked Snap Jaw, 1 Cost Uncommon Skill: (Retain) Trigger Ambush.
- Reworked Bide, Next turn draw 2(3) cards. Ambush 1(2).
- Reworked Preemptive Strike, Deal 15(20) damage. If the enemy intends to attack, Ambush 3.
- Reworked Gut Strike, Deal 6(8) damage for each Setup on the enemy.
- Reworked Stalk, 1 Cost **Uncommon** Skill: Gain 4(6) Dexterity this turn. Apply 2 ambush to ALL enemies.
- Reworked Dwelling, ~~2(1)~~ 2 Cost: Whenever you apply a Setup gain 2(3) Dexterity this turn.
- Reworked Belly Flop, 1 Cost Common Attack: Deal 10(15) damage to ALL enemies. Setup: Play a random card from your hand.
- Added Nourishment, 1 Cost Rare Skill: Setup: Enemy loses 2(3) strength this turn. Gain 2(3) Strength this turn. 
- Added Callback, 1(0) Cost Common Skill: Ambush 2. Play a random attack from your discard pile, then exhaust it.
- The Hat is now 3
**Programming**
- Remade Setup tooltips into their own separate tooltips under cards.JSON localization. Each Setup card now needs a `.setup` entry. This allows us to have smart tooltips within Setup tooltips (that use variables).

**Bugfixes**
- Fixed a bug where cards added to the draw pile would not increase the counter
- Fixed Abyssal Pull having Innate
**Mechanics**
- Muddied: ~~Adds 2 Mud to your Discard pile~~ Add 1 Mud to your Draw pile. Add 1 Mud to your Discard pile.
- Muddied: Muddied cards now gain Retain.

**Card Changes**:
- Reworked Calculated Strike ~~0~~1E, Retain. Deal ~~6(9)~~7(10) damage. Ambush ~~5~~1(2).
- Reworked Concussion Rare 3(2)E Skill: Setup: Stun the enemy. Exhaust.
- Reworked Hunter's Mark: 1(0) Cost Uncommon Skill: Setup: The next attack against this enemy deals double damage.
- Reworked Inertia, 1 Cost Uncommon Skill: Setup: deal 6(9) damage, at the end of your turn increase this damage by 6(9). Exhaust
- Reworked Respite, Setup: Gain 2(3) Energy
- Reworked Suffocate, 3 Cost Rare Power: (Innate) At the start of your turn apply 1 ambush to all enemies
Reworked
**Programming**
- Remade Setup tooltips into their own separate tooltips under cards.json localization. Each setup card now needs a .setup entry. This allows us to have smart tooltips within Setup tooltips (that use variables).

**Bugfixes**
- Fixed a bug where cards added to the draw pile would not increase the counter
- Fixed Abyssal Pull having Innate
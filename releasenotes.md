**Added Cards**:
- Advantage

**Changed Cards**:
- Concussion now costs 1

**Bugfixes**
- Fixed Pursuit not working properly due to incorrect variables.
- Maelstrom now no longer picks cards that have CanBeGeneratedInCombat set to False. This so far only includes Left-Hook.
- Fixed Pursuit exhausting when it shouldn't.
- Fixed Bask not playing.
- Fixed Mud Bath having an incorrect tooltip
- Fixed Ancient Form's dumb tooltip.
- Fixed Seethe's tooltip.
- Fixed a bug where preemptive strike did not work properly, as it was unfinished.
- Renamed Charge Through -> Preemptive Strike

**Known Issues**
- Advantage doesn't apply a setup twice, it plays the card twice. This is unintended, and will be fixed afterwards.
- Ancient Form's smart tooltip doesn't show the correct amount. Add 2 of the power at the time, and use Amount in the tooltip.
- Preemptive Strike has no Setup tooltip because it has Setup ALL enemies: instead of the common Setup: prefix.
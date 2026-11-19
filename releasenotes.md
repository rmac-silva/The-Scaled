**Leviathan**
- Detached all Ancient code from the character mod

**Programming**
- Restructured how Setups work (WIP). 
    - Each setup is a now a power, that only appears for the player who applied it
    - Setup cards simply add their respective Setup Power
    - Setup Powers have a special method that can be called, to trigger their effects.
    - The Ambush Power will not instead just have to iterate the list of powers, and call that method over all corresponding powers.
    - This way we can have setups that stack (Vulnerable, Weakened) and things that operate separately (Inertia, that one setup I never ended up implementing)
    - This makes setups more modular, each with their own icons and tooltips.
    - ISetupPower Abstract class




**Card Changes**:
- **Mud**, gained Ethereal

**Art**:
- Preemptive Strike
- Lacerate
- From the depths
- Death from Below
- Hunter's Mark power icon
- Suffocate power icon
- Lead Boots power icon
- Smothering Presence power icon
- Character Select Transition




**Bugfixes**
- Fixed Callback not behaving properly
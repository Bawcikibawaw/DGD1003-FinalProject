

GAME DESIGN DOCUMENT (GDD)
Project Name: Arena of Bones (Placeholder Title) Genre: 2D Top-Down Shooter / Arena Survivor Platform: PC (Windows/Mac) Engine: Unity 6 Target Audience: Hardcore gamers, Roguelite fans, Retro arcade lovers.

1. GAME OVERVIEW
1.1. High Concept
The player is a lone gladiator trapped in an ancient coliseum, forced to fight against endless waves of undead skeletons. Armed with a gun and agility, the goal is simple: Survive as long as possible.

1.2. Unique Selling Points (USP)
Fast-Paced Combat: Fluid movement and aiming mechanics inspired by classic arcade shooters.

Wave System: Progressively difficult waves of enemies, culminating in boss fights.

Retro Aesthetic: High-quality pixel art visuals with a gladiator arena theme.

Time Rewind Mechanic: (Optional/Planned) Ability to rewind time briefly to escape death.

2. GAMEPLAY MECHANICS
2.1. Player Controls
Movement: WASD Keys (Top-down movement).

Aiming: Mouse Cursor (Player rotates to face the mouse).

Shooting: Left Mouse Button (Fires projectile).

Pause: ESC Key.

Dash: Space Bar (Planned feature).

2.2. Combat System
Shooting: The player fires projectiles towards the cursor position.

Health: The player has a health bar (UI Slider). Taking damage from enemies reduces health.

Death: When health reaches 0, the "Game Over" screen appears with the wave reached score.

Camera: Smoothly follows the player but is clamped to the arena boundaries.

2.3. Enemy AI
Tracking: Enemies spawn from portals and constantly move towards the player's position.

Collision: Enemies deal damage upon contact with the player.

Rotation: Enemies utilize sprite flipping to face the direction of movement (no rotation, just flip X).

3. CHARACTERS & ENEMIES
3.1. The Gladiator (Player)
A pixel-art warrior character equipped with a ranged weapon.

Features visuals for Idle, Run, and Shoot states.

3.2. Enemy Types
Skeleton Grunt: Basic enemy. Average speed, average health. Melee attacker.

Skeleton Rusher (Planned): Low health, high speed. Swarms the player.

Skeleton Tank (Planned/Boss): High health, slow speed. Hard to kill.

4. LEVEL DESIGN
4.1. The Arena
Visuals: A circular stone coliseum with spectators in the background (Pixel Art).

Boundaries: Enclosed by walls (Edge Collider 2D) to keep the player inside.

Obstacles: Stone pillars in the center to provide cover and block enemy pathfinding.

Spawn Points: Blue glowing portals located at the sides of the arena where enemies spawn.

5. USER INTERFACE (UI)
5.1. HUD (Heads-Up Display)
Health Bar: Visual slider indicating current HP.

Wave Counter: Text displaying the current wave number (e.g., "WAVE 3").

Countdown: Timer showing when the next wave starts.

5.2. Menus
Pause Menu: Activated by ESC. Options: Resume, Restart, Quit. Freezes game time.

Game Over Screen: Triggers on death. Displays "YOU DIED" and the highest wave reached. Options: Try Again, Main Menu.

6. AUDIO
6.1. Sound Effects (SFX)
Gunshot: Sharp, satisfying sound when firing.

Enemy Hit: Impact sound when a bullet hits a skeleton.

Player Damage: Grunt or impact sound when the player gets hurt.

Game Over: A distinct jingle signaling defeat.

6.2. Music
Battle Theme: High-energy, looped track that plays during gameplay to maintain tension.

7. TECHNICAL SPECIFICATIONS
Camera System: CameraFollow.cs with Mathf.Clamp to prevent seeing outside the map.

Input System: Unity New Input System (Keyboard & Mouse).

Data Management: Singleton pattern used for AudioManager, GameManager, and PlayerMovement.

Physics: Rigidbody2D used for movement; Colliders used for hit detection.

8. FUTURE ROADMAP (To-Do)
[ ] Add "Dash" mechanic for dodging.

[ ] Implement Main Menu scene.

[ ] Add High Score saving system.

[ ] Introduce particle effects (Blood, Muzzle Flash).

[ ] Create different weapon types (Shotgun, Machine Gun).

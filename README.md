# Unity interview test project.
Last tested version: Unity 2019.x

Just open the project in Unity and run Tactics scene. In SO/ folder there are the configurable settings for the game and Units (Scriptable Objects).

## License
### Code files
MIT License

Copyright (c) [2020] [Felipe "pepipe" Cabedo]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

### Plugins folder
All things from this folder are from Unity Asset Store and some are from paid assets. You'll need to check Unity Asset Store for the license.

## Overview
This test was done in 4 days and I had complete freedom for using Components, Asset Store, etc.

## Description
#### Build a smallTactics style game:
- Turn-based

- Units move around a map and attack each other

- Winning condition: all AI Units have been killed

- [Wiki about tactical role](https://en.wikipedia.org/wiki/Tactical_role)

#### Unit-based
- 2 to 4 Units controlled by the Player

- 4 to 8 Units controlled by AI

#### AI: use anything you want. Example Actions:
- Find Nearest Unit

- Move To Specific Location

- Find Weakest Unit

- Attack If I Am Stronger

- Run Away If I Am Losing

#### Each turn, each Unit does 1 or 2 Actions
- Move (always)

- Attack (if AI or Player wants to)

- Have a Text Window that shows the Actions taken

- When done, hand control to the next

#### Have some form of visual Map
- Anything you want: grid-based, coordinates, navmesh, etc

- 2D, 3D, etc

## Decisions
This project took 4 days of work and to accomplish the goal in this timeframe I opted for a top-down view with a 2D grid. Using a 2D grid is simpler to create the gameplay and play the tests. 

Of course in this timeframe, all pieces are very rudimentary but my goal was to write the components in a way that in a future can be built upon more complex gameplay.
Trying always, when it makes sense, to create components that not derive from MonoBehaviour. This way, if we want to Unit test the components, it will be easier.

The project break downs in this "components" (lacking a better word for it): Commands, Grid, Path, Units (with States), TurnSystem and configurable files (Scriptable Objects). 

#### Commands
By needing a text window that shows all the actions taken it immediately comes to mind the [Commands design pattern](https://gameprogrammingpatterns.com/command.html). 

All commands will be derived from the interface ICommand. 
It has only the Execute command, but initially, I had the idea of adding undo and replay functionality, hence why there's a CommandManager that has a list of Commands. 
That in this case doesn't do much (since we only do Execute command).

#### Grid
Grid is a generic 2-dimensional grid. 
GridObject is the MonoBehaviour used to populate the grid (it can have a visual and/or a debug text). 
The Grid will basically create a 2D grid and do the calculations between Vector3 WorldPos and the (int X, int Y) GridPos. 
Is here used in a top-down view but it could be easily adapted to a 2D grid in a 3D environment (without height).

SubGrid - Abstract class that is specific to Unity, implemented by:

- MovementGrid - Grid that shows where the units can move.
- AttackGrid - Grid that shows where the units can attack.

#### Path
Path uses simple A* to traverse the grid. 
Moving horizontally is straight so it has a simple cost of 1, moving diagonally has a cost of the square root of 2 which is 1.4 .
To simplify calculations, horizontal = 10, diagonal = 14.

G - Cost from the start node
H - Cost to the goal node
F - Sum of G + H

I used a simple A* implementation, it could easily be improved, by for example:

- use a binary tree in GetLowestFCostNode to search open list

- in neighbor code, I'm getting them dynamically but I could pre-calculating and store them

- I thought about using DOTs here but I didn't have the time.

#### Units
All Units will have an internal [state machine](https://gameprogrammingpatterns.com/state.html) to control them. 
They also use an interface for input (IUnitInput).
This way we can create multiple inputs for the units.
In this example, it has a PlayerInput and a basic AI (dumb) Input that will chase the less healthy player unit.

#### TurnSystem
Component responsible for controlling the game (well it's a turn-based game) that in this case is basically keeping track of units in the map.

#### Configurable settings
I used Scriptable Objects to easily configure game settings and unit stats. This files can be found in SO folder.

## TO-DO
Things left out because not enough time.

- Undo functionality

- Replay functionlity

- Optimize A* code

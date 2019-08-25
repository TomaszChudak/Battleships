Task:
The challenge is to program a simple version of the game Battleships. Create an application to allow a single human player to play a one-sided game of Battleships against ships placed by the computer.
The program should create a 10x10 grid, and place several ships on the grid at random with the following sizes:
- 1x Battleship (5 squares)
- 2x Destroyers (4 squares)
The player enters coordinates of the form “A5”, where "A" is the column and "5" is the row, to specify a square to target. Shots result in hits, misses or sinks. The game ends when all ships are sunk.


To run - run Battleships.Console project

Cofiguration:
To change some features of the game you can edit appsettings.json
'Grid' section - number of columns and rows of grid (possible values between 10 and 20)
'ShipTypes' section - you can add other types of ships (possible Size values between 1 and 6, possible Count values between 0 and 10)
All possible values are set in SettingRules class
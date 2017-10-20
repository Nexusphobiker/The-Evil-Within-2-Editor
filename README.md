# The Evil Within 2 Editor by Nexusphobiker

This is my wip tew2 editor. Replacing/Removing/Adding files is at the moment not implemented.
Disclaimer: This project utilizes the DotNetZip library

# Current features
- Preview for .png, .lanb (localization files), .decl, partially .bdecl
- Extraction of all .pkr files

# WIP
Better ui (jumping to a specific path  etc.)

Implementing Replacing/Removing/Adding files

The biggest part at the moment i am working at are the bdecl (binary decl) files which depending on the types got different objects into them which makes it not possible to reverse one and get all. Some have a very simple structure with two objects others have three or 4 objects with 3 sub objects etc. The game contains 66 binary decl types. Down below you will find the list of implemented ones.

Implemented BDECL formats (9/66):

- achievement
- attackTable
- casting
- randomMessage
- doorSounds
- enemyBody
- equipment
- goreSetting
- health

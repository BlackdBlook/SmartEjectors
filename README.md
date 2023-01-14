# SmartEjectors
A Dyson Sphere Program mod that makes EM Rail Ejectors smarter.

## Features
- Independently configurable features
- Ejectors select Orbit 1 by default
- Ejectors stop firing when local dyson sphere is filled
- Ejectors stop firing when local dyson swarm exceeds a certain size, proportional to the number of available nodes

## Changelog
### v1.3.3
- Fixed performance issue with ejector logic, see issue [#1](https://github.com/DanielHeEGG/SmartEjectors/issues/1) for more details
### v1.3.2
- Fixed visual bug of ejectors stuck in firing state
- Redo ejector disabling logic with transpilers
### v1.3.1
- Fixed bug where nodes that are not constructed count towards available nodes
### v1.3.0
- Added feature where EM Rail Ejectors stop firing depending on swarm size
- Added features section in README
### v1.2.1
- Readme file changes not committed in v1.2.0
### v1.2.0
- Added feature where placed EM Rail Ejectors select Orbit 1 by default (can be disabled in config, follows host settings in multiplayer)
### v1.1.3
- Added zhCN language support
### v1.1.2
- Disabled EM Rail Ejectors now show "Disabled - Filled Sphere" instead of "Idle"
### v1.1.1
- Fixed missing required dependency in thunderstore
### v1.1.0
- Added support for Nebula Multiplayer (new dependency: [NebulaMultiplayerModAPI](https://dsp.thunderstore.io/package/nebula/NebulaMultiplayerModApi/))
### v1.0.2 - v1.0.4
- Code formatting, no functional change
### v1.0.1
- Added process filter for game executable
- Fixed bug where disabled ejectors draw full charging power

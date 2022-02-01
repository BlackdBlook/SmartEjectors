# SmartEjectors
A Dyson Sphere Program mod.

In the current state, Dyson Swarms are terribly inefficient. There is no in-game option to have the EM Rail Ejectors fire only at the Dyson Sphere and stopping automatically when it's filled up.

This mod stops EM Rail Ejectors from firing when the local Dyson Sphere has no available cell points.

## Changelog
### v1.1.2
- Disabled EM Rail Ejectors now show "Disabled - Filled Sphere" instead of "Idle"
### v1.1.1
- Fixed missing required dependency in thunderstore
### v1.1.0
- Added support for Nebula Multiplayer (new dependency: [NebulaMultiplayerModAPI](https://dsp.thunderstore.io/package/nebula/NebulaMultiplayerModApi/))
### v1.0.2 - v1.0.4
- Code formatting, no functional change
### v1.0.1
- Added process filter for game excecutable
- Fixed bug where disabled ejectors draw full charging power

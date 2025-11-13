## Introduction

Using the default configuration, all this plugin does is add **Eclipse 8** to the usual difficulty options, and change the initial selection to **Monsoon** instead of **Rainstorm**. These changes are supported in both singleplayer and the **Simulacrum** game mode. It is also fully compatible with multiplayer lobbies - only the host needs to have this installed.

However, the settings can be modified to hide or show any difficulty in the game. The default selection can be customized as well. Since the configuration file is dynamically generated, it even supports difficulties added by other plugins. This is useful if you wish to remove unused options from the menu.

## F.A.Q.

#### *How do you change the settings?*
After installing, launch the game in order to initialize the configuration file. Then, edit the file `BepInEx/config/local.difficulty.selection.cfg`, preferably using the editor built into [*r2modman*](https://thunderstore.io/package/ebkr/r2modman/). A separate entry is created for each difficulty option. In-game, difficulties will be shown if the corresponding value is set to *true*, hidden when *false*.

#### *Where is the option for `[insert difficulty name here]`?*
Please note that identifiers in the configuration file use the internal names for difficulties rather than the ones you may be familiar with. However, each entry also includes the display name in the description.

#### *Why would someone use this over existing alternatives that provide similar functionality?*
Extra care has been taken to ensure this one is flexible and compatible with multiplayer lobbies, additional difficulties, and the **Eclipse** menu.

#### *I have an issue to report. How can I contact the developer?* 
Please provide bug reports or any other feedback on the [issues](https://github.com/6thmoon/DifficultySelection/issues) page. Feel free to check out my other [work](https://thunderstore.io/package/6thmoon/?ordering=top-rated) as well.

## Version History

#### `0.1.3`
- Sanitize identifiers for compatibility with certain difficulties.

#### `0.1.2`
- Reload configuration upon entering the lobby.

#### `0.1.1`
- Update icon.

#### `0.1.0` ***- Initial Release***

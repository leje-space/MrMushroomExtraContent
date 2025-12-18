# Mister Mushroom Extra Content

This mod for **Hollow Knight: Silksong** enhances your encounters with Mister Mushroom, adding three extra interactions during Acts 1 and 2:

- **First Encounter**: Before the Moss Mother boss fight
- **Second Encounter**: After obtaining the Silkspear ability, before the Bell Beast boss fight
- **Third Encounter**: After defeating the Bell Beast, at the entrance to the Hunter's March (before the Skarrgard fight)

***To Be Continued***

## Requirements

- Hollow Knight: Silksong
- [BepInEx 5.x](https://github.com/BepInEx/BepInEx)

## Installation

1. **Install BepInEx** if you haven't already:
   - Download and install [BepInEx 5.x](https://github.com/BepInEx/BepInEx) for your platform
   - Run the game once to generate BepInEx folders

2. **Install Mister Mushroom Extra Content**:
   - Download the latest release containing both required DLL files:
     - `space.leje.MrMushroomExtraContent.dll` (main plugin)
     - `Core_FsmUtil.dll` (dependency)
   - Place both DLL files in the `BepInEx/plugins` folder in your game directory
   - The plugin will load automatically when you start the game

## Building from Source

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (compatible with .NET Standard 2.1)

### Build Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/leje-space/MrMushroomExtraContent.git
   cd MrMushroomExtraContent
   ```

2. Build the project:
   ```bash
   dotnet build MrMushroomExtraContent/MrMushroomExtraContent.csproj -c Release
   ```

3. The compiled DLL files will be in:
   ```
   MrMushroomExtraContent/bin/Release/netstandard2.1/space.leje.MrMushroomExtraContent.dll
   MrMushroomExtraContent/bin/Release/netstandard2.1/Core_FsmUtil.dll
   ```

## Troubleshooting

### How do I know if the mod is working?
Check the log file (`BepInEx/LogOutput.log`) for the following message on game start:
```
[Info   : Mister Mushroom Extra Content] Plugin Mister Mushroom Extra Content v1.x is loaded!
```

### The mod isn't working
1. Ensure both DLL files are in the `BepInEx/plugins` folder
2. Verify BepInEx is correctly installed and running
3. Check the BepInEx log for any errors related to the mod
4. Make sure you're in the correct game area and meet the conditions for each encounter

## Uninstalling
To remove the mod, simply delete both DLL files from your `BepInEx/plugins` folder:
- `space.leje.MrMushroomExtraContent.dll`
- `Core_FsmUtil.dll` (only if not used by other mods)

## Compatibility

- **Game Version**: Tested with Hollow Knight: Silksong v1.0.29315
- **BepInEx**: Requires version 5.x
- **Other Mods**: Should be compatible with most mods, but conflicts may occur with other mods that modify Mister Mushroom or FSM behavior

## Known Issues

- None currently reported

## Changelog

### Version 1.2.0
- Added third encounter with Mister Mushroom

### Version 1.1.0
- Added second encounter with Mister Mushroom

### Version 1.0.0
- Initial release
- Added first encounter with Mister Mushroom

## Disclaimer

This is an **unofficial, fan-made mod** and is not affiliated with, endorsed by, or connected to Team Cherry or any official Hollow Knight: Silksong development.

- **Hollow Knight: Silksong** is owned by Team Cherry
- This mod is provided "as-is" without warranty of any kind
- Use at your own risk - modding may affect game stability or violate terms of service
- The creators of this mod are not responsible for any issues that may arise from its use

## Credits

This mod uses the following libraries:

- **[Core_FsmUtil](https://github.com/hk-modding/HK.Core.FsmUtil)** - Utility library for working with PlayMaker FSMs, created by the Hollow Knight modding community

## Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bugs and feature requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

# Fork Notes
This fork is port of InputGlyphs asset made by [@eviltwo](https://github.com/eviltwo) to Facepunch.Steamworks for simpler Steamworks integration and more consistent with C# naming convention. Based on InputGlyphs 1.2.4.

New Features:
- Code has been refactored into more decoupled event-driven update system.
- SendMessages and BroadcastMessages are now supported for PlayerInput component.
- Made port from [Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET) to my fork of [Facepunch.Steamworks](https://github.com/TheMatiaz0/Facepunch.Steamworks).

Important Note: Facepunch.Steamworks is rarely updated compared to Steamworks.NET. If you want absolute latest version of Steamworks, please use eviltwo's implementation. You can also post issue within my fork and I will try to patch it up!

Along with these changes, there is also a new fork for eviltwo's SteamInputAdapter with Facepunch.Steamworks port: https://github.com/TheMatiaz0/UnitySteamInputAdapter

# Input Glyphs
InputGlyphs is a package designed to display button glyph images (icons) of input devices detected by Unity's InputSystem. It is easy to install and designed to allow for the extension of devices and glyph images.

The glyph images in the game will automatically switch according to the device in use.

The package includes pre-configured glyph images for keyboards & mice and various controllers, but you can add or change your own glyph images or use the glyphs provided by Steamworks.

![Duo player glyphs](https://eviltwo.github.io/InputGlyphs_Docs/assets/duo_glyphs.png)

# Purchase eviltwo implementation from Asset Store
https://assetstore.unity.com/packages/slug/289760

# Install Facepunch.Steamworks implementation with UPM
```
https://github.com/TheMatiaz0/InputGlyphs.git?path=InputGlyphs/Assets/InputGlyphs
```
To retrieve a specific version, append `#1.0.0` at the end.

# Documentation
https://eviltwo.github.io/InputGlyphs_Docs/

For information on the licenses of the assets used by this package, please refer to [Third-Party Notices.txt](InputGlyphs/Assets/InputGlyphs/Third-Party%20Notices.txt).

# Support Work of eviltwo
As a solo developer, your financial support would be greatly appreciated and helps me continue working on this project.
- [Asset Store](https://assetstore.unity.com/publishers/12117)
- [Steam](https://store.steampowered.com/curator/45066588)
- [GitHub Sponsors](https://github.com/sponsors/eviltwo)

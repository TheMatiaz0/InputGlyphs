#if INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using System.Collections.Generic;
using System.Linq;
using InputGlyphs.Loaders.Utils;
using InputGlyphs.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

namespace InputGlyphs.Loaders
{
    public class GamepadGlyphLoader : IInputGlyphLoader
    {
        private readonly InputGlyphTextureMap _fallbackTextureMap;
        private readonly InputGlyphTextureMap _xboxControllerTextureMap;
        private readonly InputGlyphTextureMap _playstationControllerTextureMap;
        private readonly InputGlyphTextureMap _switchProControllerTextureMap;

        public GamepadGlyphLoader(
            InputGlyphTextureMap fallbackTextureMap,
            InputGlyphTextureMap xboxControllerTextureMap,
            InputGlyphTextureMap playstationControllerTextureMap,
            InputGlyphTextureMap switchProControllerTextureMap)
        {
            _fallbackTextureMap = fallbackTextureMap;
            _xboxControllerTextureMap = xboxControllerTextureMap;
            _playstationControllerTextureMap = playstationControllerTextureMap;
            _switchProControllerTextureMap = switchProControllerTextureMap;
        }

        public bool LoadGlyph(Texture2D texture, IReadOnlyList<InputDevice> activeDevices, string inputLayoutPath)
        {
            var supportedDevice = TryGetSupportedDevice(activeDevices);
            if (supportedDevice == null)
            {
                return false;
            }
            
            var activeTextureMap = GetTextureMap(supportedDevice);

            var localPath = InputLayoutPathUtility.RemoveRoot(inputLayoutPath);
            if (activeTextureMap.TryGetTexture(localPath, out var result))
            {
                texture.Reinitialize(result.width, result.height, TextureFormat.ARGB32, false);
                texture.SetPixels(result.GetPixels());  // Glyph texture must be readable
                texture.Apply();
                return true;
            }

            return false;
        }

        private static Gamepad TryGetSupportedDevice(IReadOnlyList<InputDevice> activeDevices)
        {
            return activeDevices.OfType<Gamepad>().FirstOrDefault();
        }

        private InputGlyphTextureMap GetTextureMap(Gamepad gamepad)
        {
            switch (gamepad)
            {
                case XInputController:
                    return _xboxControllerTextureMap;

                case DualShockGamepad:
                    return _playstationControllerTextureMap;

                case SwitchProControllerHID:
                    return _switchProControllerTextureMap;

                default:
                    return _fallbackTextureMap;
            }
        }
    }
}
#endif

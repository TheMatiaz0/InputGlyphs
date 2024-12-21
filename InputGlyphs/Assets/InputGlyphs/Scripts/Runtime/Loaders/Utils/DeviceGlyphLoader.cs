#if INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using System.Collections.Generic;
using System.Linq;
using InputGlyphs.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs.Loaders.Utils
{
    /// <summary>
    /// General implementation of the <see cref="IInputGlyphLoader"/>.
    /// It is recommended to use the <see cref="DeviceGlyphLoaderInitializer{T}"/> to generate the loader instead of inheriting and implementing this class.
    /// </summary>
    public class DeviceGlyphLoader<T> : IInputGlyphLoader
        where T : InputDevice
    {
        public readonly List<InputGlyphTextureMap> TextureMaps = new List<InputGlyphTextureMap>();

        public bool LoadGlyph(Texture2D texture, IReadOnlyList<InputDevice> activeDevices, string inputLayoutPath)
        {
            if (!IsAnyDeviceSupported(activeDevices))
            {
                return false;
            }
            
            var localPath = InputLayoutPathUtility.RemoveRoot(inputLayoutPath);
            for (var i = 0; i < TextureMaps.Count; i++)
            {
                if (TextureMaps[i].TryGetTexture(localPath, out var result))
                {
                    texture.Reinitialize(result.width, result.height, TextureFormat.ARGB32, false);
                    texture.SetPixels(result.GetPixels()); // Glyph texture must be readable
                    texture.Apply();
                    return true;
                }
            }

            return false;
        }

        private static bool IsAnyDeviceSupported(IReadOnlyList<InputDevice> activeDevices)
        {
            return activeDevices.OfType<T>().Any();
        }
    }
}
#endif

using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace InputGlyphs.Display
{
    public static class InputGlyphDisplayBridge
    {
        private static List<IGlyphDisplay> GlyphDisplays { get; set; } = new List<IGlyphDisplay>();

        public static void Register(IGlyphDisplay display)
        {
            GlyphDisplays.Add(display);
        }

        public static void Unregister(IGlyphDisplay display)
        {
            GlyphDisplays.Remove(display);
        }
        
        public static void UpdateGlyphs(PlayerInput playerInput)
        {
            if (playerInput == null || !playerInput.isActiveAndEnabled || playerInput.devices.Count == 0)
            {
                return;
            }
            
            foreach (var glyphDisplay in GlyphDisplays)
            {
                if (glyphDisplay.IsVisible)
                {
                    glyphDisplay.UpdateGlyphs(playerInput.devices, playerInput.currentControlScheme);
                }
            }
        }
    }
}
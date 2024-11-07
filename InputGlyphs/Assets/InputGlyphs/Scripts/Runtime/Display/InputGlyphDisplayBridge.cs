using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace InputGlyphs.Display
{
    public static class InputGlyphDisplayBridge
    {
        public static event Action<IGlyphDisplay> OnRegisteredDisplay;
        
        private static List<IGlyphDisplay> GlyphDisplays { get; } = new List<IGlyphDisplay>();

        public static void Register(IGlyphDisplay display)
        {
            if (!GlyphDisplays.Contains(display))
            {
                GlyphDisplays.Add(display);
                OnRegisteredDisplay?.Invoke(display);
            }
        }

        public static void Unregister(IGlyphDisplay display)
        {
            if (GlyphDisplays.Contains(display))
            {
                GlyphDisplays.Remove(display);
            }
        }
        
        public static void UpdateGlyphs(PlayerInput playerInput)
        {
            if (playerInput == null || !playerInput.isActiveAndEnabled || playerInput.devices.Count == 0)
            {
                return;
            }
            
            foreach (var glyphDisplay in GlyphDisplays)
            {
                glyphDisplay.UpdateGlyphs(playerInput.devices, playerInput.currentControlScheme);
            }
        }
    }
}
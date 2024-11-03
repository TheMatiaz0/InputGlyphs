using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs.Display
{
    public static class InputGlyphDisplayBridge
    {
        private static List<IGlyphDisplay> GlyphDisplays { get; } = new List<IGlyphDisplay>();

        public static void Register(IGlyphDisplay display)
        {
            if (!GlyphDisplays.Contains(display))
            {
                GlyphDisplays.Add(display);
            }
            else
            {
                Debug.LogWarning("Display is already registered!");
            }
        }

        public static void Unregister(IGlyphDisplay display)
        {
            if (!GlyphDisplays.Remove(display))
            {
                Debug.LogWarning("Can't unregister display that wasn't registered!");
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
                if (glyphDisplay.IsVisible)
                {
                    glyphDisplay.UpdateGlyphs(playerInput.devices, playerInput.currentControlScheme);
                }
            }
        }
    }
}
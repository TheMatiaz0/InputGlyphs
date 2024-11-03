using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputGlyphs.Display
{
    public interface IGlyphDisplay
    {
        bool IsVisible { get; }
        void UpdateGlyphs(ReadOnlyArray<InputDevice> devices, string controlScheme);
    }
}
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputGlyphs.Display
{
    public interface IGlyphDisplay
    {
        void UpdateGlyphs(ReadOnlyArray<InputDevice> devices, string controlScheme);
    }
}
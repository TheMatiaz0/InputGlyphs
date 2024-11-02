using UnityEngine.InputSystem;

namespace InputGlyphs.Display
{
    public interface IGlyphDisplay
    {
        bool IsVisible { get; }
        void UpdateGlyphs(PlayerInput playerInput);
    }
}
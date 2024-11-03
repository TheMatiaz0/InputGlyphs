using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs.Display
{
    public class InputGlyphDisplayManager : MonoBehaviour
    {
        [SerializeField] private InputManager InputManager;

        private void Awake()
        {
            if (InputManager == null)
            {
                return;
            }
            
            InputManager.OnControlsChangedEvent += OnControlsChanged;
        }

        private void OnDestroy()
        {
            if (InputManager == null)
            {
                return;
            }
            
            InputManager.OnControlsChangedEvent -= OnControlsChanged;
        }

        private void OnControlsChanged(PlayerInput playerInput)
        {
            InputGlyphDisplayBridge.UpdateGlyphs(playerInput);
        }
    }
}
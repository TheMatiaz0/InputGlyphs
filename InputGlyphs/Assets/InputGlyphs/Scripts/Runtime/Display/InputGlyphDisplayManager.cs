using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs.Display
{
    public class InputGlyphDisplayManager : MonoBehaviour
    {
        [SerializeField] private InputBroker InputBroker;

        private void Awake()
        {
            if (InputBroker == null)
            {
                return;
            }
            
            InputBroker.OnControlsChangedEvent += UpdateGlyphs;
            InputGlyphDisplayBridge.OnRegisteredDisplay += OnRegisteredDisplay;
        }

        private void OnDestroy()
        {
            if (InputBroker == null)
            {
                return;
            }
            
            InputBroker.OnControlsChangedEvent -= UpdateGlyphs;
            InputGlyphDisplayBridge.OnRegisteredDisplay -= OnRegisteredDisplay;
        }
        
        private void OnRegisteredDisplay(IGlyphDisplay display)
        {
            UpdateGlyphs(InputBroker.PlayerInputReference);
        }

        private void UpdateGlyphs(PlayerInput playerInput)
        {
            InputGlyphDisplayBridge.UpdateGlyphs(playerInput);
        }
    }
}
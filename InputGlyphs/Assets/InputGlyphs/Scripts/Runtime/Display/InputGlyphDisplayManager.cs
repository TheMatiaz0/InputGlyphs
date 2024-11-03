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
            
            InputBroker.OnControlsChangedEvent += OnControlsChanged;
        }

        private void OnDestroy()
        {
            if (InputBroker == null)
            {
                return;
            }
            
            InputBroker.OnControlsChangedEvent -= OnControlsChanged;
        }

        private void OnControlsChanged(PlayerInput playerInput)
        {
            InputGlyphDisplayBridge.UpdateGlyphs(playerInput);
        }
    }
}
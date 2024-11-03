using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs.Utils
{
    [RequireComponent(typeof(PlayerInput))]
    public class ControlsChangedMessageBroker : MonoBehaviour
    {
        public event Action<PlayerInput> OnControlsChangedMessage; 
        
        // Uses PlayerNotifications.SendMessages OR PlayerNotifications.BroadcastMessages
        private void OnControlsChanged(PlayerInput playerInput)
        {
            OnControlsChangedMessage?.Invoke(playerInput);
        }
    }
}
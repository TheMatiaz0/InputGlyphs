using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs
{
    /// <summary>
    /// Transmits messages from <c>PlayerNotification.SendMessages</c> and <c>PlayerNotifications.BroadcastMessages</c>.
    /// Add this script next to <c>PlayerInput</c> or use <c>InputBroker</c> which handles it automatically.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputMessageBroker : MonoBehaviour
    {
        public event Action<PlayerInput> OnControlsChangedMessage;
        public event Action<PlayerInput> OnDeviceLostMessage;
        
        private void OnControlsChanged(PlayerInput playerInput)
        {
            OnControlsChangedMessage?.Invoke(playerInput);
        }

        private void OnDeviceLost(PlayerInput playerInput)
        {
            OnDeviceLostMessage?.Invoke(playerInput);
        }
    }
}
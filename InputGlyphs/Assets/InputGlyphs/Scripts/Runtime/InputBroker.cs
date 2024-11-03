using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs
{
    public class InputBroker : MonoBehaviour
    {
        public event Action<PlayerInput> OnControlsChangedEvent;
        
        [SerializeField]
        private PlayerInput PlayerInput;

        private PlayerInput _lastPlayerInput;
        private ControlsChangedMessageBroker _messageBroker;

#if UNITY_EDITOR
        private void Reset()
        {
            PlayerInput = FindAnyObjectByType<PlayerInput>();
        }
#endif
        
        private void Start()
        {
            TryGetPlayerInput();
        }

        private void TryGetPlayerInput()
        {
            if (PlayerInput == null && InputGlyphDisplaySettings.AutoCollectPlayerInput)
            {
                PlayerInput = PlayerInput.all.FirstOrDefault();
            }
            
            if (PlayerInput == null)
            {
                Debug.LogError("PlayerInput is not set.", this);
            }
            else
            {
                if (PlayerInput.notificationBehavior is PlayerNotifications.SendMessages or PlayerNotifications.BroadcastMessages)
                {
                    if (!PlayerInput.TryGetComponent(out _messageBroker))
                    {
                        _messageBroker = PlayerInput.gameObject.AddComponent<ControlsChangedMessageBroker>();
                    }
                }
            }
        }

        private void Update()
        {
            TryGetPlayerInput();

            if (PlayerInput == _lastPlayerInput) return;
            if (_lastPlayerInput != null)
            {
                UnregisterPlayerInputEvents(_lastPlayerInput);
            }

            else
            {
                RegisterPlayerInputEvents(PlayerInput);
                OnControlsChangedEvent?.Invoke(PlayerInput);
            }
            _lastPlayerInput = PlayerInput;
        }

        private void RegisterPlayerInputEvents(PlayerInput playerInput)
        {
            switch (playerInput.notificationBehavior)
            {
                case PlayerNotifications.InvokeUnityEvents:
                    playerInput.controlsChangedEvent.AddListener(OnControlsChanged);
                    break;
                case PlayerNotifications.InvokeCSharpEvents:
                    playerInput.onControlsChanged += OnControlsChanged;
                    break;
                case PlayerNotifications.SendMessages:
                case PlayerNotifications.BroadcastMessages:
                    _messageBroker.OnControlsChangedMessage += OnControlsChanged;
                    break;
            }
        }

        private void UnregisterPlayerInputEvents(PlayerInput playerInput)
        {
            switch (playerInput.notificationBehavior)
            {
                case PlayerNotifications.InvokeUnityEvents:
                    playerInput.controlsChangedEvent.RemoveListener(OnControlsChanged);
                    break;
                case PlayerNotifications.InvokeCSharpEvents:
                    playerInput.onControlsChanged -= OnControlsChanged;
                    break;
                case PlayerNotifications.SendMessages:
                case PlayerNotifications.BroadcastMessages:
                    _messageBroker.OnControlsChangedMessage -= OnControlsChanged;
                    break;
            }
        }

        private void OnDestroy()
        {
            if (_lastPlayerInput != null)
            {
                UnregisterPlayerInputEvents(_lastPlayerInput);
                _lastPlayerInput = null;
            }
        }

        private void OnControlsChanged(PlayerInput playerInput)
        {
            if (playerInput == PlayerInput)
            {
                OnControlsChangedEvent?.Invoke(playerInput);
            }
        }
    }
}

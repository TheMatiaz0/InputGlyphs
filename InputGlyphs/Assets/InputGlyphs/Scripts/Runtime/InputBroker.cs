using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs
{
    /// <summary>
    /// Handles and transmits Unity's PlayerInput events allowing to use it for any purpose.
    /// <example>Check <c>InputGlyphDisplayManager</c> for example implementation.</example>
    /// </summary>
    public class InputBroker : MonoBehaviour
    {
        public event Action<PlayerInput> OnControlsChangedEvent;
        public event Action<PlayerInput> OnDeviceLostEvent;
        
        [Header("Optional: InputBroker will try to find PlayerInput")]
        [SerializeField]
        private PlayerInput PlayerInput;

        private PlayerInput _lastPlayerInput;
        private PlayerInputMessageBroker _messageBroker;

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
                        _messageBroker = PlayerInput.gameObject.AddComponent<PlayerInputMessageBroker>();
                    }
                }
            }
        }

        private void Update()
        {
            Scan();
        }

        private void Scan()
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
                    playerInput.deviceLostEvent.AddListener(OnDeviceLost);
                    break;
                case PlayerNotifications.InvokeCSharpEvents:
                    playerInput.onControlsChanged += OnControlsChanged;
                    playerInput.onDeviceLost += OnDeviceLost;
                    break;
                case PlayerNotifications.SendMessages:
                case PlayerNotifications.BroadcastMessages:
                    _messageBroker.OnControlsChangedMessage += OnControlsChanged;
                    _messageBroker.OnDeviceLostMessage += OnDeviceLost;
                    break;
            }
        }

        private void UnregisterPlayerInputEvents(PlayerInput playerInput)
        {
            switch (playerInput.notificationBehavior)
            {
                case PlayerNotifications.InvokeUnityEvents:
                    playerInput.controlsChangedEvent.RemoveListener(OnControlsChanged);
                    playerInput.deviceLostEvent.RemoveListener(OnDeviceLost);
                    break;
                case PlayerNotifications.InvokeCSharpEvents:
                    playerInput.onControlsChanged -= OnControlsChanged;
                    playerInput.onDeviceLost -= OnDeviceLost;
                    break;
                case PlayerNotifications.SendMessages:
                case PlayerNotifications.BroadcastMessages:
                    _messageBroker.OnControlsChangedMessage -= OnControlsChanged;
                    _messageBroker.OnDeviceLostMessage -= OnDeviceLost;
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

        private void OnDeviceLost(PlayerInput playerInput)
        {
            if (playerInput == PlayerInput)
            {
                OnDeviceLostEvent?.Invoke(playerInput);
            }
        }
        
        #if UNITY_EDITOR
        
        private void DebugDevices(PlayerInput playerInput, string info = "")
        {
            StringBuilder t = new($"[{playerInput.devices.Count}] {info}: ");
            foreach (var device in playerInput.devices)
            {
                t.Append($"{device.name}, ");
            }
            
            Debug.Log(t.ToString());
        }
        
        #endif
    }
}

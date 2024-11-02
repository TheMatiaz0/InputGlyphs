using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs.Display
{
    public class InputMessagePropagator : MonoBehaviour, IDisplayManager
    {
        public static IDisplayManager Manager { get; private set; }

        [SerializeField]
        private PlayerInput PlayerInput;

        private PlayerInput _lastPlayerInput;
        private List<IGlyphDisplay> glyphDisplays = new List<IGlyphDisplay>();

#if UNITY_EDITOR
        private void Reset()
        {
            PlayerInput = FindAnyObjectByType<PlayerInput>();
        }
#endif

        private void Awake()
        {
            Manager = this;
        }

        public void Register(IGlyphDisplay display)
        {
            glyphDisplays.Add(display);
        }

        public void Unregister(IGlyphDisplay display)
        {
            glyphDisplays.Remove(display);
        }

        private void Start()
        {
            if (PlayerInput == null && InputGlyphDisplaySettings.AutoCollectPlayerInput)
            {
                PlayerInput = PlayerInput.all.FirstOrDefault();
            }
            if (PlayerInput == null)
            {
                Debug.LogWarning("PlayerInput is not set.", this);
            }
        }

        private void Update()
        {
            if (PlayerInput == null && InputGlyphDisplaySettings.AutoCollectPlayerInput)
            {
                PlayerInput = PlayerInput.all.FirstOrDefault();
            }

            if (PlayerInput != _lastPlayerInput)
            {
                if (_lastPlayerInput != null)
                {
                    UnregisterPlayerInputEvents(_lastPlayerInput);
                }
                if (PlayerInput == null)
                {
                    Debug.LogError("PlayerInput is not set.", this);
                }
                else
                {
                    RegisterPlayerInputEvents(PlayerInput);
                    UpdateGlyphs(PlayerInput);
                }
                _lastPlayerInput = PlayerInput;
            }
        }

        private void UpdateGlyphs(PlayerInput playerInput)
        {
            foreach (var glyphDisplay in glyphDisplays)
            {
                if (glyphDisplay.IsVisible)
                {
                    glyphDisplay.UpdateGlyphs(playerInput);
                }
            }
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
                UpdateGlyphs(playerInput);
            }
        }

        // Support for PlayerNotifications.SendMessages AND PlayerNotifications.BroadcastMessages
        private void OnControlsChanged()
        {
            OnControlsChanged(PlayerInput);
        }
    }
}

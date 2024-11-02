#if INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using InputGlyphs.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputGlyphs.Display
{
    public class InputGlyphSprite : MonoBehaviour, IGlyphDisplay
    {
        public bool IsVisible => SpriteRenderer != null && SpriteRenderer.enabled && SpriteRenderer.isVisible;

        [SerializeField]
        public SpriteRenderer SpriteRenderer = null;

        [SerializeField]
        public InputActionReference InputActionReference = null;

        [SerializeField]
        public GlyphsLayoutData GlyphsLayoutData = GlyphsLayoutData.Default;

        private PlayerInput _lastPlayerInput;
        private List<string> _pathBuffer = new List<string>();
        private Texture2D _texture;

        private void Reset()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            if (SpriteRenderer == null)
            {
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }
            _texture = new Texture2D(2, 2);
        }

        private void OnDestroy()
        {
            Destroy(_texture);
            _texture = null;
            if (SpriteRenderer != null)
            {
                SpriteRenderer.sprite = null;
            }
        }

        public void UpdateGlyphs(PlayerInput playerInput)
        {
            if (!playerInput.isActiveAndEnabled)
            {
                return;
            }

            var devices = playerInput.devices;
            if (devices.Count == 0)
            {
                Debug.LogWarning("No devices are connected.", this);
                return;
            }

            if (InputLayoutPathUtility.TryGetActionBindingPath(InputActionReference?.action, playerInput.currentControlScheme, _pathBuffer))
            {
                if (DisplayGlyphTextureGenerator.GenerateGlyphTexture(_texture, devices, _pathBuffer, GlyphsLayoutData))
                {
                    SpriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), Mathf.Min(_texture.width, _texture.height));
                }
            }
        }
    }
}
#endif

#if INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using InputGlyphs.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputGlyphs.Display
{
    public class InputGlyphSprite : MonoBehaviour, IGlyphDisplay
    {
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

        private void OnEnable()
        {
            InputGlyphDisplayBridge.Register(this);
        }

        private void OnDisable()
        {
            InputGlyphDisplayBridge.Unregister(this);
        }

        public void UpdateGlyphs(ReadOnlyArray<InputDevice> devices, string controlScheme)
        {
            if (InputLayoutPathUtility.TryGetActionBindingPath(InputActionReference?.action, controlScheme, _pathBuffer))
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
